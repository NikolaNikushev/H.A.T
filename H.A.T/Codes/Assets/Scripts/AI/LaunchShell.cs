using UnityEngine;
using System.Collections;

public class LaunchShell : FireShell 
{
	public float turretTraverseSpeed;
	public BotDifficulty difficulty;
	
	private Transform turret;
	private Transform raySpawnPoint;
	private RaycastHit rayHit = new RaycastHit();
 	private Transform myTarget;
	private Vector3 target;
	private int shootRange;
	private int tankDistance;
	private SetPosition WithingRange;
	
	private GameObject player;
	
	private void Start()
	{
		turret = GameObject.Find (gameObject.transform.root.name + "TurretControl").transform;
		raySpawnPoint = GameObject.Find (gameObject.transform.root.name + "ShellSpawnPoint").transform;
		
		difficulty = transform.root.GetComponent("BotDifficulty") as BotDifficulty;
		turretTraverseSpeed = difficulty.TurretTraverseSpeed;
		shootRange = difficulty.ShootDistance;
		
		WithingRange = GameObject.FindGameObjectWithTag("Bot").GetComponent("SetPosition") as SetPosition;
		
		player = GameObject.Find(GameObject.FindGameObjectWithTag("Player").name + "Turret");
		myTarget = player.transform;
	}
	
	public override bool CheckReloadTime ()
	{
		return timeSinceLastShot >= gunData.ReloadTime + difficulty.ReactionTime;
	}

	public override void Update()
	{ 
		if(isAlive && player.transform.root.GetComponent<TankCollection>().IsAlive)
		{
			target = new Vector3(myTarget.position.x,myTarget.position.y, myTarget.position.z);
			
			tankDistance = (int)Vector3.Distance(turret.position, target);
			
			Vector3 viewCheck = raySpawnPoint.TransformDirection(0, 0, 1);
			
			Debug.DrawRay(raySpawnPoint.position, viewCheck * tankDistance, Color.red);
			
			if( Physics.Raycast(raySpawnPoint.position, viewCheck,out rayHit , tankDistance))
			{
				if(rayHit.collider.gameObject.transform.root.tag == "Player")
				{
					if (tankDistance <= shootRange)
					{
						base.Update();
						WithingRange.allowMove = false;
					}
				}
				else
				{
					WithingRange.allowMove = true;
				}
			}
		}
		else if (isAlive && !player.transform.root.GetComponent<TankCollection>().IsAlive)
		{
			WithingRange.allowMove = true;
			turret.rotation =  Quaternion.Slerp(turret.rotation, transform.root.transform.rotation, Time.deltaTime * turretTraverseSpeed );	
		}
	}
}
