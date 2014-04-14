using UnityEngine;
using System.Collections;

public class DisplayEditorGUI : MonoBehaviour {

	public Vector2 tilesScrollPosition = Vector2.zero;
	public Vector2 levelsScrollPosition = Vector2.zero;

	private int selectionGridInt = 0;

	public bool showLevelsList = false;


	public Object[] PrefabArray;

	public int numberOfPrefabs = 0;


	// Button variables
	public int buttonWidth = 100;
	public int buttonHeight = 20;

	public int buttonHeightSpacing = 20;

	public void DisplayTileButtons()
	{
		tilesScrollPosition = GUI.BeginScrollView(new Rect(10, 10, 150, 800), tilesScrollPosition, new Rect(0, 0, 50, 600));
		for(int i = 0; i < PrefabArray.Length; i++)
		{
			if(GUI.Button(new Rect(0, (buttonHeight * i) + (buttonHeightSpacing * i), buttonWidth, buttonHeight), PrefabArray[i].name))
			{
				GetComponent<EditLevel>().userSelection = i;
			}
		}
		//Debug.Log (Screen.height);
		
		GUI.EndScrollView();
	}


	public void DisplayListOfLoadedLevels()
	{
		levelsScrollPosition = GUI.BeginScrollView(new Rect(150, 10, 150, 800), levelsScrollPosition, new Rect(0, 0, 50, 600));

		for(int i = 0; i < GetComponent<LoadLevels>().levels.Count; i++)
		{
			if(GUI.Button(new Rect(0, (buttonHeight * i) + (buttonHeightSpacing * i), buttonWidth, buttonHeight), GetComponent<LoadLevels>().levels[i].name))
			{
				Debug.Log(GetComponent<LoadLevels>().levels[i].name);

				// Time to load this level!
				GetComponent<EditLevel>().levelName = "Assets/Resources/" + GetComponent<LoadLevels>().levels[i].name + ".txt";
				GetComponent<EditLevel>().LoadChoosenLevel();

				// remove old level memory in lists and array
				showLevelsList = false;
				GetComponent<LoadLevels>().DeleteLevelsFromMemory();
			}
		}

		GUI.EndScrollView();
	}

	// Use this for initialization
	void Awake () 
	{
		// Obtain the invisible prefab containing all of the sub tile prefabs
		GameObject tempHolder = Instantiate(GetComponent<EditLevel>().emptyNode,Vector3.zero,Quaternion.identity) as GameObject;
		PrefabArray = tempHolder.GetComponent<NodeDetails> ().PrefabArray;
		numberOfPrefabs = PrefabArray.Length;
		Debug.Log (numberOfPrefabs);
	}

	void OnGUI()
	{
		DisplayTileButtons ();
		
		// Save the level!
		if (GUI.Button (new Rect (100, Screen.height - (buttonHeight * 3), buttonWidth, buttonHeight), "Save Level"))
		{
			GetComponent<EditLevel>().SaveLevel();
		}

		// Create new Level
		if (GUI.Button (new Rect (110 + buttonWidth, Screen.height - (buttonHeight * 3), buttonWidth, buttonHeight), "Create New Level"))
		{
			//GetComponent<EditLevel>().SaveLevel();
		}

		// Load a Level
		if (GUI.Button (new Rect (110 + buttonWidth, Screen.height - (buttonHeight * 5), buttonWidth, buttonHeight), "Load Level"))
		{
			// load the levels
			GetComponent<LoadLevels>().LoadLevelsIntoMemory ();
			showLevelsList = !showLevelsList;

			// if the list is disabled we remove all text files in memory
			if(!showLevelsList)
			{
				GetComponent<LoadLevels>().DeleteLevelsFromMemory();
			}
		}

		// If the user clicked Load Levels we show don't show depending
		if(showLevelsList)
		{
			DisplayListOfLoadedLevels();
		}

	}
}