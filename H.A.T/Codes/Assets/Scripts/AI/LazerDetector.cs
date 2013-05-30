using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LazerDetector : MonoBehaviour
{
	private int viewRange = 80;
	private SetPosition setPositionData;
	private bool goingLeft = false;
	private const int POINTER_SPEED = 500;
	private const int POINTER_WIDTH = 54;
	
	private Vector3 objectFound;
	private Vector3 currentObjectBeingDodged;
	
	private void Start()
	{
		setPositionData = gameObject.transform.root.GetComponent<SetPosition>();
	}
	
	private void Update()
	{
		//Move the ray
		goingLeft = GetDirection();
		
		Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector3.forward) * viewRange);
		Vector3 newPosition = gameObject.transform.localPosition;
		if(goingLeft)
		{
			newPosition.x += Time.deltaTime * POINTER_SPEED;
		}
		else
		{
			newPosition.x -= Time.deltaTime * POINTER_SPEED;
		}
		
		gameObject.transform.localPosition = newPosition;
		Ray searchRay = new Ray(gameObject.transform.position, transform.forward);
		RaycastHit hitObject = new RaycastHit();
		if(Physics.Raycast(searchRay, out hitObject, viewRange))
		{
			if(hitObject.transform.name != "Terrain" && hitObject.transform.tag != "wall" &&
				hitObject.transform.tag != "Player" && !hitObject.transform.name.Contains("Shell") &&
				!hitObject.transform.name.Contains("Rocket") && hitObject.transform.tag != "mountain")
			{
				objectFound = hitObject.transform.position;
				if(setPositionData.fakeWaypointsID.Count == 0 )
				{
					currentObjectBeingDodged = new Vector3(0, 0, 0);
				}
				if(currentObjectBeingDodged != objectFound)
				{
					setPositionData.ResetFakePoints();
					currentObjectBeingDodged = objectFound;
				
					Vector3 currentWaypoint = setPositionData.wayPoints[setPositionData.currentWaypoint];
					Vector3 newFirstPoint = GetPoint(hitObject, currentWaypoint, true);
					Vector3 newSecondPoint = GetPoint(hitObject, currentWaypoint, false);
					
					AddWaypoint(newFirstPoint, false);
					AddWaypoint(newSecondPoint, true);
				}
			}
		}
	}
	
	private Vector3 GetPoint(RaycastHit hitObject, Vector3 currentWaypoint, bool isFirstPoint)
	{
		Vector3 newPoint = hitObject.collider.bounds.center;
		//Based on a cube-like mesh
		//Left == left of the box; right == right of the box; back == back of the box, etc
		switch(GetSideOfInteraction(hitObject))
		{
		case "Left":
			switch(isFirstPoint)
			{
			case true:
				newPoint.x += hitObject.collider.bounds.extents.x + setPositionData.detectionRange;
				break;
			case false:
				newPoint.x -= hitObject.collider.bounds.extents.x + setPositionData.detectionRange;
				break;
			}
			newPoint = LeftRightZ(hitObject, newPoint, currentWaypoint);
			break;
			
		case "Right":
			switch(isFirstPoint)
			{
			case true:
				newPoint.x -= (hitObject.collider.bounds.extents.x + setPositionData.detectionRange);
				break;
			case false:
				newPoint.x += hitObject.collider.bounds.extents.x + setPositionData.detectionRange;
				break;
			}
			newPoint = LeftRightZ(hitObject, newPoint, currentWaypoint);
			break;
			
		case "Front":
			switch(isFirstPoint)
			{
			case true:
				newPoint.z -= (hitObject.collider.bounds.extents.z + setPositionData.detectionRange);
				break;
			case false:
				newPoint.z += hitObject.collider.bounds.extents.z + setPositionData.detectionRange;
				break;
			}
			newPoint = FrontBackX(hitObject, newPoint, currentWaypoint);
			break;
			
		case "Back":
			switch(isFirstPoint)
			{
			case true:
				newPoint.z += hitObject.collider.bounds.extents.z + setPositionData.detectionRange;
				break;
			case false:
				newPoint.z -= (hitObject.collider.bounds.extents.z + setPositionData.detectionRange);
				break;
			}
			newPoint = FrontBackX(hitObject, newPoint, currentWaypoint);
			break;
		default:
			break;
		}
		return newPoint;
	}
	
	//Based on a cube-like mesh
	private string GetSideOfInteraction(RaycastHit hitObject)
	{
		//It gets the cooridantes from between object + view range and object + it's collider frame
		//if the target is in that location -> it is on that side
		Vector3 colliderLocation = hitObject.collider.bounds.center;
		Vector3 extents = hitObject.collider.bounds.extents;
		Vector3 tankLocation = gameObject.transform.root.transform.position;
		
		if(colliderLocation.x + extents.x + viewRange * 2 >= tankLocation.x &&
			tankLocation.x >= colliderLocation.x + extents.x)
		{
			if(colliderLocation.z + extents.z + viewRange >= tankLocation.z &&
				tankLocation.z >= colliderLocation.z + extents.z + setPositionData.detectionRange)
			{
				return "Back";
			}
			else if(colliderLocation.z - extents.z - viewRange <= tankLocation.z &&
				tankLocation.z <= colliderLocation.z - extents.z - setPositionData.detectionRange)
			{
				return "Front";
			}
			return "Left";
		}
		else
		{
			if(colliderLocation.z + extents.z + viewRange >= tankLocation.z &&
				tankLocation.z >= colliderLocation.z + extents.z + setPositionData.detectionRange)
			{
				return "Back";
			}
			else if(colliderLocation.z - extents.z - viewRange <= tankLocation.z &&
				tankLocation.z <= colliderLocation.z - extents.z - setPositionData.detectionRange)
			{
				return "Front";
			}
			return "Right";
		}
		return "";
	}
	
	private void AddWaypoint(Vector3 waypoint, bool isSecondPoint)
	{
		if(!setPositionData.wayPoints.Contains(waypoint))
		{
			if(isSecondPoint)
			{
				setPositionData.wayPoints.Insert(setPositionData.currentWaypoint + 1, waypoint);
				setPositionData.fakeWaypointsID.Add(setPositionData.currentWaypoint + 1);
			}
			else
			{
				setPositionData.wayPoints.Insert(setPositionData.currentWaypoint, waypoint);
				setPositionData.fakeWaypointsID.Add(setPositionData.currentWaypoint);
			}
		}
	}
	
	private Vector3 FrontBackX(RaycastHit hitObject, Vector3 newPoint, Vector3 currentWaypoint)
	{
		if(currentWaypoint.x - hitObject.transform.position.x >= 0)
		{
			newPoint.x += hitObject.collider.bounds.extents.x + setPositionData.detectionRange;
		}
		else
		{
			newPoint.x -= (hitObject.collider.bounds.extents.x + setPositionData.detectionRange);
		}
		return newPoint;
	}
	
	private Vector3 LeftRightZ(RaycastHit hitObject, Vector3 newPoint, Vector3 currentWaypoint)
	{
		if(currentWaypoint.z - hitObject.transform.position.z >= 0)
		{
			newPoint.z += hitObject.collider.bounds.extents.z + setPositionData.detectionRange;
		}
		else
		{
			newPoint.z -= (hitObject.collider.bounds.extents.z + setPositionData.detectionRange);
		}
		return newPoint;
	}
	
	private bool GetDirection()
	{
		if(gameObject.transform.localPosition.x <= POINTER_WIDTH * -1 )
		{
			return true;
		}
		else if(gameObject.transform.localPosition.x >= POINTER_WIDTH)
		{
			return false;
		}
		else
		{
			return goingLeft;
		}
	}
}
