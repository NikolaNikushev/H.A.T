using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour 
{
	private float CurrentRange;
	
	private void Update () 
	{
		CurrentRange = Camera.mainCamera.fieldOfView;
		if (Input.GetAxis("Mouse ScrollWheel") > 0  && CurrentRange > 10)
		{
			Camera.mainCamera.fieldOfView -= 10;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && CurrentRange < 100)
		{
			Camera.mainCamera.fieldOfView += 10;
		}
		if(Input.GetMouseButton(2))
		{
			Camera.mainCamera.fieldOfView = 60;
		}
	}
}
