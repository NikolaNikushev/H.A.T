using UnityEngine;
using System.Collections;

public abstract class PositionCursor : MonoBehaviour 
{
	public float angleRotationSpeed;
	public GameObject turret;
	public float gunShieldMaxUp;
	public float gunShieldMaxDown;
	public bool inverted = false;
	public bool isAlive;
	public bool IsJammed;
	
	public float resetSpeed = Time.deltaTime * 2;
	public float invertedConvert = 1;
	
	public virtual void Start () 
	{
		if(inverted)
		{
			invertedConvert = 1;
		}
		else
		{
			invertedConvert = -1;
		}
	}
	
	public virtual void Update () 
	{
		if(isAlive && !IsJammed)
		{
			RotateTurret();
			ChangeDepression();
		}
	}
	
	public float GetCurrentTraverse()
	{
		return transform.eulerAngles.y;
	}
	
	//True if in boundries
	public bool CheckTraverse()
	{
		bool result = false;
		return !result;
	}
	
	public float GetCurrentDepression()
	{
		return transform.eulerAngles.x;
	}
	
	//True if in boundries
	public bool CheckDepression()
	{
		bool result = false;
		return !result;
	}
	
	public virtual void RotateTurret()
    {
    	
    }
	
	public virtual void ChangeDepression()
	{
		
	}
}
