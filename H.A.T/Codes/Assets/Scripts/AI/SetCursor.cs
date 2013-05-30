using UnityEngine;
using System.Collections;

public class SetCursor : PositionCursor 
{
	public BotDifficulty difficulty;
	
	private GameObject player;
	private Transform turretControl;
 	private Transform enemy;
	private Vector3 aimingPosition;
	
	private Quaternion targetRotation;
	private int shootRange;
	private int tankDistance;
	private float turretTraverseSpeed;
	
	private SetPosition WithinRange;

	public override void Start ()
	{
		turretControl = GameObject.Find (gameObject.transform.root.name + "TurretControl").transform;
		difficulty = transform.root.GetComponent("BotDifficulty") as BotDifficulty;
		turretTraverseSpeed = difficulty.TurretTraverseSpeed;
		shootRange = difficulty.ShootDistance;
		
		WithinRange = GameObject.FindGameObjectWithTag("Bot").GetComponent("SetPosition") as SetPosition;
		player = GameObject.Find(GameObject.FindGameObjectWithTag("Player").name + "Chassis");
		enemy = player.transform;
	}
	
    public override void ChangeDepression ()
	{
		aimingPosition = new Vector3(enemy.position.x,enemy.position.y + 5, enemy.position.z);
		Quaternion enemyTank = Quaternion.LookRotation(aimingPosition - transform.position );
		if(transform.localEulerAngles.x < gunShieldMaxDown ||
			transform.localEulerAngles.x > 360 - gunShieldMaxUp)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation,
			enemyTank, Time.deltaTime * turretTraverseSpeed);
			
			Quaternion gunShieldReset = transform.localRotation;
			gunShieldReset.y = 0;
			gunShieldReset.z = 0;
			transform.localRotation = gunShieldReset;
		}
		else
		{
			Vector3 angle = transform.localEulerAngles;
    		if(angle.x > 180)
			{
				angle.x += resetSpeed;
			}
			else
			{
				angle.x -= resetSpeed;
			}
    		transform.localEulerAngles = angle;
		}
	}
  
    public override void RotateTurret()
    {
		aimingPosition = new Vector3(enemy.position.x,enemy.position.y + 5, enemy.position.z);
		
		tankDistance = (int)Vector3.Distance(turretControl.position, enemy.position);
    	if (tankDistance <= shootRange + 50 )
		{
			if (enemy && player.transform.root.GetComponent<TankCollection>().IsAlive)
			{
				Quaternion viewTarget = Quaternion.LookRotation(aimingPosition - turretControl.position );
				TurretRotateTo(viewTarget);
			}
		}
		else if(isAlive)
		{
			WithinRange.allowMove = true;
			TurretRotateTo(transform.root.transform.rotation);
		}
    }

	private void TurretRotateTo(Quaternion aimingPosition)
	{
		turretControl.rotation = Quaternion.Slerp(turretControl.rotation, aimingPosition, Time.deltaTime * turretTraverseSpeed );	
		
		Quaternion turretReset = turretControl.localRotation;
		turretReset.x = 0;
		turretReset.z = 0;
		turretControl.localRotation = turretReset;
	}
 }
