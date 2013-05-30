using UnityEngine;
using System.Collections;

public class FireShell : MonoBehaviour 
{
	public Rigidbody Shell;
	public GameObject SpawnPoint;
	public bool isAlive;
	public ParticleSystem smoke;
	public DataGun gunData;
	public float timeSinceLastShot;
	
	private bool isReloaded = true;
	
	public virtual void Start () 
	{
		audio.Stop ();
		smoke.Stop();
	}
	
	public virtual void Update () 
	{
		if(isAlive)
		{
			RefreshAmmo();
			if(Available())
			{
				InitializeFiring();
			}
		}
	}
	
	private void CreateEffect()
	{	
		audio.Play ();
		smoke.Play();
	}
	
	private Rigidbody CreateShell(){
		Rigidbody newShell = Instantiate(Shell,SpawnPoint.transform.position,SpawnPoint.transform.rotation) as Rigidbody;
		newShell.renderer.enabled=true;
		return newShell;
	}
	
	private Rigidbody LaunchShell(Rigidbody shell){
		shell.velocity = transform.TransformDirection(new Vector3(0,0,gunData.Power));
		return shell;
	}
	
	public virtual bool CheckReloadTime(){
		return timeSinceLastShot >= gunData.ReloadTime;
	}
	
	private bool CheckAmmo(){
		return gunData.ShellAmmo > 0;
	}
	
	private void RefreshAmmo()
	{
		gunData =(DataGun) gameObject.GetComponent("DataGun");
	}
	
	public void CheckAvailability()
	{
		isReloaded = CheckReloadTime();
		if(!isReloaded)
		{
			timeSinceLastShot += Time.deltaTime;
		}
	}
	
	public bool Available()
	{
		CheckAvailability();
		return isReloaded && CheckAmmo();
	}
	
	public virtual void InitializeFiring()
	{
		Rigidbody shell = CreateShell();
		shell = LaunchShell(shell);
		isReloaded = false;
		gunData.ShellAmmo -= 1;
		CreateEffect();
		timeSinceLastShot = 0;
	}
}
