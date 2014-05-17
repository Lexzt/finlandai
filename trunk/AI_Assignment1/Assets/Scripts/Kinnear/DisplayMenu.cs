using UnityEngine;
using System.Collections;

public class DisplayMenu : MonoBehaviour {

	int buttonHeight = 20;
	int buttonWidth = 100;
	

	void OnGUI()
	{
		// Play Game
		if (GUI.Button (new Rect ((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) - buttonHeight, buttonWidth, buttonHeight), "Play"))
		{
			Application.LoadLevel("main");
		}

		// Level Editor
		if (GUI.Button (new Rect ((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) + buttonHeight, buttonWidth, buttonHeight), "Level Editor"))
		{
			Application.LoadLevel("LevelEditor");
		}
	}
}
