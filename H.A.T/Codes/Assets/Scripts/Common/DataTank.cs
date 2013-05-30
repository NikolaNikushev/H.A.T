using UnityEngine;
using System.Collections;

public class DataTank : MonoBehaviour
{
	//Tank chasis info :
	public double ReloadTimeRocket; //in secs
	public double Weight; // in tons
	public double MaxWeight;
	public float HealthAmount;
	public int ArmorFront; //milimeters
	public int ArmorSide;
	public int ArmorBack;
	public bool IsAlive{ get{ return this.HealthAmount > 0; } }
	private float MaxHealth;
	public float horsePower; // TODO: Different engines
	
	public bool OverWeight;
	
	private void Start () 
	{
		MaxHealth = HealthAmount;
		CheckWeight();
	}
	
	private void Update () 
	{
		CheckWeight(); // This might never occur...... in the current scenario -> idea for : pickup of BONUS ammo
	}
	
	private void CheckWeight()
	{
		if(this.MaxWeight < this.Weight)
		{
			this.OverWeight = true;
		}
	}
	
	public void RestoreHealth()
	{
		this.HealthAmount = MaxHealth;
	}
}
