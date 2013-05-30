using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : Movement 
{
	public override void MoveForward()
	{
		if(Input.GetKey(KeyCode.W)  || Input.GetKey(KeyCode.UpArrow))
		{
			base.MoveForward();
		}
		else
		{
			substractSpeed += (float)( weight / 2000 ) * Time.deltaTime;
		}
	}
	
	public override void MoveBackward ()
	{
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			base.MoveBackward();
		}
		else
		{
			backwardSubstractSpeed += (float)( weight / 750 ) * Time.deltaTime;
		}
	}
	
	public override void TurnLeft ()
	{
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			base.TurnLeft();
		}
	}
	
	public override void TurnRight ()
	{
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			base.TurnRight();
		}
	}
}
   