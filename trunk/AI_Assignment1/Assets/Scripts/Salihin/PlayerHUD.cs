using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	
	Rect PlayerScore;										//introduce variable to store a Rect
	string ScoreDisplay;									//introduce variable to store string displaying score on GUI
	public int PlayerPoint;									//introduce variable to store Player's points

	// Use this for initialization
	void Start () {

		//initialize the variables in this script
		PlayerScore = new Rect(Screen.width * 0.9f, 0.0f, Screen.width * 0.1f, Screen.height * 0.05f);
		PlayerPoint = 0;
		ScoreDisplay = "SCORE: " + PlayerPoint.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		//update display of score
		ScoreDisplay = "SCORE: " + PlayerPoint.ToString();
	}

	//function to display GUI
	void OnGUI()
	{
		//generate a text field on GUI displaying Player's score
		GUI.TextField(PlayerScore, ScoreDisplay);
	}
}
