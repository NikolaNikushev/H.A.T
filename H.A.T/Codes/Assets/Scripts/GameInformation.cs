using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInformation : MonoBehaviour 
{
	public GameObject player;
	public GameObject bot;
	
	public BotDifficulty.Difficulty difficultyLevel;
	public int scoreLimit = 0;
	
	public DataShells.ShellSize playerShellCaliber;
	public DataRockets.RocketSize playerRocketCaliber;
	public SetPosition resetWaypoint;
	public int playerRocketCount;
	
	public DataShells.ShellSize botShellCaliber;
	public DataRockets.RocketSize botRocketCaliber;
	public int botRocketCount;
	
	public GameObject botSpawnPoint;
	public GameObject playerSpawnPoint;
	
	public bool ShowBotChassisProps = true;
	public bool ShowBotMinigun = true;
	public bool ShowBotTurretProps = true ;
	public bool ShowBotSideskirts = true;
	
	public bool ShowPlayerChassisProps = true;
	public bool ShowPlayerMinigun = true;
	public bool ShowPlayerTurretProps = true ;
	public bool ShowPlayerSideskirts = true;
	
	private bool playerStatus;
	private bool botStatus;
	
	private int playerScore;
	private int botScore;
	
	private bool firstTime = true;
	private float timer;
	
	private bool playerWin = false;
	
	private List<GameObject> presets = new List<GameObject>();
	
	public bool forseRestart = false;
	private bool restart = false;
	private bool endGame = false;
	
	private int  scoreLimitZeros;

	private void Start () 
	{
		GameData info = GameObject.Find("GameData").GetComponent<GameData>();
		botRocketCaliber = info.botRocketCaliber;
		playerRocketCaliber = info.playerRocketCaliber;
		playerShellCaliber = info.playerShellCaliber;
		botShellCaliber = info.botShellCaliber;
		playerRocketCount = info.playerRocketCount;
		botRocketCount = info.botRocketCount;
		ShowBotChassisProps = info.ShowBotChassisProps;
		ShowBotTurretProps = info.ShowBotTurretProps;
		ShowBotMinigun = info.ShowBotMinigun;
		ShowBotSideskirts = info.ShowBotSideskirts;
		ShowPlayerChassisProps = info.ShowPlayerChassisProps;
		ShowPlayerTurretProps = info.ShowPlayerTurretProps;
		ShowPlayerMinigun = info.ShowPlayerMinigun;
		ShowPlayerSideskirts = info.ShowPlayerSideskirts;
		scoreLimit = info.scoreMax;
		difficultyLevel = info.gameDifficulty;
		
		Destroy(GameObject.Find("GameData"));
			
		CreateNew();
		for (int i = 1; i <= scoreLimit; i *= 10) 
		{
			scoreLimitZeros++;
		}
		//Replaced at scene --> GameInformation
		//PauseMenu pauseMenu = gameObject.AddComponent("PauseMenu") as PauseMenu;
	}
	
	private void Update()
	{
		StatusOfGame();
		if(restart)
		{
			timer += Time.deltaTime;
			if(timer >= 5)
			{
				Restart();
				restart = false;
			}
		}
		else
		{
			timer = 0;	
		}
		if(forseRestart)
		{
			Restart();
			restart = false;
			forseRestart = false;
		}
	}
	
	private void CreateNew()
	{
		if(botSpawnPoint == null)
		{
			botSpawnPoint = GameObject.Find("BotSpawnPoint");
		}
		if(playerSpawnPoint == null)
		{
			playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
		}
		
		player.GetComponent<TankCollection>().shellCaliber = playerShellCaliber;
		player.GetComponent<TankCollection>().rocketCaliber = playerRocketCaliber;
		player.GetComponent<TankCollection>().rocketCount = playerRocketCount;
		player.GetComponent<TankCollection>().showChassisProps = ShowPlayerChassisProps;
		player.GetComponent<TankCollection>().showTurretProps = ShowBotTurretProps;
		player.GetComponent<TankCollection>().minigunActive = ShowPlayerMinigun;
		player.GetComponent<TankCollection>().sideSkirtsActive = ShowPlayerSideskirts;
		
		bot.GetComponent<TankCollection>().shellCaliber = botShellCaliber;
		bot.GetComponent<TankCollection>().rocketCaliber = botRocketCaliber;
		bot.GetComponent<TankCollection>().rocketCount = botRocketCount;
		bot.GetComponent<TankCollection>().showChassisProps = ShowBotChassisProps;
		bot.GetComponent<TankCollection>().showTurretProps = ShowBotTurretProps;
		bot.GetComponent<TankCollection>().minigunActive = ShowBotMinigun;
		bot.GetComponent<TankCollection>().sideSkirtsActive = ShowBotSideskirts;
		
		Instantiate(player, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
		
		Instantiate(bot, botSpawnPoint.transform.position, botSpawnPoint.transform.rotation);
		
		RemoveClone();
		
		if(firstTime)
		{
			foreach (TankCollection.TankPreset preset in System.Enum.GetValues(typeof(TankCollection.TankPreset))) 
			{
				if(GameObject.Find(preset.ToString() + "Data"))
				{
					GameObject tankPreset = GameObject.Find(preset.ToString() + "Data");
					AudioSource[] sources = tankPreset.GetComponentsInChildren<AudioSource>();
					for (int i = 0; i < sources.Length; i++) 
					{
						sources[i].Stop();
					}
				}
			}
			firstTime = false;
		}
		
		// set bot Difficulty of all bots;
		GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");
		
		foreach (GameObject botTank in bots)
		{
			if(bot.GetComponent("BotDifficulty"))
			{
				EditDifficulty(botTank);
			}
			else
			{
				CreateDifficulty(botTank);
			}
		}
	}
	
	private void CreateDifficulty(GameObject bot)
	{
		BotDifficulty tempDifficulty = bot.AddComponent("BotDifficulty") as BotDifficulty;
		tempDifficulty.botDifficulty = difficultyLevel;
	}
	
	private void EditDifficulty(GameObject bot)
	{
		BotDifficulty tempDifficulty = bot.GetComponent("BotDifficulty") as BotDifficulty;
		tempDifficulty.botDifficulty = difficultyLevel;
	}
	
	private void RemoveClone()
	{
		foreach (GameObject item in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(item.name.Contains("(Clone)"))
			{
				item.name = item.name.Replace("(Clone)","");
			}
		}
	}
	
	private void StatusOfGame()
	{
		if (!endGame)
		{
			if(!restart)
			{
				playerStatus = GameObject.FindGameObjectWithTag("Player").
					GetComponent<TankCollection>().IsAlive;
			}
			else
			{
				playerStatus = true;
			}
			if(!playerStatus)
			{
				botScore++;
				playerWin = false;
				if(botScore >= scoreLimit)
				{
					EndGame();
				}
				else
				{
					restart = true;
				}
			}
			if(!restart)
			{
				botStatus = GameObject.FindGameObjectWithTag("Bot").
					GetComponent<TankCollection>().IsAlive;
			}
			else
			{
				botStatus = true;
			}
			if(!botStatus)
			{
				playerScore++;
				playerWin = true;
				if(playerScore >= scoreLimit)
				{
					EndGame();
				}
				else
				{
					restart = true;
				}
			}
		}
	}
	
	private void Restart()
	{
		GameObject botTank = GameObject.FindGameObjectWithTag("Bot");
		
		//Transform botTurret = GameObject.FindGameObjectWithTag("Bot").GetComponent("TurretControl") as Transform;
		GameObject playerTank = GameObject.FindGameObjectWithTag("Player");
		
		//ResetHealth, Armor, Speed
		ResetComponents(botTank);
		ResetComponents(playerTank);
		
		playerTank.GetComponent<GUIPlayer>().currentHealth = 8;
		
		//ResetPosition and Rotation
		botTank.transform.position = botSpawnPoint.transform.position;
		botTank.transform.rotation = botSpawnPoint.transform.rotation;
		//botTurret.transform.rotation = botSpawnPoint.transform.rotation;
		
		playerTank.transform.position = playerSpawnPoint.transform.position;
		playerTank.transform.rotation = playerSpawnPoint.transform.rotation;
		
		//Reset's the bot's waypoint
		botTank.GetComponent<SetPosition>().ResetWaypoint();
		botTank.GetComponent<SetPosition>().ResetFakePoints();
	}
	
	private void ResetComponents(GameObject tank)
	{
		tank.GetComponentInChildren<DataTank>().RestoreHealth();
		tank.GetComponent<TankCollection>().ResetAmmo();
		tank.GetComponent<Movement>().substractSpeed = tank.GetComponent<Movement>().maxSpeed;
		tank.GetComponent<Movement>().backwardSubstractSpeed =
			tank.GetComponent<Movement>().maxSpeedBackwards;
		DataTracks[] tracksData = tank.GetComponentsInChildren<DataTracks>();
		foreach (DataTracks track in tracksData) {
			track.ResetHealth();
		}
		tank.GetComponentInChildren<DataTurret>().RestroreHealth();
		tank.rigidbody.velocity = Vector3.zero;
	}
	
	private void EndGame()
	{
		RemoveOld();
		endGame = true;
		GameObject.Find("Lights").AddComponent("AudioListener");
	}
	
	private void RemoveOld()
	{
		DestroyObject(GameObject.FindGameObjectWithTag("Player"));
		DestroyObject(GameObject.FindGameObjectWithTag("Bot"));
	}
	
	private void OnGUI()
	{
		if(restart)
		{
			DrawRestart(UserBox(playerWin), UserBox(!playerWin));
		}
		
		if(endGame)
		{
			DrawEndGame();
		}
	}
	
	private GUIStyle UserBox(bool winnerIsPlayer)
	{
		GUIStyle scoreBox = new GUIStyle();
		scoreBox.border.Add(new Rect(0, 0, 20, 20));
		scoreBox.fontSize = 30;
		
		if(winnerIsPlayer)
		{
			scoreBox.normal.textColor = Color.green;
		}
		else
		{
			scoreBox.normal.textColor = Color.red;
		}
		return scoreBox;
	}
	
	private void DrawRestart(GUIStyle playerWinGUI, GUIStyle botWinGUI)
	{
		playerScore = playerScore;
		botScore = botScore;
		scoreLimitZeros = scoreLimitZeros;
		playerWin = playerWin;
		
		int startX, startY;
		
		if(Screen.width > 600)
		{
			startX = Screen.width/2 - 300;
		}
		else
		{
			startX = 10;
		}
		if(Screen.height > 500)
		{
			startY = Screen.height/2 - 300;
		}
		else
		{
			startY = 10;
		}
		
		Rect playerRect = new Rect(300 + startX , startY, 200, 200);
		Rect botRect = new Rect(300 + startX + 300, startY, 200, 200);
		Rect time = new Rect (300 + startX, startY + 60 , 200, 200);
		
		DrawUser(playerRect, "Player", playerScore, playerWinGUI);
		DrawUser(botRect, "Bot", botScore, botWinGUI);
		DrawTime(time, (int)timer);
	}

	private void DrawUser(Rect rectriangle, string user, int score, GUIStyle style)
	{
		Rect labelRect = rectriangle;
		labelRect.x /= 2;
		labelRect.x += 30;
		labelRect.y /= 2;
		labelRect.height /= 2;
		labelRect.width /= 2;
		GUI.Label(labelRect, user, style);
		
		Rect scoreRect = labelRect;
		scoreRect.y += 30;
		scoreRect.width /= 2;
		scoreRect.width += scoreLimitZeros * 10;
		scoreRect.height /= 2;
		GUI.Box(scoreRect, "");
		GUI.Label(scoreRect, score.ToString(), GUIAssist.BiggerText(40));
		scoreRect.x -= 40;
		scoreRect.y += 10;
		if(!playerWin && user == "Bot")
		{
			GUI.Label(scoreRect, "+1", UserBox(!playerWin));
		}
		else if(playerWin && user == "Player")
		{
			GUI.Label(scoreRect, "+1", UserBox(playerWin));
		}
	}
	
	private void DrawTime(Rect rectriangle, int time)
	{
		time = 5 - time;
		
		Rect labelRect = rectriangle;
		labelRect.x /= 2;
		labelRect.x -= 10;
		labelRect.x += 140;
		labelRect.y += 50;;
		labelRect.height = 80;
		labelRect.width = 80;
		
		Rect boxRect = labelRect;
		boxRect.x -= 8;
		boxRect.y -= 8;
		if(time != 10)
			labelRect.x += 15;	
		GUI.Box (boxRect, "");
		GUI.Label(labelRect, time.ToString(), GUIAssist.BiggerText(60));
		boxRect.x -= 190;
		boxRect.y += 3;
		boxRect.width += 100;
		boxRect.height -= 40;
		GUI.Box(boxRect, "");
		labelRect.x -= 200;
		GUI.Label(labelRect, "Playing to " + scoreLimit, GUIAssist.BiggerText(30));
	}
	
	private void DrawEndGame()
	{
		GUI.Box(new Rect(Screen.width / 2 - 230, 100, 100, 200),"END OF GAME SESSION",
			GUIAssist.BiggerText(40));
		
		string winner = "";
		if(playerScore >= scoreLimit)
			winner = "A human ";
		else
			winner = "A bot has ";
		
		GUI.Box(new Rect(Screen.width/2 - 155, 150, 270, 50),"");
		GUI.Label(new Rect(Screen.width/2 - 150, 150, 100, 50), winner + "won!",
			GUIAssist.BiggerText(40));
		
		if(GUI.Button(new Rect(Screen.width / 2 - 210, 200, 100, 50), ""))
		{
			Application.Quit();
		}
		GUI.Label(new Rect(Screen.width / 2 - 210, 200, 100, 50), "QUIT",
			GUIAssist.BiggerText(40));
		
		if(GUI.Button(new Rect(Screen.width / 2, 200, 250, 50), ""))
		{
			Application.LoadLevel(0);
		}
		GUI.Label(new Rect(Screen.width / 2 + 5, 200, 200, 50), "PLAY AGAIN",
			GUIAssist.BiggerText(40));
	}
}