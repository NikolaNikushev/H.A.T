using UnityEngine;
using System.Collections;

public class DataTracks : MonoBehaviour 
{
	//Tank tracks info :
	public double Weight; // in tons
	public bool Detracked; // are tracks damaged or not
	public int MaxSpeed; // in KM/H
	public float Health; // health of the tracks (100)
	public int RepairTime; // in secs
	public int TraverseSpeed; // degree/sec
	public double WeightCapacity; //in tons
	public bool isGrounded;
	
	private float timer;
	private float HealthSave;
	
	private MeshFilter trackMesh;
	private MeshFilter normalTrackLeft;
	private MeshFilter normalTrackRight;
	private MeshFilter detrackLeft;
	private MeshFilter detrackRight;
	
	// Use this for initialization
	private void Start () 
	{
		detrackLeft = GameObject.Find(gameObject.transform.root.name + "TrackLeftDetrack")
			.GetComponent("MeshFilter") as MeshFilter;
		detrackRight = GameObject.Find(gameObject.transform.root.name + "TrackRightDetrack").
			GetComponent("MeshFilter") as MeshFilter;
		trackMesh = gameObject.GetComponent("MeshFilter") as MeshFilter;
		normalTrackLeft = GameObject.Find(gameObject.transform.root.name + "TrackNormalLeft").
			GetComponent("MeshFilter") as MeshFilter;
		normalTrackRight = GameObject.Find(gameObject.transform.root.name + "TrackNormalRight").
			GetComponent("MeshFilter") as MeshFilter;
		HealthSave = Health;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if (Health <= 0)
		{
			Detracked = true;
		}
		if (Detracked)
		{
			timer += Time.deltaTime;
			if(gameObject.name.Contains("Left"))
			{
				trackMesh.mesh = detrackLeft.mesh;
			}
			else if(gameObject.name.Contains("Right"))
			{
				trackMesh.mesh = detrackRight.mesh;
			}
			if (timer >= RepairTime / 4)
			{
				Health += HealthSave / 4;
				timer = 0;
			}
			if(Health >= HealthSave)
			{
				ResetHealth();
				Detracked = false;
				if(gameObject.name.Contains("Left"))
				{
					trackMesh.mesh = normalTrackLeft.mesh;
				}
				else if(gameObject.name.Contains("Right"))
				{
					trackMesh.mesh = normalTrackRight.mesh;
				}
			}
		}
	}
	
	private void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name == "Terrain")
		{
			isGrounded = false;
		}
    }
	
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.name == "Terrain")
		{
			isGrounded = true;
		}
    }
	
	public void ResetHealth()
	{
		Health = HealthSave;
	}
}
