using UnityEngine;
using System.Collections;

public class LaunchRocket : FireRocket 
{
	public BotDifficulty difficulty;
	
	private Transform turret;
	private Transform myTarget;
	private GameObject player;
	
	private int tankDistance;
	private int shootRange;
	
    public override void Start ()
	{
		difficulty = transform.root.GetComponent("BotDifficulty") as BotDifficulty;
		turret = GameObject.Find(gameObject.transform.root.name + "TurretControl").transform;
		myTarget = GameObject.Find(GameObject.FindGameObjectWithTag("Player").name + "Turret").transform;
		shootRange = difficulty.ShootDistance;
		
		player = GameObject.Find(GameObject.FindGameObjectWithTag("Player").name + "Turret");
	}
	
	public override void HatchControl ()
	{
		if(!isClosed)
		{
			gameObject.transform.RotateAroundLocal(Vector3.forward, Time.deltaTime);
		}
	}
	
	public override void InitializeFiring ()
	{
		if(isAlive && player.transform.root.GetComponent<TankCollection>().IsAlive)
		{
			tankDistance = (int)Vector3.Distance(turret.position, myTarget.position);
			if(tankDistance <= shootRange + 100)
			{
				base.InitializeFiring ();
			}
		}
	}
	
	public override bool CheckReloadTime ()
	{
		return timeSinceLastShot >= hatchControlData.ReloadTime + difficulty.ReactionTime;
	}
}
