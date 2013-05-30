using UnityEngine;
using System.Collections;

public class FollowCursor : PositionCursor 
{
	private GameObject playerCursor;
	private RaycastHit rayHit;
	private Quaternion cursorPosition;
	private float traverseSpeed;
	
	public override void Start ()
	{	
		traverseSpeed = angleRotationSpeed / 90;
		playerCursor = GameObject.Find(gameObject.transform.root.name + "Cursor");
		base.Start ();
	}
	
	public override void Update ()
	{
		if(!Input.GetKey(KeyCode.Space))
		{
			GetCursorPosition();
			base.Update ();
		}
	}
	
	/*
	 var cursorImage : Texture;
 
	function Start() {
	    Screen.showCursor = false;
	5.}
	 
	function OnGUI() {
	    var mousePos : Vector3 = Input.mousePosition;
	    var pos : Rect = Rect(mousePos.x,Screen.height - mousePos.y,cursorImage.width,cursorImage.height);
	10.    GUI.Label(pos,cursorImage);
	}

	 */
	public override void RotateTurret()
	{
		if(!Input.GetKey(KeyCode.Space))
		{
			turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation,
				cursorPosition, Time.deltaTime * traverseSpeed  * 3	);
			
			Quaternion turretReset = turret.transform.localRotation;
			turretReset.x = 0;
			turretReset.z = 0;
			turret.transform.localRotation = turretReset;
		}
	}
	
	public override void ChangeDepression()
	{
		if(!Input.GetKey(KeyCode.Space))
		{
			if(transform.localEulerAngles.x < gunShieldMaxDown ||
				transform.localEulerAngles.x > 360 - gunShieldMaxUp)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation,
				cursorPosition, Time.deltaTime * traverseSpeed);
				
				Quaternion gunShieldReset = transform.localRotation;
				gunShieldReset.y = 0;
				gunShieldReset.z = 0;
				transform.localRotation = gunShieldReset;
			}
			else
			{
				Vector3 angle = transform.localEulerAngles;
	    		if(angle.x > 180)
				{
					angle.x += resetSpeed;
				}
				else
				{
					angle.x -= resetSpeed;
				}
	    		transform.localEulerAngles = angle;
			}
		}
	}
	
	private void GetCursorPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition );
		if(Physics.Raycast(ray.origin, ray.direction, out rayHit))
		{
			playerCursor.transform.position = rayHit.point;
		}
		cursorPosition = Quaternion.LookRotation(playerCursor.transform.position - turret.transform.position);	
	}
}