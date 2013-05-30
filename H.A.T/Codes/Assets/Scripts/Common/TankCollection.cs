using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	//GET ALL DATA FOR THE CURRENT TANK , everything is kept here, so that the GameObject.Find function is called only once for
	//every part of the tank.
	// Then everything is spreaded out to the components
	// this structure allows for many tanks to be created, independantly of one another

// Everything could have been into different classes :|
// Class : Tank with child Panther, KV1
// Class : Emmiters ; AudioSources
//OVERALL : Their constructors and fields are here

public class TankCollection : MonoBehaviour 
{
	public enum TankPreset
	{
		Panther,
		KV1,
		KingTiger,
		IS3,
		T44,
		Custom
	}
	
	public enum Controller
	{
		Player,
		Bot
	}
	
	public Controller controlledBy;
	public TankPreset tankPreset;
	
	public bool showChassisProps;
	public bool showTurretProps;
	public bool minigunActive;
	public bool sideSkirtsActive;
	
	public int rocketCount = 6;
	public DataShells.ShellSize shellCaliber;
	public DataRockets.RocketSize rocketCaliber;
	
	public float gameGravity = 80f;
	
	public bool paused;
	public bool IsAlive = true;
	
	private string user;
	
	private DataGun gunData;
	private DataTurret turretData;
	private DataTank tankData;
	private DataTracks tracksLeftData;
	private DataTracks tracksRightData;
	private Movement movement;
	private PositionCursor positionCursorData;
	private FireShell fireShellData;
	private FireRocket fireRocketData;
	private DataHatchControl hatchControlData;
	private DataRockets rocketData;
	private DataShells shellData;
	private ShellBehaviour shellBehaviourData;
	private RocketBehaviour rocketBehaviourData;
	private LazerDetector lazerDetectorData;

	private GameObject chassis;
	private GameObject miniGun;
	private GameObject sideSkirts;
	private GameObject trackLeft;
	private GameObject trackRight;
	private GameObject turret;
	private GameObject turretControl;
	private GameObject gunShield;
	private GameObject gunShieldControl;
	private GameObject gun;
	private GameObject shell;
	private GameObject hatchControl;
	private GameObject rocket;
	private GameObject hatchOpener;
	private GameObject hatch;
	private GameObject bonusProps;
	private GameObject bonusProps_1;
	private GameObject trackWheelsL;
	private GameObject trackWheelsR;
	
	private Transform[] trackWheelsLeft = new Transform[11];
	private Transform[] trackWheelsRight = new Transform[11];
	
	private ParticleSystem smokeExhaustLeft;
	private ParticleSystem smokeExhaustRight;
	private ParticleSystem dirtForwardLeft;
	private ParticleSystem dirtForwardRight;
	private ParticleSystem dirtBackwardLeft;
	private ParticleSystem dirtBackwardRight;
	private ParticleSystem gunSmoke;
	
	private ParticleSystem smokeDead;
	
	private float timer;

	private float HealthSave;
	
	private AudioSource gunFire;
	private AudioSource rocketFire;
	private AudioSource engine;
	private AudioSource engineMoving;
	
	//Gui
	private GUIPlayer playerGUI;
	private GUITexture playerGuiTexture;
	
	private void Start () 
	{
		Physics.gravity = new Vector3 ( 0, -gameGravity * 1.5f, 0);
		
		user = gameObject.name;
		
		int counter = 0;
		foreach (var item in gameObject.GetComponentsInChildren<Transform>()) 
		{
			if(counter != 0 && !item.name.Contains("Camera"))
			{
				item.name = user + item.name;
			}
			counter++;
		}
		
		//GET objects
		 //Chassis
		 chassis = GameObject.Find(user + "Chassis");
		 //MinigGun
		 miniGun = GameObject.Find(user + "MiniGun");
		 //SideSkirts
		 sideSkirts = GameObject.Find(user + "SideSkirts");
		 //TrackLeft
		 trackLeft = GameObject.Find(user + "TrackLeft");
		 //TrackRight
		 trackRight = GameObject.Find(user + "TrackRight");
		 //Turret
		 turret = GameObject.Find(user + "Turret");
		 //turretControl
		 turretControl = GameObject.Find(user + "TurretControl");
		 //GunShield
		 gunShield = GameObject.Find(user + "GunShield");
		 //GunShieldControl
		 gunShieldControl = GameObject.Find(user + "GunShieldControl");
		//Gun
		 gun = GameObject.Find(user + "Gun");
		//Shell
		 shell = GameObject.Find(tankPreset.ToString() + "Shell");
		//HatchControl
		 hatchControl = GameObject.Find(user + "HatchControl");
		//Rocket
		 rocket = GameObject.Find(tankPreset.ToString() + "Rocket");
		//HatchOpener
		 hatchOpener = GameObject.Find(user + "HatchOpener");
		//Hatch
		 hatch = GameObject.Find(user + "Hatch");
		//BonusProps ( chassis )
		 bonusProps = GameObject.Find(user + "BonusProps");
		//BonusProps_1 ( turret )
		 bonusProps_1 = GameObject.Find(user + "BonusProps_1");
		
		//Emmiters
		smokeExhaustLeft = GameObject.Find(user + "LeftExhaust").GetComponent("ParticleSystem") as ParticleSystem;
		smokeExhaustRight = GameObject.Find(user + "RightExhaust").GetComponent("ParticleSystem") as ParticleSystem;
		//For when going forward
		dirtForwardLeft = GameObject.Find(user + "TrackForwardLeft").GetComponent("ParticleSystem") as ParticleSystem;
		dirtForwardRight = GameObject.Find(user + "TrackForwardRight").GetComponent("ParticleSystem") as ParticleSystem;
		//For when going backward
		dirtBackwardLeft = GameObject.Find(user + "TrackBackLeft").GetComponent("ParticleSystem") as ParticleSystem;
		dirtBackwardRight = GameObject.Find(user + "TrackBackRight").GetComponent("ParticleSystem") as ParticleSystem;
		gunSmoke = GameObject.Find(user + "GunSmoke").GetComponent("ParticleSystem") as ParticleSystem;
		
		smokeDead = GameObject.Find(user + "DeadSmoke").GetComponent("ParticleSystem") as ParticleSystem;
		
		//TrackWheels
		trackWheelsL = GameObject.Find(user+"TrackWheelsLeft");
		trackWheelsR = GameObject.Find(user+"TrackWheelsRight");
		
		Transform[] wheels = trackWheelsL.GetComponentsInChildren<Transform>();
		for (int i = 0; i < wheels.Length; i++)
		{
			trackWheelsLeft[i] = wheels[i];
		}
		
		wheels =  trackWheelsR.GetComponentsInChildren<Transform>();
		for (int i = 0; i < wheels.Length; i++)
		{
			trackWheelsRight[i] = wheels[i];
		}
		
		//Create Data Module
		
		//Chassis	
		tankData = chassis.AddComponent("DataTank") as DataTank;
		engine = chassis.AddComponent("AudioSource") as AudioSource;
		engineMoving = gameObject.AddComponent("AudioSource") as AudioSource;
		
		//MinigGun
		if(minigunActive)
		{
			miniGun.renderer.enabled = true;
		}
		else
		{
			miniGun.renderer.enabled = false;
		}
		
		//SideSkirts
		if(sideSkirtsActive)
		{
			sideSkirts.renderer.enabled = true;
		}
		else
		{
			sideSkirts.renderer.enabled = false;
		}
		
		//TrackLeft
		tracksLeftData = trackLeft.AddComponent("DataTracks") as DataTracks;
		
		//TrackRight
		tracksRightData = trackRight.AddComponent("DataTracks") as DataTracks;	
		
		//Turret
		turretData = turret.AddComponent("DataTurret") as DataTurret;
		
		//Turret Control
		
		//GunShield
		
		//Gun
		gunData = gun.AddComponent("DataGun") as DataGun;
		gunFire = gun.AddComponent("AudioSource") as AudioSource;
		
		//HatchControl
		rocketFire = hatchControl.AddComponent("AudioSource") as AudioSource;
		
		//HatchOpener
		//Hatch
		//BonusProps ( chassis 
		if(showChassisProps)
		{
			bonusProps.renderer.enabled = true;
		}
		else
		{
			bonusProps.renderer.enabled = false;
		}
		
		//BonusProps_1 ( turret 
		if(showTurretProps)
		{
			bonusProps_1.renderer.enabled = true;
		}
		else
		{
			bonusProps_1.renderer.enabled = false;
		}
		
		//LoadData
		if(tankPreset.ToString() == "Panther")
		{
			PantherPreset();
		}
		else if(tankPreset.ToString() == "KV1")
		{
			KV1Preset();
		}
		else if(tankPreset.ToString() == "KingTiger")
		{
			KingTigerPreset();
		}
		else if(tankPreset.ToString() == "IS3")
		{
			IS3Preset();
		}
		else if(tankPreset.ToString() == "T44")
		{
			T44Preset();
		}
		else if(tankPreset.ToString()== "Custom")
		{
			CustomPreset();
		}
		else
		{
			TankNotFound();
		}
	}
	
	private void Update () 
	{
		if(controlledBy.ToString() == "Player")
		{
			PlayerUpdate();
		}
		else
		{
			BotUpdate();
		}
		CommonUpdate();
	}
	
	private void CommonUpdate()
	{
		IsAlive = tankData.IsAlive;
		if(tracksLeftData.isGrounded || tracksRightData.isGrounded)
		{
			movement.isGrounded = true;
		}
		if(!movement.isGrounded)
		{
			timer += Time.deltaTime;
		}
		else
		{
			if(timer >= 2)
			{
				timer -= 2;
				tankData.HealthAmount -=  timer * timer * timer * (float)tankData.Weight * Time.deltaTime;
				timer = 0;
			}
		}
		if (tankData.OverWeight)
		{
			float overWeightMass = (float)(tankData.Weight - tankData.MaxWeight);
			tankData.HealthAmount -= Time.deltaTime * (overWeightMass);
			tracksLeftData.Health -= (Time.deltaTime * (overWeightMass)) / 4;
			tracksRightData.Health -= (Time.deltaTime * (overWeightMass)) / 4;
		}
		if(!IsAlive || paused)
		{	
			movement.isAlive = false;
			positionCursorData.isAlive = false;
			fireRocketData.isAlive = false;
			fireShellData.isAlive = false;
			if(smokeDead.isStopped)
			{
				smokeDead.Play ();
			}
			smokeExhaustLeft.startColor = Color.black;
			smokeExhaustRight.startColor = Color.black;
		}
		else
		{
			movement.isAlive = true;
			positionCursorData.isAlive = true;
			fireRocketData.isAlive = true;
			fireShellData.isAlive = true;
			smokeExhaustLeft.startColor = Color.gray;
			smokeExhaustRight.startColor = Color.gray;
			if(smokeDead.isPlaying)
			{
				smokeDead.Stop();
			}
		}
		positionCursorData.IsJammed = turretData.isJammed;
		movement.Detracked = tracksLeftData.Detracked || tracksRightData.Detracked;
		//TODO --Weight if fired
	}
	
	private void PlayerUpdate()
	{
		GUIPlayer playerGUI = gameObject.GetComponent("GUIPlayer") as GUIPlayer;
		
		playerGUI.Health = tankData.HealthAmount;
		playerGUI.RocketAmmo = hatchControlData.RocketAmmo;
		playerGUI.ShellAmmo = gunData.ShellAmmo;
	}
	
	private void BotUpdate()
	{
		//TODO ??
	}
	
	private double GetAmmoWeight()
	{
		return gunData.ShellAmmo * gunData.ShellWeight() + hatchControlData.RocketAmmo * hatchControlData.RocketWeight();
	}
	
	private bool GetPlayerControlled()
	{
		return controlledBy.ToString() == "Player";
	}
	
	private void CheckController()
	{
		if(GetPlayerControlled())
		{
			PlayerControlled();
		}
		else
		{
			BotControlled();
		}
	}
	
	public void ResetAmmo()
	{
		gunData.ShellAmmo = 0;
		hatchControlData.RocketAmmo = 0;
		PresetWeight();
		
		GetAmmo();
	}
	
	private void GetAmmo()
	{
		FillAmmo(rocketCount);
		
		if(!tankData.OverWeight)
		{
			FillAcessWeightWithShells(GetAcessWeight());
		}
	}
	
	private void FillAcessWeightWithShells(double acessWeight)
	{
		int shellCount=(int)( acessWeight / gunData.ShellWeight());
		gunData.ShellAmmo += shellCount;
		tankData.Weight += shellCount * gunData.ShellWeight();
	}
	
	private void FillAmmo(int rockets)
	{
		if(rockets * hatchControlData.RocketWeight() + tankData.Weight > tankData.MaxWeight)
		{
			rockets += (int)((tankData.MaxWeight - 
				(rockets * hatchControlData.RocketWeight() + tankData.Weight)) /
				hatchControlData.RocketWeight() - 1);
		}
		gunData.ShellAmmo = (int)(((tankData.MaxWeight - tankData.Weight) -
			(rockets * hatchControlData.RocketWeight())) /
			gunData.ShellWeight());
		tankData.Weight += (gunData.ShellAmmo * gunData.ShellWeight());
		hatchControlData.RocketAmmo = rockets;
		tankData.Weight += hatchControlData.RocketAmmo * hatchControlData.RocketWeight();
	}
	
	private double GetAcessWeight()
	{
		return tankData.MaxWeight - tankData.Weight;
	}
	
	private void TankDepression(string tankName)
	{
		if(tankName == "Panther")
		{
			positionCursorData.gunShieldMaxDown = 14;
			positionCursorData.gunShieldMaxUp = 3;
		}
		else if (tankName == "KV1")
		{
			positionCursorData.gunShieldMaxDown = 2;
			positionCursorData.gunShieldMaxUp = 25;
		}
		else if(tankName == "KingTiger")
		{
			Debug.Log("King Tiger NOT IMPLEMENTED YET");
		}
		else if(tankName == "IS3")
		{
			Debug.Log("IS 3 NOT IMPLEMENTED YET");
		}
		else if(tankName == "T44")
		{
			Debug.Log("T 44 NOT IMPLEMENTED YET");
		}
		else if (tankName == "Custom")
		{
			Debug.Log("CUSTOM NOT IMPLEMENTED YET");
		}
		else
		{
			Debug.Log("TANK PRESET NOT FOUND");
		}
	}
	
	private void PlayerControlled()
	{
		gameObject.tag = "Player";
		
		//Player Modules
		if(!gameObject.GetComponent("GUIPlayer"))
		{
			playerGUI = gameObject.AddComponent("GUIPlayer") as GUIPlayer;
		}
		if(!gameObject.GetComponent("GUITexture"))
		{
			playerGuiTexture = gameObject.AddComponent("GUITexture") as GUITexture;
		}
		
		fireRocketData = hatchControl.AddComponent("PlayerLaunchRocket") as PlayerLaunchRocket;  
		fireShellData = gun.AddComponent("PlayerLaunchShell") as PlayerLaunchShell;
		movement = gameObject.AddComponent("PlayerMovement") as PlayerMovement;
		positionCursorData = gunShieldControl.AddComponent("FollowCursor") as FollowCursor;
		
		AudioListener playerAudioListener = gameObject.AddComponent("AudioListener") as AudioListener;
	}
	
	private void PlayerMechanics()
	{
		playerGUI = gameObject.GetComponent("GUIPlayer") as GUIPlayer;
		playerGUI.Health = tankData.HealthAmount;
		playerGUI.MaxHealth = HealthSave;
		playerGUI.HealthBars = GameObject.Find(tankPreset.ToString() + "HealthBarsFull").
			GetComponent<GUIPlayer>().HealthBars;
		playerGUI.ShellAmmo = gunData.ShellAmmo;
		playerGUI.RocketAmmo = hatchControlData.RocketAmmo;
		
		playerGuiTexture.texture = new Texture2D(140, 60);
	}
	
	private void BotControlled()
	{
		if(gameObject.GetComponent("GUIPlayer"))
		{
			Destroy(gameObject.GetComponent("GUIPlayer"));
		}
		
		fireRocketData = hatchControl.AddComponent("LaunchRocket") as LaunchRocket;  
		fireShellData = gun.AddComponent("LaunchShell") as LaunchShell;
		movement = gameObject.AddComponent("SetPosition") as SetPosition;
		positionCursorData = gunShieldControl.AddComponent("SetCursor") as SetCursor;
		lazerDetectorData = GameObject.Find(user + "Lazer").AddComponent("LazerDetector") as LazerDetector;
		
		gameObject.tag = "Bot";
	}
	
	private void SetAmmoData()
	{
		fireShellData.Shell = shell.rigidbody;
		fireShellData.SpawnPoint = GameObject.Find (user+"ShellSpawnPoint");
		fireShellData.isAlive = true;
		fireShellData.smoke = gunSmoke;
		
		hatchControlData = hatchControl.AddComponent("DataHatchControl") as DataHatchControl;
		hatchControlData.Power = 30;
		hatchControlData.RocketCaliber = rocketCaliber;
		
		shellData = shell.AddComponent("DataShells") as DataShells;
		shellData.CurrentShell = shellCaliber;
		shellData.Reload();
		
		rocketData = rocket.AddComponent("DataRockets") as DataRockets;
		rocketData.CurrentRocket = hatchControlData.RocketCaliber;
		rocketData.Reload();
	}
	
	private void SetCommonData()
	{
		hatchControlData.ReloadTime = tankData.ReloadTimeRocket;
		
		positionCursorData.angleRotationSpeed = gunData.RotationSpeed;
		positionCursorData.turret = turretControl;
		
		TankDepression(tankPreset.ToString());
		
		positionCursorData.inverted = false;
		positionCursorData.isAlive = true;
		
		movement.maxSpeed = tracksLeftData.MaxSpeed + tracksRightData.MaxSpeed;
		movement.gravity = gameGravity;
		movement.rotationSpeed = tracksLeftData.TraverseSpeed + tracksRightData.TraverseSpeed;
		movement.horsePower = tankData.horsePower;
		movement.trackWheelsLeft = trackWheelsLeft;
		movement.trackWheelsRight = trackWheelsRight;
		movement.trackLeft = trackLeft;
		movement.trackRight = trackRight;
		movement.isAlive = true;
		movement.smokeExhaustLeft = smokeExhaustLeft;
		movement.smokeExhaustRight = smokeExhaustRight;
		movement.dirtForwardLeft = dirtForwardLeft;
		movement.dirtForwardRight = dirtForwardRight;
		movement.dirtBackwardLeft = dirtBackwardLeft;
		movement.dirtBackwardRight = dirtBackwardRight;
		//.weight is calculated later

		shellBehaviourData = shell.AddComponent("ShellBehaviour") as ShellBehaviour;
		shellBehaviourData.currentShell = shellData;
		
		rocketBehaviourData = rocket.AddComponent("RocketBehaviour") as RocketBehaviour;
		rocketBehaviourData.currentRocket = rocketData;
		                                           
		fireRocketData.Rocket = rocket.rigidbody;
		fireRocketData.SpawnPointRocket = GameObject.Find(user+"RocketSpawnPoint");
		fireRocketData.isAlive = true;
		
		GetAmmo();
		
		movement.weight = tankData.Weight;
		if(shellCaliber == DataShells.ShellSize.mm40 || shellCaliber == DataShells.ShellSize.mm50)
		{
			gunFire = GameObject.Find(tankPreset.ToString()+"ShellSmallSound").
				GetComponent("AudioSource") as AudioSource;
		}
		else
		{
			gunFire = GameObject.Find(tankPreset.ToString()+"ShellSound").
				GetComponent("AudioSource") as AudioSource;
		}
		rocketFire = GameObject.Find(tankPreset.ToString()+"RocketSound").
			GetComponent("AudioSource") as AudioSource;
		engine = GameObject.Find(tankPreset.ToString()+"ChassisSound").
			GetComponent("AudioSource") as AudioSource;
		engineMoving = GameObject.Find(tankPreset.ToString()+"ChassisMovingSound").
			GetComponent("AudioSource") as AudioSource;
		
		
		gameObject.audio.clip = engineMoving.clip;
		gameObject.audio.volume = 0.3f;
		gameObject.audio.loop = true;
		gameObject.audio.Stop();
		
		gun.audio.clip = gunFire.clip;
		gun.audio.volume = 0.4f;
		
		hatchControl.audio.clip = rocketFire.clip;
		hatchControl.audio.volume = 0.3f;
		
		chassis.audio.clip = engine.clip;
		chassis.audio.volume = 0.3f;
		chassis.audio.loop = true;
		chassis.audio.Play();
	}
	
	private DataTank EditTankData(DataTank tankData, int armorSide, int armorFront,
		int armorBack, double reloadTimeRocket, float healthAmount, float horsePower)
	{
		tankData.ArmorSide = armorSide;
		tankData.ArmorFront = armorFront;
		tankData.ArmorBack = armorBack;
		tankData.ReloadTimeRocket = reloadTimeRocket;
		tankData.HealthAmount = healthAmount;
		HealthSave = healthAmount;
		tankData.horsePower = horsePower;
		tankData.OverWeight = false;
		return tankData;
	}
	
	private DataTracks EditTracksData(DataTracks tracks,
		float health, int repairTime, int traverseSpeed,
		double weight, double weightCapacity, int maxSpeed)
	{
		tracks.Detracked = false;
		tracks.Health = health;
		tracks.RepairTime = repairTime;
		tracks.Weight = weight;
		tracks.WeightCapacity = weightCapacity;
		tracks.MaxSpeed = maxSpeed;
		tracks.TraverseSpeed = traverseSpeed;
		return tracks;
	}
	
	private DataTurret EditTurretData(DataTurret turret, int weight, float health, int repairTime,
		int traverseSpeed, int armorSide, int ArmorFront, int armorBack)
	{
		turret.isJammed = false;
		turret.Health = health;
		turret.RepairTime = repairTime;
		turret.Weight = weight;
		turret.TraverseSpeed = traverseSpeed;
		turret.ArmorBack = armorBack;
		turret.ArmorFront = ArmorFront;
		turret.ArmorSide = armorSide;
		return turret;
	}
	
	private DataGun EditGunData(DataGun gun, int rotationSpeed, int traverse,
		int depression, double reloadTime, int power, DataShells.ShellSize shellCaliber)
	{
		gun.RotationSpeed = rotationSpeed;
		gun.AngleTraverse = traverse;
		gun.AngleDepression = depression;
		gun.ReloadTime = reloadTime;
		gun.Power = power;
		gun.ShellCaliber = shellCaliber;
		return gun;
	}
	
	private void PantherPreset()
	{
		//ShootMechanism, Movement
		CheckController();
		
		SetAmmoData();
		//Chassis;
		tankData = EditTankData(tankData, 100, 150, 60, rocketData.ReloadTime, 800, 400);
		
		//Tracks
		tracksLeftData = EditTracksData(tracksLeftData, 50, 10 , 15, 200, 10800, 15);
		// ++ //
		tracksRightData = EditTracksData(tracksRightData, 50, 10 , 15, 200, 10800, 15);
		
		//Turret
		turretData = EditTurretData(turretData, 50, 300, 15, 30, 60, 80, 40);
		
		//GunShield
		gunData = EditGunData(gunData, 50, 10, 10, shellData.ReloadTime, 150, shellData.CurrentShell);
		gunData.Reload();
		
		tankData.MaxWeight = tracksLeftData.WeightCapacity + tracksRightData.WeightCapacity;
		
		//Chassis
		PresetWeight();
		
		SetCommonData();
		
		if(user == "Player")
		{
			PlayerMechanics();
		}
	}
	
	private void KV1Preset()
	{
		//ShootMechanism, Movement
		CheckController();
		
		SetAmmoData();
		//Chassis;
		tankData = EditTankData(tankData, 150, 120, 80, rocketData.ReloadTime, 1600, 200);
		
		//Tracks
		tracksLeftData = EditTracksData(tracksLeftData, 50, 10 , 15, 200, 13000, 10);
		// ++ //
		tracksRightData = EditTracksData(tracksRightData, 50, 10 , 15, 200, 13000, 10);
		
		//Turret
		turretData = EditTurretData(turretData, 50, 400, 20, 20, 80, 80, 80);
		
		//GunShield
		gunData = EditGunData(gunData, 50, 10, 10, shellData.ReloadTime, 150, shellCaliber);
		gunData.Reload();
		
		tankData.MaxWeight = tracksLeftData.WeightCapacity + tracksRightData.WeightCapacity;
		
		//Chassis
		PresetWeight();
				
		SetCommonData();
		
		if(user == "Player")
		{
			PlayerMechanics();
		}
	}
	
	private void KingTigerPreset()
	{
		Debug.Log("King Tiger NOT IMPLEMENTED YET");
	}
	private void IS3Preset()
	{
		Debug.Log("IS 3 NOT IMPLEMENTED YET");
	}
	
	private void T44Preset()
	{
		Debug.Log("T 44 NOT IMPLEMENTED YET");
	}
	private void CustomPreset()
	{
		Debug.Log("CUSTOM NOT IMPLEMENTED YET");
		//GUI Prompt Please set the data ; CHECK FOR VALID DATA 
	}
	private void TankNotFound()
	{
		Debug.Log("TANK NOT FOUND AND NOT IMPLEMENTED YET");
		// Back to Screen
	}
	
	private void PresetWeight()
	{
		int presetWeight = 0;
		string tankName = tankPreset.ToString();
		
		if(tankName == "Panther")
		{
			presetWeight = 20000;
		}
		else if (tankName == "KV1")
		{
			presetWeight = 24000;
		}
		else if(tankName == "KingTiger")
		{
			Debug.Log("King Tiger NOT IMPLEMENTED YET");
		}
		else if(tankName == "IS3")
		{
			Debug.Log("IS 3 NOT IMPLEMENTED YET");
		}
		else if(tankName == "T44")
		{
			Debug.Log("T 44 NOT IMPLEMENTED YET");
		}
		else if (tankName == "Custom")
		{
			Debug.Log("CUSTOM NOT IMPLEMENTED YET");
		}
		else
		{
			Debug.Log("TANK PRESET NOT FOUND");
		}
		
		tankData.Weight = 
			gunData.Weight +
				turretData.Weight +
				tracksLeftData.Weight +
				tracksRightData.Weight +
				presetWeight
				;
	}
	
}
