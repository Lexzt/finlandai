using UnityEngine;
using System.Collections;

public enum TypeOfInput
{
	NORMAL_TILE = 0,
	WAYPOINT,
};

public class DisplayEditorGUI : MonoBehaviour {

	public Vector2 tilesScrollPosition = Vector2.zero;
	public Vector2 levelsScrollPosition = Vector2.zero;

	private int selectionGridInt = 0;

	public bool showLevelsList = false;

	// decide to prompt the user to create a new level or not
	public bool createNewLevel = false;

	float rectWidthSize = 500.0f;
	float rectHeightSize = 200.0f;

	public Rect windowRect;

	public string newLevelName = "";
	public string newTerrainHeight = "";
	public string newTerrainWidth = "";

	// Decides if we're building using a waypoint or a standard tile selection
	//public bool waypointApplication = false;

	//public bool advancedWaypointApplication = false;

	public Object[] PrefabArray;

	public int numberOfPrefabs = 0;

	public TypeOfInput inputType = TypeOfInput.NORMAL_TILE;


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
				//waypointApplication = false;
				inputType = TypeOfInput.NORMAL_TILE;
				GetComponent<EditLevel>().userSelection = i;
			}
		}

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

	void DisplayCreateNewLevel(int windowID)
	{
		//we show the UI for creating a level

		// Save the level!
		if (GUI.Button (new Rect (250, 170, buttonWidth, buttonHeight), "Save Level"))
		{
			// validate if the user has entered valid items
			if(int.Parse(newTerrainHeight) > 0 && int.Parse(newTerrainWidth) > 0 && newLevelName != "")
			{
				GetComponent<EditLevel>().CreateNewLevel("Assets/Resources/" + newLevelName + ".txt", int.Parse(newTerrainWidth), int.Parse(newTerrainHeight));
			}
			else 
			{
				Debug.Log("Invalid Terrain Width or Height Please enter a value more than 0. Or the Map Name does not contain any characters!");
			}
		}

		// Save the level!
		if (GUI.Button (new Rect (350, 170, buttonWidth, buttonHeight), "Cancel"))
		{
			createNewLevel = !createNewLevel;
		}

		GUI.Label (new Rect (25, 25, 100, 30), "Map Name: ");

		GUI.Label (new Rect (25, 75, 100, 30), "Map Size: ");

		
		newLevelName = GUI.TextField (new Rect (120, 25, 350, 30), newLevelName);
		newTerrainHeight = GUI.TextField (new Rect (120, 75, 350, 30), newTerrainHeight);
		newTerrainWidth = newTerrainHeight;
	}

	// Use this for initialization
	void Awake () 
	{
		windowRect = new Rect ((Screen.width/2) - (rectWidthSize/2), (Screen.height/2) - (rectHeightSize/2), rectWidthSize, rectHeightSize);

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
			//toggle the new level menu
			createNewLevel = !createNewLevel;
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

		// Add a waypoint
		if (GUI.Button (new Rect (100, Screen.height - (buttonHeight * 5), buttonWidth, buttonHeight), "Add Waypoint"))
		{
			// Switch from non waypoint to waypoint highlight
			inputType = TypeOfInput.WAYPOINT;
		}

		// If the user clicked Load Levels we show don't show depending
		if(showLevelsList)
		{
			DisplayListOfLoadedLevels();
		}

		if(createNewLevel)
		{
			windowRect = GUI.Window (0, windowRect, DisplayCreateNewLevel, "");
		}

		//Debug.Log ("Waypointapplication: " + waypointApplication);

	}
}