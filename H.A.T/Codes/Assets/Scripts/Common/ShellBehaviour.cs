using UnityEngine;
using System.Collections;

public class ShellBehaviour : MonoBehaviour 
{
	public DataShells currentShell;
	
	private Vector3 startPoint;
	private Vector3 currentPoint;
	private float distance;
	private CollisionFlags collisionFlags;
	
	private void Start () 
	{
		startPoint = transform.position;
		gameObject.rigidbody.constraints = RigidbodyConstraints.None;
	}
	
	private void Update () 
	{
		if(gameObject.renderer.enabled)
		{
			gameObject.rigidbody.detectCollisions = true;
			
			distance = Vector3.Distance(transform.position, startPoint);
			if(distance < 300)
			{
				gameObject.rigidbody.useGravity = false;
			}
			else if(distance > 1500)
			{
				Destroy(gameObject);
			}
			else
			{
				gameObject.rigidbody.useGravity = true;
				rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity); 
			}
		}
		else
		{
			gameObject.rigidbody.detectCollisions = false;
		}
	}
	
	private void OnCollisionEnter(Collision collision) {
		
		string contactPoint = GetContactPoint(collision);
		
		string user = gameObject.name.Replace("(Clone)","").Replace("Shell","");
		string gun = "Gun";
		
		if(gameObject.renderer.enabled == true &&
			collision.gameObject.name != user + gun &&
			collision.gameObject.name != user)
		{
			Destroy(gameObject);
			if(collision.gameObject.transform.root.GetChildCount() > 0)
			{
				switch(contactPoint)
				{
				case "LeftTrack":
					DamageTrack("Left", collision);
					break;
				case "RightTrack":
					DamageTrack("Right", collision);
					break;
				case "Turret":
					DamageTurret(collision);
					break;
				 default:
					DamageChassis(collision.gameObject.transform);
					break;
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
				
	private string GetContactPoint(Collision collision)
	{
		foreach (ContactPoint cont in collision.contacts) 
		{
			if(cont.otherCollider.name.Contains("Track"))
			{
				if(cont.otherCollider.name.Contains("Left"))
				{
					return "LeftTrack";
				}
				else
				{
					return "RightTrack";
				}
			}
			if(cont.otherCollider.name.Contains("Turret"))
			{
				return "Turret";
			}
		}		
			return "";		
	}
	
	private void DamageChassis(Transform collisionObjectTransform)
	{
		if(collisionObjectTransform.root.GetComponentInChildren<DataTank>())
		{
			DataTank collisionTargetData = collisionObjectTransform.root.
				GetComponentInChildren<DataTank>();
			Vector3 sideOfCollision = collisionObjectTransform.localPosition;
			
			Vector3 difference = sideOfCollision - transform.localPosition;
			
			int armor = GetChassisArmor(difference, collisionTargetData);
			collisionTargetData.HealthAmount -= CalculateDamage(armor);
		}
	}
	
	private void DamageTurret(Collision collision)
	{
		DataTurret collisionTargetData = collision.gameObject.transform.root.
			GetComponentInChildren<DataTurret>();
		Vector3 sideOfCollision = collision.gameObject.transform.localPosition;
		Vector3 difference = sideOfCollision - transform.localPosition;
		int armor = GetTurretArmor(difference, collisionTargetData);
		
		float damage = CalculateDamage(armor);
		GameObject.Find(collision.gameObject.transform.root.name + "Turret").
			GetComponent<DataTurret>().Health -= damage;
		SmallDamageToChassis(damage, collision);
		
	}
	
	private void DamageTrack(string side, Collision collision)
	{
		float damage = CalculateDamage(200);
		GameObject.Find(collision.gameObject.transform.root.name + "Track" + side).
			GetComponent<DataTracks>().Health -= damage;
		SmallDamageToChassis(damage, collision);
	}
	
	private void SmallDamageToChassis(float damage, Collision collision)
	{	
		collision.gameObject.transform.root.
		GetComponentInChildren<DataTank>().HealthAmount -= damage / 10;
	}
	
	private int GetChassisArmor(Vector3 side, DataTank tankArmors)
	{
		//Based for PANTHER
		
		//if (side.y < -10 || side.y > 10) //Up Below
	    //	return 0;
		
		if (side.x > 0) //Left
		{
			if (side.z > 8)
			{
	    		return tankArmors.ArmorBack;
			}
			else if ( side.z < -11)
			{
				return tankArmors.ArmorFront;
			}
    		return tankArmors.ArmorSide;
		}
	    else //Right
		{
	    	if (side.z > 8)
			{
	    		return tankArmors.ArmorBack;
			}
			else if ( side.z < -11)
			{
				return tankArmors.ArmorFront;
			}
    		return tankArmors.ArmorSide;
		}
	}
	
	private int GetTurretArmor(Vector3 side, DataTurret turretArmors)
	{
		//if (side.y < -12 || side.y > 12) //Up Below
	    //	return 0;
		if (side.x > 0) //Left
		{
			if (side.z >= 4)
			{
	    		return turretArmors.ArmorBack;
			}
			else if ( side.z <= -4)
			{
				return turretArmors.ArmorFront;
			}
    		return turretArmors.ArmorSide;
		}
	    else //Right
		{
	    	if (side.z >= 4)
			{
	    		return turretArmors.ArmorBack;
			}
			else if ( side.z <= -4)
			{
				return turretArmors.ArmorFront;
			}
    		return turretArmors.ArmorSide;
		}
	
	}
	
	private float CalculateDamage(int armor)
	{
		float damage = 0;
		float reduction = 100;
		while(reduction < distance)
		{
			damage -= 10;
			reduction += 200;
		}
		damage += (float)Random.Range((float)currentShell.ShellDamageMin,
			(float)currentShell.ShellDamageMax);
		//Damage bonus from penetration
		damage += (Random.Range((float)(currentShell.ShellPenetrationMin - armor),
			(float)(currentShell.ShellPenetrationMax - armor))
			* ((float)currentShell.ShellPenetrationMin /
			(float)currentShell.ShellPenetrationMax)) / 2;
		
		if(Mathf.Abs((float)(armor - currentShell.ShellPenetrationMax)) <
			currentShell.ShellPenetrationMin * 2)
		{
			if(damage > currentShell.ShellPenetrationMax * 2)
			{
				damage = damage * 0.75f;
			}
		}
		
		if (damage < 0)
		{
			damage = 0;
		}
		return damage;
	}
}
