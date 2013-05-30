using UnityEngine;
using System.Collections;

public class DataGun : MonoBehaviour 
{
	//Tank gun info :
	public int AngleTraverse;
	public int AngleDepression;
	public int RotationSpeed;
	public double Weight;
	public double ReloadTime;
	public int Power;
	public int ShellAmmo;
	public DataShells.ShellSize ShellCaliber;
	
	private int counter = 0;
	private DataShells currentShell;
	
	// Use this for initialization
	private void Start () 
	{
		if(!gameObject.GetComponent("DataShells"))
		{
			currentShell = gameObject.AddComponent("DataShells") as DataShells;
		}
		else if(currentShell == null)
		{
			currentShell = gameObject.AddComponent("DataShells") as DataShells;
		}
		else
		{
			currentShell = gameObject.GetComponent("DataShells") as DataShells;
		}
		
		currentShell.CurrentShell = ShellCaliber;
		
		if(ShellCaliber.ToString() == DataShells.ShellSize.mm100.ToString())
		{
			Weight = 500;
		}
		else if(ShellCaliber.ToString() == DataShells.ShellSize.mm80.ToString())
		{
			Weight = 350;
		}
		else
		{
			Weight = 200;
		}
	}
	
	public double GetReloadTime
	{
		get { return this.ReloadTime; }
	}
	public double ShellWeight()
	{
		if(counter==0)
		{
			Start ();
			currentShell.Reload();
			counter++;
		}
		return this.currentShell.Weight; 
	}
	
	public void Reload()
	{
		Start();
	}
}
