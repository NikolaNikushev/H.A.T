using UnityEngine;
using System.Collections;

public class BotBehaviour : MonoBehaviour 
{	
	public BotDifficulty difficultyLevel;
	public Transform target; 
	public float moveSpeed = 3; 
	public float rotationSpeed = 3; 
	public Transform myTransform; 
	
	private float timer;
	
	 void Start()
	 {
		difficultyLevel = gameObject.GetComponent("BotDifficulty") as BotDifficulty;
		myTransform = transform; 
	 	target = GameObject.FindWithTag("Player").transform; 
	 }
	
	 void Update () 
	{
		if(difficultyLevel == null)
		{
			difficultyLevel = gameObject.GetComponent("BotDifficulty") as BotDifficulty;
		}
		timer += Time.deltaTime;

		if((int)timer % (int)difficultyLevel.ReactionTime == 0)
		{
			 Vector3 lookDir = target.position - myTransform.position;
			 lookDir.y = 0; // zero the height difference
			 myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
			 Quaternion.LookRotation(lookDir), rotationSpeed * Time.deltaTime);
			 myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		}
	}
}
