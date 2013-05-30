using UnityEngine;
using System.Collections;

class GUIPlayer : MonoBehaviour 
{	
	public int currentHealth = 8;
	public float Health;
	public float MaxHealth;
	public int ShellAmmo;
	public int RocketAmmo;
	
	public Texture2D[] HealthBars = new Texture2D[9];	
	
	private float timer;

	private void Start()
	{
		CheckHealth();
	}
	
	private void Update()
	{
		HealthCheck();
	}
	
	private void OnGUI ()
	{
		DisplayHealth();
		DisplayAmmo();
	}
	
	private float getHealth()
	{
		return MaxHealth - MaxHealth*(0.1F * (9 - currentHealth));
	}

	void CheckHealth() 
	{
        InvokeRepeating("HealthCheck", 1, 0.1F);
    }
	
	private void HealthCheck()
	{
		if(Health < getHealth())
		{
			currentHealth--;
		}
	}
	
	private void DisplayHealth()
	{
		if(currentHealth > 0)
		{
			GUI.Box (new Rect(10, 10, 140, 60), HealthBars[currentHealth]);
		}
		else if(currentHealth == 0)
		{
			GUI.Box (new Rect(10, 10, 140, 60), HealthBars[1]);
		}
		else if (currentHealth == -1)
		{
			GUI.Box (new Rect(10, 10, 140, 60), HealthBars[0]);
			currentHealth = 1;
		}
		else 
		{
			GUI.Box (new Rect(10, 10, 140, 60), HealthBars[0]);
		}
		if(Health >= 0)
		{
			GUI.Label(new Rect(90, 50, 30, 30), ((int)Health).ToString(), GUIAssist.BiggerText(20));
		}
		else
		{
			GUI.Label(new Rect(90, 50, 30, 30), "DEAD", GUIAssist.BiggerText(20));
		}
	}
	
	private void DisplayAmmo()
	{
		GUI.Label(new Rect(10, Screen.height - 75, 75, 50),
			" Shells", GUIAssist.BiggerText(20));
		GUI.Box (new Rect(10, Screen.height - 50, 75, 50), "");
		GUI.Label (new Rect(15, Screen.height - 40, 75, 50),
			ShellAmmo.ToString(), GUIAssist.BiggerText(30));
		
		GUI.Label(new Rect(Screen.width - 85, Screen.height - 75, 75, 50),
			" Rockets", GUIAssist.BiggerText(20));
		GUI.Box (new Rect(Screen.width - 75, Screen.height - 50, 75, 50), "");
		GUI.Label (new Rect(Screen.width - 60, Screen.height - 40, 75, 50),
			RocketAmmo.ToString(), GUIAssist.BiggerText(30));
	}
}

	
	