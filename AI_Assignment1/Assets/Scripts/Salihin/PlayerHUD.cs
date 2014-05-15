using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	
	Rect PlayerScore;										//introduce variable to store a Rect
	string ScoreDisplay;									//introduce variable to store string displaying score on GUI
	[HideInInspector] public int PlayerPoint;				//introduce variable to store Player's points

	Rect PlayerLives;										//introduce variable to store a Rect
	string LivesDisplay;									//introduce variable to store string displaying lives on GUI
	public int PlayerHealth;								//introduce variable to store Player's health

	void Awake () {
		PlayerPoint = 0;
	}

	// Use this for initialization
	void Start () {

		//initialize the variables for Player's points in this script
		PlayerScore = new Rect(Screen.width * 0.9f, 0.0f, Screen.width * 0.1f, Screen.height * 0.05f);
		ScoreDisplay = "SCORE: " + PlayerPoint.ToString();

		//initialize the variables for Player's health in this script
		PlayerLives = new Rect(0.0f, 0.0f, Screen.width * 0.1f, Screen.height * 0.05f);
		LivesDisplay = "LIVES: " + PlayerHealth.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		//update display of score
		ScoreDisplay = "SCORE: " + PlayerPoint.ToString();

		//update display of health
		LivesDisplay = "LIVES: " + PlayerHealth.ToString();
	}

	//function to display GUI
	void OnGUI()
	{
		//generate a text field on GUI displaying Player's score
		GUI.TextField(PlayerScore, ScoreDisplay);

		//generate a text field on GUI displaying Player's health
		GUI.TextField(PlayerLives, LivesDisplay);
	}
}
