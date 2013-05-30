using UnityEngine;
using System.Collections;

public class PlayerLaunchShell : FireShell 
{
	public override void InitializeFiring ()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			base.InitializeFiring ();
		}
	}
}