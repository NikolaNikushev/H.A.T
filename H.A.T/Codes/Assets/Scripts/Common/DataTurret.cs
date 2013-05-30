using UnityEngine;
using System.Collections;

public class DataTurret : MonoBehaviour 
{	
	//Tank turret info :
	public int Weight; // in tons
	public int ArmorFront; //milimeters
	public int ArmorSide;
	public int ArmorBack;
	public int TraverseSpeed; // degree/sec
	public int RepairTime; // in secs
	public float Health; // health of the turret (100)
	public bool isJammed;
	
	private float timer;
	private float HealthSave;
	
	private void Start()
	{
		HealthSave = Health;
	}
	
	private void Update () 
	{
		if(Health <= 0)
		{
			isJammed = true;
		}
		if(isJammed)
		{
			timer+= Time.deltaTime;
			if (timer >= RepairTime / 4)
			{
				Health += HealthSave / 4;
				timer = 0;
			}
			if(Health >= HealthSave)
			{
				RestroreHealth();
				isJammed = false;
			}
		}
	}
	
	public void RestroreHealth()
	{
		Health = HealthSave;
	}
}
