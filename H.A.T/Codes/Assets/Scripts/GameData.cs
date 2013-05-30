using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{
	public Texture2D[] animFrames = new Texture2D[241];	
	
	private ComboBox playerShellSizes = new ComboBox();
	private ComboBox playerRocketSizes = new ComboBox();
	private ComboBox botShellSizes = new ComboBox();
	private ComboBox botRocketSizes = new ComboBox();
	private ComboBox difficulties = new ComboBox();
	
	public DataRockets.RocketSize botRocketCaliber;
	public DataRockets.RocketSize playerRocketCaliber;
	public DataShells.ShellSize botShellCaliber;
	public DataShells.ShellSize playerShellCaliber;
	public BotDifficulty.Difficulty gameDifficulty;
	public int botRocketCount;
	public int playerRocketCount;
	public int scoreMax;
	
	public bool ShowBotChassisProps = true;
	public bool ShowBotMinigun = true;
	public bool ShowBotTurretProps = true ;
	public bool ShowBotSideskirts = true;
	
	public bool ShowPlayerChassisProps = true;
	public bool ShowPlayerMinigun = true;
	public bool ShowPlayerTurretProps = true ;
	public bool ShowPlayerSideskirts = true;
	
	private string playerRockets = "";
	private string botRockets = "";
	private string scoreLimit = "";
	
	private GUIContent[] Sizes;
	private GUIContent[] Difficulties;
	
	private int startX;
	private int startY;
	
	private float timer;
	private int currentFrame;
	
	private bool Properties = false;
	
	private GUIStyle listStyle = new GUIStyle();
	private Texture cursorImage;
	private GameObject playerCursor;
	
	private void Start()
	{
		Screen.showCursor = false;
		playerCursor = GameObject.Find(gameObject.transform.root.name + "Cursor");
		cursorImage = GameObject.Find ("CursorDesign").GetComponent<GUITexture>().texture;
		
		DontDestroyOnLoad(gameObject);
	    Sizes = new GUIContent[4];
	    Sizes[0] = new GUIContent("mm40");
	    Sizes[1] = new GUIContent("mm50");
	    Sizes[2] = new GUIContent("mm80");
	    Sizes[3] = new GUIContent("mm100");
		
		Difficulties = new GUIContent[3];
		Difficulties[0] = new GUIContent("Easy");
		Difficulties[1] = new GUIContent("Medium");
		Difficulties[2] = new GUIContent("Hard");

	    listStyle.normal.textColor = Color.white; 
	    listStyle.onHover.background =
	    listStyle.hover.background = new Texture2D(2, 2);
	    listStyle.padding.left =
	    listStyle.padding.right =
	    listStyle.padding.top =
	    listStyle.padding.bottom = 4;
	}
	
	private void OnGUI()
	{
		timer += Time.deltaTime;
		
		if(timer > 0.1f)
		{
			timer = 0;
			currentFrame++;
			if(currentFrame > 299)
				currentFrame = 0;
		}
		
		if(Screen.width > 600)
		{
			startX = Screen.width/2 - 300;
		}
		else
		{
			startX = 0;
		}
		if(Screen.height > 500)
		{
			startY = Screen.height/2 - 300;
		}
		else
		{
			startY = 0;
		}
		
		if(Properties)
		{
			PropertiesMenu();
		}
		else
		{
			MainMenu();
		}
		Vector3 mousePosition = Input.mousePosition;
		Rect setMousePosition = new Rect(mousePosition.x, Screen.height - mousePosition.y, 25, 25);
		GUI.Label(setMousePosition, cursorImage);
	}
	
	public void MainMenu()
	{
		if(GUI.Button(new Rect(0, Screen.height - 100 ,100,100), "Quit"))
		{
			Application.Quit();
		}
		if(GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), "Play"))
		{
			Properties = true;
		}
		TankAnimation();
		
		GUI.Label(new Rect(startX + 250 , startY + 280 , 100, 100), "H.A.T.",
			GUIAssist.BiggerText(40));
		GUI.Label(new Rect(startX , startY + 330, 100, 100), "HEATED AFFRAYS WITH TANKS",
			GUIAssist.BiggerText(40));
	}
	
	private void TankAnimation()
	{
		if(currentFrame < 240)
		{
			GUI.Box(new Rect(startX + 50, startY + 25, 500, 250), animFrames[currentFrame]); // Images
		}
		else
		{
			GUI.Box(new Rect(startX + 50, startY + 25, 500, 250), ""); // Background
		}
	}
	
	private void PropertiesMenu()
	{
		Ammo ();
		Details();
		
		if(GUI.Button(new Rect(Screen.width-100, Screen.height - 100,100,100), "ROLL OUT"))
		{
			if(botRocketCount > -1 && playerRocketCount >-1 &&
				int.TryParse(playerRockets, out playerRocketCount)&&
				int.TryParse(botRockets, out botRocketCount)&&
				int.TryParse(scoreLimit, out scoreMax)&&
				scoreMax < 1000 && scoreMax > 0)
			{
				if(playerRocketCount < 0)
				{
					playerRocketCount = 0;
				}
				
				if(botRocketCount < 0)
				{
					botRocketCount = 0;
				}
				
				Application.LoadLevel(1);
			}
		}
		
		if(GUI.Button(new Rect(Screen.width - 100, 0, 100, 100), "Back"))
		{
			timer = 0;
			currentFrame = 0;
			Properties = false;
		}
	}
	
	private void Ammo()
	{
		int selectedPlayerShell = playerShellSizes.GetSelectedItemIndex();
		
		GUI.Label( new Rect(startX, startY + 30, 180, 21), "Player Shell Size");
	    selectedPlayerShell = playerShellSizes.List( new Rect(startX + 10, startY + 60, 60, 20), Sizes[selectedPlayerShell].text, Sizes, listStyle );
		playerShellCaliber = (DataShells.ShellSize)System.Enum.Parse(typeof(DataShells.ShellSize), Sizes[selectedPlayerShell].text);
		
		int selectedPlayerRocket = playerRocketSizes.GetSelectedItemIndex();
		
		GUI.Label( new Rect(startX + 120, startY + 30, 180, 21), "Player Rocket Size");
	    selectedPlayerRocket = playerRocketSizes.List( new Rect(startX + 130, startY + 60, 60, 20), Sizes[selectedPlayerRocket].text, Sizes, listStyle );
		playerRocketCaliber = (DataRockets.RocketSize)System.Enum.Parse(typeof(DataRockets.RocketSize), Sizes[selectedPlayerRocket].text);
		
		int selectedBotShell = botShellSizes.GetSelectedItemIndex();
		
		GUI.Label( new Rect(startX + 300, startY + 30, 180, 21), "Bot Shell Size");
	    selectedBotShell = botShellSizes.List( new Rect(startX + 310, startY + 60, 60, 20), Sizes[selectedBotShell].text, Sizes, listStyle );
		botShellCaliber = (DataShells.ShellSize)System.Enum.Parse(typeof(DataShells.ShellSize), Sizes[selectedBotShell].text);
		
		int selectedBotRocket = botRocketSizes.GetSelectedItemIndex();
		
		GUI.Label( new Rect(startX + 400, startY + 30, 180, 21), "Bot Rocket Size");
	    selectedBotRocket = botRocketSizes.List( new Rect(startX + 410, startY + 60, 60, 20), Sizes[selectedBotRocket].text, Sizes, listStyle );
		botRocketCaliber = (DataRockets.RocketSize)System.Enum.Parse(typeof(DataRockets.RocketSize), Sizes[selectedBotRocket].text);
		
		GUI.Label( new Rect(startX+ 60, startY + 170, 180, 21), "Player Rocket Count");
		playerRockets = GUI.TextField(new Rect(startX + 60, startY + 190, 50, 21), playerRockets, 25);
		
		GUI.Label( new Rect(startX + 350, startY + 170, 180, 21), "Bot Rocket Count");
		botRockets = GUI.TextField(new Rect(startX + 350, startY +   190, 50, 21), botRockets, 25);
	}
	
	private void Details()
	{
		PlayerDetails();
		BotDetails();
		GameDetails();
	}
	
	private void PlayerDetails()
	{
		GUI.Label( new Rect(startX + 30, startY + 240, 180, 21), "Display Player Details");
		if(GUI.Button( new Rect(startX + 30, startY +   260, 120, 21), "Chassis Props"))
		{
			ShowPlayerChassisProps = !ShowPlayerChassisProps;
		}
		
		if(ShowPlayerChassisProps)
		{
			GUI.Label( new Rect(startX + 160, startY +  260, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 160, startY + 260, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 30, startY +   290, 120, 21), "Turret Props"))
		{
			ShowPlayerTurretProps = !ShowPlayerTurretProps;
		}
		
		if(ShowPlayerTurretProps)
		{
			GUI.Label( new Rect(startX + 160, startY +   290, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 160, startY +   290, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 30, startY +   320, 120, 21), "Mini Gun"))
		{
			ShowPlayerMinigun = !ShowPlayerMinigun;
		}
		
		if(ShowPlayerMinigun)
		{
			GUI.Label( new Rect(startX + 160, startY +   320, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 160, startY +   320, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 30, startY +   350, 120, 21), "Side Skirts"))
		{
			ShowPlayerSideskirts = !ShowPlayerSideskirts;
		}
		
		if(ShowPlayerSideskirts)
		{
			GUI.Label( new Rect(startX + 160, startY +   350, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 160, startY +   350, 120, 21), "X");
		}
	}
	
	private void BotDetails()
	{
		GUI.Label( new Rect(startX + 310, startY +   240, 180, 21), "Display Bot Details");
		if(GUI.Button( new Rect(startX + 310, startY +   260, 120, 21), "Chassis Props"))
		{
			ShowBotChassisProps = !ShowBotChassisProps;
		}
		
		if(ShowBotChassisProps)
		{
			GUI.Label( new Rect(startX + 450, startY +  260, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 450, startY +  260, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 310, startY +  290, 120, 21), "Turret Props"))
		{
			ShowBotTurretProps = !ShowBotTurretProps;
		}
		
		if(ShowBotTurretProps)
		{
			GUI.Label( new Rect(startX + 450, startY +  290, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 450, startY +  290, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 310, startY +  320, 120, 21), "Mini Gun"))
		{
			ShowBotMinigun = !ShowBotMinigun;
		}
		
		if(ShowBotMinigun)
		{
			GUI.Label( new Rect(startX + 450, startY +  320, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 450, startY +  320, 120, 21), "X");
		}
		
		if(GUI.Button( new Rect(startX + 310, startY +  350, 120, 21), "Side Skirts"))
		{
			ShowBotSideskirts = !ShowBotSideskirts;
		}
		
		if(ShowBotSideskirts)
		{
			GUI.Label( new Rect(startX + 450, startY +  350, 120, 21), "V");
		}
		else
		{
			GUI.Label( new Rect(startX + 450, startY +  350, 120, 21), "X");
		}
	}
	
	private void GameDetails()
	{
		GUI.Label( new Rect(startX + 205, startY + 170, 180, 21), "Score Limit");
		scoreLimit = GUI.TextField(new Rect(startX + 205, startY + 190, 50, 21), scoreLimit, 25);
		if(int.TryParse(scoreLimit, out scoreMax))
		{
			if (!(scoreMax < 1000))
			{
				GUI.Label( new Rect(startX + 255, startY + 190, 180, 21), "<1000");
			}
			if(!(scoreMax > 0))
			{
				GUI.Label( new Rect(startX + 185, startY + 190, 180, 21), "0 < ");
			}
		}
		else
		{
			GUI.Label( new Rect(startX + 185, startY + 210, 180, 21), "Enter a number");
		}
		
		int selectedDifficulty = difficulties.GetSelectedItemIndex();
		
		GUI.Label( new Rect(startX + 205, startY + 300, 180, 21), "Difficulty");
	    selectedDifficulty = difficulties.List( new Rect(startX + 205, startY + 320, 60, 20), Difficulties[selectedDifficulty].text, Difficulties, listStyle );
		gameDifficulty = (BotDifficulty.Difficulty)System.Enum.Parse(typeof(BotDifficulty.Difficulty), Difficulties[selectedDifficulty].text);
	}
}
