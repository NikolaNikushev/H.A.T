using UnityEngine;
using System.Collections;

public class BotDifficulty : MonoBehaviour 
{
	public enum Difficulty{
		Easy,
		Medium,
		Hard
	}
	
	public Difficulty botDifficulty;
	
	private int shootDistance;
	private float turretTraverseSpeed;
	private float reactionTime;
	
	public int ShootDistance 
	{
		get {return this.shootDistance;}
	}
	
	public float TurretTraverseSpeed
	{
		get {return this.turretTraverseSpeed;}
	}
	
	public float ReactionTime 
	{
		get {return this.reactionTime;}
	}
	
	private void Start () 
	{
		if (botDifficulty.ToString() == "Easy")
		{
			turretTraverseSpeed = 0.5f;
			reactionTime = 2.5f;
			shootDistance = 100;
		}
		else if (botDifficulty.ToString() == "Medium")
			{
			turretTraverseSpeed = 1f;
			reactionTime = 1.5f;
			shootDistance = 200;
		}
		else if (botDifficulty.ToString() == "Hard")
			{
			turretTraverseSpeed = 2f;
			reactionTime = 1f;
			shootDistance = 300;
		}
		else
		{
			Debug.Log("Difficulty not found");
		}
	}
}
