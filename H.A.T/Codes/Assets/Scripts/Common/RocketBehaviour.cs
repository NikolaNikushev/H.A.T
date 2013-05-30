using UnityEngine;
using System.Collections;

public class RocketBehaviour : MonoBehaviour 
{
	//Tracing system
	public GameObject targetPoint;
	public GameObject targetBotPoint;
	
	public DataRockets currentRocket;
	
	private Vector3 startPoint;
	private Vector3 currentPoint;
	private float distance;
	private CollisionFlags collisionFlags;
	
	private float timer;
	
	private Transform[] waypoints = new Transform[2];
	private float waypointRadius = 1f;
    private float damping = 0.1f;
    private bool loop = false;
    private float speed = 5;
    private bool faceHeading = true;
	private bool reachedDestination;
	
	private RaycastHit rayHit;
	private Vector3 currentHeading,targetHeading;
    private int targetwaypoint;
	
	void Start () 
	{
		targetPoint = GameObject.Find("TargetRocketPoint");
		targetBotPoint = GameObject.Find("TargetRocketBotPoint");
		
		startPoint = transform.position;
		gameObject.rigidbody.constraints = RigidbodyConstraints.None;
		waypoints[0] = transform;
		
		waypoints[1] = targetPoint.transform;
        currentHeading = transform.forward;
		
        targetwaypoint = 0;
		
		rigidbody.useGravity = false;
	}
	
	private void ControlRocket()
	{
		if(Input.GetButtonDown("Fire2") && gameObject.renderer.enabled)
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			
			if(Physics.Raycast(ray.origin, ray.direction, out rayHit))
			{
				targetPoint.transform.position = rayHit.point;
			}
		}
	}
	
	private void SetDestination()
	{
		if(gameObject.renderer.enabled && (int)timer % 2 == 0)
		{
			waypoints[1] = targetBotPoint.transform;
			BotDifficulty.Difficulty difficultyLevel = GameObject.Find("GameInformation").GetComponent<GameInformation>().difficultyLevel;
			
			BotDifficulty difficultySettings = gameObject.AddComponent("BotDifficulty") as BotDifficulty;
			difficultySettings.botDifficulty = difficultyLevel;
			
			Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
			
			target.x += Random.Range(-15 * difficultySettings.ReactionTime, 15 * difficultySettings.ReactionTime);
			target.y += Random.Range(-15 * difficultySettings.ReactionTime, 15 * difficultySettings.ReactionTime);
			target.z += Random.Range(-15 * difficultySettings.ReactionTime, 15 * difficultySettings.ReactionTime);
			
			targetBotPoint.transform.position = target;
		}
	}
	
	private void Update () 
	{
		if(gameObject.transform.root.tag == "Player")
		{
			ControlRocket();
		}
		else
		{
			SetDestination();
		}
		
		BasicBehaviour();
		
	}
	
	private void BasicBehaviour()
	{
		if(gameObject.renderer.enabled)
		{
			if(timer > 1)
			{
				gameObject.rigidbody.detectCollisions = true;
			}
			timer += Time.deltaTime;
			if(timer < 2.5)
			{
				transform.RotateAroundLocal(Vector3.right,1 * Time.deltaTime / 3);
			}
			else
			{
				rigidbody.useGravity = true;
    			rigidbody.velocity = currentHeading * speed * 2;
				
		        if(faceHeading)
				{
		            transform.LookAt(transform.position+currentHeading);
				}
				
				if(Vector3.Distance(transform.position, waypoints[targetwaypoint].position) <= waypointRadius)
		        {
		            targetwaypoint++;
		            if(targetwaypoint >= waypoints.Length)
		                targetwaypoint = 0;
	        	}
			}
			if(timer > 30)
			{
				Destroy(gameObject);
			}
		}
	}
	
	private void FixedUpdate ()
    {
        targetHeading = waypoints[targetwaypoint].position - transform.position;
 
        currentHeading = Vector3.Lerp(currentHeading,targetHeading,damping * Time.deltaTime);
    }
	
	private void OnCollisionEnter(Collision collision) {
		
		if(gameObject.renderer.enabled == true)
		{
			Destroy(gameObject);
			if(collision.gameObject.transform.root.GetChildCount() > 0)
			{
				if(collision.gameObject.transform.root.GetComponentInChildren<DataTank>())
				{
					collision.gameObject.transform.root.GetComponentInChildren<DataTank>().HealthAmount -= CalculateDamage();
				}
			}
		}
		/* TODO EXPLOSION
		float radius=5;
		float power=5;
		 Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders) {
            if (!hit)
            	if (hit.rigidbody)
                	hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
        */
	}
	
	private float CalculateDamage()
	{
		float damage = 0;
		
		damage += Random.Range((float)currentRocket.RocketDamageMin,
			(float)currentRocket.RocketDamageMax);
		if(damage < 0)
		{
			damage = 0;
		}
		return damage / 4;
	}
}