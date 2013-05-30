using UnityEngine;
using System.Collections;

public class HealtBar : MonoBehaviour 
{
	private float maxHealth;
	private Transform lookAt;
	private bool visible = false;
	
	private void Start()
	{
		lookAt = GameObject.FindGameObjectWithTag("Player").transform;
		maxHealth = transform.parent.GetComponent<DataTank>().HealthAmount;
	}
	
	private void Update () 
	{
		Rotate();
		GetSize();
		Show();
		Blink();
		ChangeVisibility();
	}
	
	private void Rotate()
	{
		transform.LookAt(lookAt.position);
	}
	
	private void GetSize()
	{
		float currentHealth = transform.parent.GetComponent<DataTank>().HealthAmount;
		if(currentHealth < 0)
		{
			currentHealth = 0;
		}
		Vector3 scale = transform.localScale;
		scale.x = 10 * (currentHealth / maxHealth);
		transform.localScale = scale;
	}
	
	private void Show()
	{
		gameObject.renderer.enabled = visible;
	}
	
	private void Blink()
	{
		if (Input.GetKey(KeyCode.Z))
		{
			visible = true;
		}
		if (Input.GetKeyUp(KeyCode.Z))
		{
			visible = false;
		}
	}
	
	private void ChangeVisibility()
	{
		if(Input.GetKeyDown(KeyCode.X))
		{
			visible = !visible;
		}
	}
}
