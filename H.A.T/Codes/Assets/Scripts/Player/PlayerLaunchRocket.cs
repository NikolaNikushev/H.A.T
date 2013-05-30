using UnityEngine;
using System.Collections;

public class PlayerLaunchRocket : FireRocket 
{
	public override void HatchControl ()
	{
		if(Input.GetKey(KeyCode.P))
		{
			if(isClosed)
			{
				gameObject.transform.RotateAroundLocal(Vector3.back, Time.deltaTime);
			}
		}
		if(Input.GetKey(KeyCode.O))
		{
			if(!isClosed)
			{
				gameObject.transform.RotateAroundLocal(Vector3.forward, Time.deltaTime);
			}
		}
	}
	
	public override void InitializeFiring ()
	{
		if(Input.GetButtonDown("Fire2"))
		{
			base.InitializeFiring();
		}
	}
}
