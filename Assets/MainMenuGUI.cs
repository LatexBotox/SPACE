using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
    public GUISkin skin;
	
	void Start () {
        
	}

    void OnGUI()
    {
        GUI.skin = skin;
        if(GUI.Button(new Rect(Camera.main.pixelWidth/2-150,Camera.main.pixelHeight/2-100,300,50),"New Game",skin.FindStyle("TextButton"))) {
            ShipBuilder.instance.NewGame();
            Application.LoadLevel("tutorial");
            
        }

        if (GUI.Button(new Rect(Camera.main.pixelWidth / 2 - 150, Camera.main.pixelHeight / 2-40, 300, 50), "Load Game", skin.FindStyle("TextButton")))
        {
            ShipBuilder.instance.Load(0);
            Application.LoadLevel("mapgentest");
        }

        if (GUI.Button(new Rect(Camera.main.pixelWidth / 2 - 150, Camera.main.pixelHeight / 2 + 20, 300, 50), "Options", skin.FindStyle("TextButton")))
        {

        }

        if (GUI.Button(new Rect(Camera.main.pixelWidth / 2 - 150, Camera.main.pixelHeight / 2 + 80, 300, 50), "Exit", skin.FindStyle("TextButton")))
        {
            Application.Quit();
        }
    }
}
