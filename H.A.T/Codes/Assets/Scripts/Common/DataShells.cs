using UnityEngine;
using System.Collections;

public class DataShells : MonoBehaviour 
{
	// shell types
	public enum ShellSize
	{
		mm40,
		mm50,
		mm80,
		mm100
	}
	
	public ShellSize CurrentShell;
	
	private double shellPenetrationMin;
	private double shellPenetrationMax;
	private double shellDamageMin;
	private double shellDamageMax; 
	private double reloadTime;
	
	private double weight;
	
	//Properties to be used later in calculations
	public double ShellPenetrationMax 
	{
		get{return this.shellPenetrationMax;}
	}
	
	public double ShellPenetrationMin 
	{
		get{return this.shellPenetrationMin;}
	}
	
	public double ShellDamageMax 
	{
		get{return this.shellDamageMax;}
	}
	
	public double ShellDamageMin
	{
		get{return this.shellDamageMin;}
	}
	
	public double Weight{
		get{return this.weight;}
	}
	
	public double ReloadTime
	{
		get{return this.reloadTime;}
	}
	
	// set current shell Data;
	private void ShellData()
	{
		if(this.CurrentShell.Equals(ShellSize.mm40))
		{
			this.shellDamageMax = 40;
			this.shellDamageMin = 30;
			this.shellPenetrationMin = 70;
			this.shellPenetrationMax = 140;
			this.weight = 0.8;
			reloadTime = 1;
		}
		else if(this.CurrentShell.Equals(ShellSize.mm50))
		{
			this.shellDamageMax = 70;
			this.shellDamageMin = 50;
			this.shellPenetrationMin = 70;
			this.shellPenetrationMax = 140;
			this.weight = 1;
			reloadTime = 2;
		}
		else if(this.CurrentShell.Equals(ShellSize.mm80))
		{
			this.shellDamageMax = 100;
			this.shellDamageMin = 70;
			this.shellPenetrationMin = 50;
			this.shellPenetrationMax = 200;
			this.weight = 1.6;
			reloadTime = 4;
		}
		else if(this.CurrentShell.Equals(ShellSize.mm100))
		{
			this.shellDamageMax = 150;
			this.shellDamageMin = 100;
			this.shellPenetrationMin = 150;
			this.shellPenetrationMax = 200;
			this.weight = 2;
			reloadTime = 6;
		}
		else
		{
			Debug.Log ("Shell not Found!");
		}
	}
	
	// Use this for initialization
	private void Start () 
	{
		ShellData();
	}
	
	public void Reload()
	{
		Start ();
	}
}
