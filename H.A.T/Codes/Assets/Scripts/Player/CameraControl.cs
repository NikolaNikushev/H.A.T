using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	private int position;
	
	private Vector3 startPosition;
	
	private void Start () 
	{
		startPosition = transform.localPosition;
		position = 0;
		Vector3 currentTransform = new Vector3();
	}
	
	private void Update () 
	{
		if(Input.GetKeyDown(KeyCode.V))
		{
			position++;
			SetPosition();
		}
	}
	
	private void SetPosition()
	{
		if(position == 0)
		{
			transform.localPosition = startPosition;
		}
		else if(position == 1)
		{
			transform.localPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z / 2);
		}
		else if(position == 2)
		{
			transform.localPosition = new Vector3(startPosition.x, startPosition.y * 0.75f, startPosition.z / 3);
		}
		else if(position == 3)
		{
			transform.localPosition = new Vector3(startPosition.x, startPosition.y - startPosition.y * 0.3f, 0);
		}
		else
		{
			position = 0;
			SetPosition();
		}
	}
}
