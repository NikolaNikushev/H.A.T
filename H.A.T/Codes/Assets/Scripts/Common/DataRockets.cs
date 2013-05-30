using UnityEngine;
using System.Collections;

public class DataRockets : MonoBehaviour 
{
	public enum RocketSize
	{
		mm40,
		mm50,
		mm80,
		mm100
	}
	public RocketSize CurrentRocket;
	
	private double rocketPenetrationMin;
	private double rocketPenetrationMax;
	private double rocketDamageMin;
	private double rocketDamageMax; 
	private double blastRadius;
	private double reloadTime;
	
	private double weight;
	
	//Properties to be used later in calculations
	public double RocketPenetrationMax 
	{
		get{return this.rocketPenetrationMax;}
	}
	
	public double RocketPenetrationMin 
	{
		get{return this.rocketPenetrationMin;}
	}
	
	public double RocketDamageMax 
	{
		get{return this.rocketDamageMax;}
	}
	
	public double RocketDamageMin
	{
		get{return this.rocketDamageMin;}
	}
	
	public double BlastRadius
	{
		get{return this.blastRadius;}
	}
	
	public double Weight
	{
		get{return this.weight;}
	}
	
	public double ReloadTime
	{
		get{return this.reloadTime;}
	}
	
	// set current rocket Data;
	private void rocketData()
	{
		if(this.CurrentRocket.Equals(RocketSize.mm40))
		{
			this.rocketDamageMax = 170;
			this.rocketDamageMin = 70;
			this.rocketPenetrationMin = 70;
			this.rocketPenetrationMax = 140;
			this.blastRadius = 2;
			this.weight = 40;
			reloadTime = 7;
		}
		else if(this.CurrentRocket.Equals(RocketSize.mm50))
		{
			this.rocketDamageMax = 200;
			this.rocketDamageMin = 100;
			this.rocketPenetrationMin = 70;
			this.rocketPenetrationMax = 140;
			this.blastRadius = 3;
			this.weight = 50;
			reloadTime = 10;
		}
		else if(this.CurrentRocket.Equals(RocketSize.mm80))
		{
			this.rocketDamageMax = 500;
			this.rocketDamageMin = 100;
			this.rocketPenetrationMin = 50;
			this.rocketPenetrationMax = 200;
			this.blastRadius = 5;
			this.weight = 80;
			reloadTime = 18;
		}
		else if(this.CurrentRocket.Equals(RocketSize.mm100))
		{
			this.rocketDamageMax = 600;
			this.rocketDamageMin = 300;
			this.rocketPenetrationMin = 150;
			this.rocketPenetrationMax = 200;
			this.blastRadius = 8;
			this.weight = 100;
			reloadTime = 20;
		}
		else
		{
			Debug.Log ("Rocket not Found!");
		}
	}
	
	// Use this for initialization
	private void Start () 
	{
		rocketData();
	}
	
	public void Reload()
	{
		Start ();
	}
}
