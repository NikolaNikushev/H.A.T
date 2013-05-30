using UnityEngine;
using System.Collections;

public class DataHatchControl : MonoBehaviour 
{
	public double ReloadTime;
	public int Power;
	public int RocketAmmo;
	public DataRockets.RocketSize RocketCaliber;
	
	private DataRockets currentRocket;
	private int counter = 0;
	
	// Use this for initialization
	private void Start () 
	{
		if(!gameObject.GetComponent("DataRockets"))
		{
			currentRocket = gameObject.AddComponent("DataRockets") as DataRockets;
		}
		else if(currentRocket == null)
		{
			currentRocket = gameObject.AddComponent("DataRockets") as DataRockets;
		}
		else
		{
			currentRocket = gameObject.GetComponent("DataRockets") as DataRockets;
		}
		currentRocket.CurrentRocket = RocketCaliber;
	}
	
	public double GetReloadTime()
	{
		return this.ReloadTime;
	}
	
	public double RocketWeight() 
	{
		if(counter == 0)
		{
			Start ();
			currentRocket.Reload();
			counter++;
		}
		return this.currentRocket.Weight;
	} 
}
