using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour 
{
	public GUIStyle pauseBackgroundColor;
	
	private bool paused = false;
	private List<TankCollection> tankCollections = new List<TankCollection>();
	
	private int startX = Screen.width / 4;
	private int startY = Screen.height / 4;
	
	private Texture cursorImage;
	private GameObject playerCursor;
	
 	private void Start()
	{
		Screen.showCursor = false;
		playerCursor = GameObject.Find(gameObject.transform.root.name + "Cursor");
		cursorImage = GameObject.Find ("CursorDesign").GetComponent<GUITexture>().texture;
		foreach (GameObject item in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(item.GetComponent("TankCollection"))
			{
				tankCollections.Add(item.GetComponent("TankCollection") as TankCollection);
			}
		}
	}
    private void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
     		paused = togglePause();
			PauseTanks();
		}
    }
 
    private void OnGUI()
    {
		if(paused)
		{
			GUI.Box(new Rect(startX , startY, 400, 300), "", pauseBackgroundColor);
	   		GUI.Label(new Rect(startX + 50, startY, 100, 100),
				"Game is paused!", GUIAssist.BiggerText(40));
	        if(GenerateButton("RESUME GAME", 40) || Input.GetKeyDown(KeyCode.Escape) )
			{
	       		paused = togglePause();
			}
			if(GenerateButton("MAIN MENU", 80))
			{
				Application.LoadLevel(0);
			}
			if(GenerateButton("RESTART", 140))
			{
				GameObject.Find("GameInformation").GetComponent<GameInformation>().forseRestart = true;
			}
			if(GenerateButton("QUIT", 180))
			{
				Application.Quit();
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
	        	paused = togglePause();
			}
			PauseTanks();
	    }	
	
		Vector3 mousePosition = Input.mousePosition;
		Rect setMousePosition = new Rect(mousePosition.x, Screen.height - mousePosition.y, 25, 25);
		GUI.Label(setMousePosition, cursorImage);
    }
	
 	private bool GenerateButton(string content, int nextButtonDistance)
	{
		return GUI.Button(new Rect(startX + 100, startY + nextButtonDistance + 40, 200, 30),
				content);
	}
	
    private bool togglePause()
   	{
       if(Time.timeScale == 0f)
       {
       		Time.timeScale = 1f;
			return (false);
       }
       else
       {
        	Time.timeScale = 0f;
       		return(true);    
       }
    }
	
	private void PauseTanks()
	{
		GameObject.FindGameObjectWithTag("Player").audio.Stop();
		GameObject.Find("TheCameraControl").GetComponent<MouseLook>().enabled = !paused;
		foreach (TankCollection tankStatus in tankCollections) 
		{
			tankStatus.paused = paused;
		}
	}
}
