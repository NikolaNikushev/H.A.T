using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SetPosition : Movement 
{
	public float detectionRange = 25;
	public bool allowMove = true;
	public int currentWaypoint = 0;
	public List<Vector3> wayPoints = new List<Vector3>();
	public List<int> fakeWaypointsID = new List<int>();
	
	private const int EVENING_OUT_NUMBER = 40;
	private GameObject wayPointsObject;
	private GameObject pointsCollection;
	private float rotateTime;
	
	public override void Start ()
	{
		base.Start ();
		GetWaypoints();
	}
	 
	public override void MoveForward ()
	{
		float maxSpeeds;
		maxSpeeds = maxSpeed;
		if(allowMove == false)
		{
			if(maxSpeed - 10 > 0)
			{
				maxSpeed -= 10;
			}
			else
			{
				maxSpeed = 0;
			}
		}
		else
		{
			maxSpeed = maxSpeeds;
		}
			
		if( Quaternion.Angle(transform.rotation, new Quaternion(0,0,0,0)) > 0)
		{
			if(allowMove == true)
			{
				if (rotateTime >= 5)
				{
					base.MoveForward();
				}
			}
			else
			{
				moveDirection = Vector3.zero;
			}
		}
	}
	
	public override void MoveBackward ()
	{
		
	}
	
	public override void TurnLeft ()
	{
		
	}
	
	public override void TurnRight ()
	{
		
	}
	
	void LateUpdate()
	{
		//Nullify on the Y coordinate
		Vector3 tankPositionXZ = transform.position;
		tankPositionXZ.y = 0;
		Vector3 currentWayPointXZ = wayPoints[currentWaypoint];
		currentWayPointXZ.y = 0;
		
		if((int)Vector3.Distance(tankPositionXZ, currentWayPointXZ) <= detectionRange )
		{
			rotateTime = 0;
			if(fakeWaypointsID.Count == 0)
			{
				PickRandomPoint();
			}
			else
			{
				if(fakeWaypointsID.Count != 0)
				{
					RemoveFakeWaypoint();
					if(fakeWaypointsID.Count != 0)
					{
						currentWaypoint = fakeWaypointsID[0];
					}
				}
			}
		}
	
		float angle = Quaternion.Angle(transform.rotation, new Quaternion(0, 0, 0, 0));
		
		if( angle > 0)
		{	
			Quaternion targetRotation = Quaternion.LookRotation(wayPoints[currentWaypoint] -
				transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
				Time.deltaTime * rotationSpeed / EVENING_OUT_NUMBER);
			rotateTime += Time.deltaTime;
		}
	}
	
	public void ResetWaypoint()
	{
		currentWaypoint = 0;
		wayPoints.Clear();
		GetWaypoints();
	}
	
	private void GetWaypoints()
	{
		pointsCollection = GameObject.Find("WayPoints");
		Transform[] points = pointsCollection.GetComponentsInChildren<Transform>();
		
		int counter = 0;
		foreach (Transform point in points) 
		{
			if(counter > 0)
			{
				wayPoints.Add(point.position);
			}
			counter = 1;
		}
	}
	
	private void RemoveFakeWaypoint()
	{
		wayPoints.Remove(wayPoints[fakeWaypointsID[0]]);
		fakeWaypointsID.RemoveAt(0);
		for (int i = 0, length = fakeWaypointsID.Count; i < length; i++) 
		{
			if(fakeWaypointsID[i] > 0)
			{
				fakeWaypointsID[i] -= 1;
				if(fakeWaypointsID[i] < currentWaypoint)
				{
					fakeWaypointsID.Remove(fakeWaypointsID[i]);
				}
			}
		}	
	}
	
	public void PickRandomPoint()
	{
		int previousPoint = currentWaypoint;
		
		if(previousPoint >=5 && previousPoint <= 9)
		{
			do
			{
				currentWaypoint = UnityEngine.Random.Range(0, wayPoints.Count - 1);
			}while(currentWaypoint >= 1 && currentWaypoint <= 3);
		}
		else if(previousPoint >= 1 && previousPoint <= 3)
		{
			do
			{
				currentWaypoint = UnityEngine.Random.Range(0, wayPoints.Count - 1);
			}while(currentWaypoint >= 5 && currentWaypoint <= 9);
		}
		else
		{
			currentWaypoint = UnityEngine.Random.Range(0, wayPoints.Count - 1);
		}
	}
	
	public void ResetFakePoints()
	{
		if(fakeWaypointsID.Count > 0)
		{
			for (int i = 0, length = fakeWaypointsID.Count; i < length; i++) 
			{
				wayPoints.Remove(wayPoints[fakeWaypointsID[i]]);
				fakeWaypointsID.RemoveAt(i);
			}
		}
	}
}
