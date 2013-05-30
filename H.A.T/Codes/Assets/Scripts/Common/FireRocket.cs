using UnityEngine;
using System.Collections;

public abstract class FireRocket : MonoBehaviour 
{
	public Rigidbody Rocket;
	public GameObject SpawnPointRocket;
	public bool isAlive;
	
	public bool isClosed = false;
	public bool shootRocket = true;
	public bool readyToBeReloaded = false;
	
	public DataHatchControl hatchControlData;
	public bool isReloaded = true;
	public double timeSinceLastShot;
	
	public virtual void Start()
	{
		
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if(isAlive)
		{
			HatchControl();
			RefreshAmmo();
			CheckAvailability();
			if(Available())
			InitializeFiring();
		}
	}
	
	public bool CheckIsHatchOpen(bool current)
	{
		if ((int)gameObject.transform.localEulerAngles.x > 0) 
		{
			shootRocket = false;
		}
		if((int)gameObject.transform.localEulerAngles.x <= 0)
		{
			shootRocket = true;
			return true;
		}
		else if((int)gameObject.transform.localEulerAngles.x >= 90 )
		{
			shootRocket = false;
			return false;
		}
		else
		{
			return current;
		}
	}
	
	public void CreateEffect()
	{
		audio.Play ();
	}
	
	public Rigidbody CreateRocket()
	{
		Rigidbody newRocket = Instantiate(Rocket, SpawnPointRocket.transform.position, SpawnPointRocket.transform.rotation) as Rigidbody;
		newRocket.renderer.enabled = true;
		newRocket.detectCollisions = false;
		return newRocket;
	}
	
	public Rigidbody LaunchRocket(Rigidbody rocket)
	{
		rocket.velocity = transform.TransformDirection(new Vector3(hatchControlData.Power, 0, 0));
		return rocket;
	}
	
	public bool CheckAmmo()
	{
		return hatchControlData.RocketAmmo > 0;
	}
	
	public virtual bool CheckReloadTime()
	{
		return timeSinceLastShot >= hatchControlData.ReloadTime;
	}
	
	public void RefreshAmmo()
	{
		hatchControlData = (DataHatchControl) gameObject.GetComponent("DataHatchControl");
	}
	
	public void CheckAvailability()
	{
		isClosed = CheckIsHatchOpen(isClosed);
		isReloaded = CheckReloadTime();
		if(!isReloaded)
		{
			timeSinceLastShot += Time.deltaTime;
		}
	}
	
	public bool Available()
	{
		return isReloaded && CheckAmmo() && shootRocket;
	}
	
	public virtual void HatchControl()
	{
		
	}
	
	public virtual void InitializeFiring()
	{
		Rigidbody rocket = CreateRocket();
		rocket = LaunchRocket(rocket);
		rocket.tag = transform.root.transform.tag;
		isReloaded = false;
		hatchControlData.RocketAmmo -= 1;
		timeSinceLastShot = 0;
		CreateEffect ();
	}
}
