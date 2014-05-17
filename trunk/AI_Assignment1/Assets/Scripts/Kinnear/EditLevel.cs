using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
* This script creates an entirefield of gameobjects depending on the size of the terrain width and length
*/

public class EditLevel : MonoBehaviour {

	// .txt file to load you level from
	public string levelName = "Assets/Resources/Level1.txt";

	private string waypointStringDifference = "PatrolWaypoints";

	public string waypointName;

	// our selection according to the user
	public int userSelection = 2;

	// the height and the length of the terrain to obtain size use these values and times
	private int terrainHeight = 0;
	private int terrainLength = 0;

	// Size of the final prefab object
	private float i_SizeDiff = 1.0f;
	
	// storing all the level details
	List<List<GameObject>> levelNodes = new List<List<GameObject>>();

	// Our clickable object container that stores all the rest of our display prefabs!
	public Object emptyNode;

	private GameObject previousNode;

	public int playerPrefabNumber = 4;
	public int enemyPrefabNumber = 5;

	// Line positions for waypoints
	public List<Vector3> lineConnector = new List<Vector3> ();

	// stores the new level's data if theres such a thing
	public List<List<int>> newLevelNodes = new List<List<int>> ();

	
	public void CreateNewLevel(string newMapName, int newTerrainWidth, int newTerrainHeight)
	{
		newLevelNodes.Clear ();

		string newWaypointName = newMapName;
		newWaypointName = newWaypointName.Insert (newWaypointName.Length - 4, waypointStringDifference);

		// Initialise the list of new level nodes
		for(int i = 0; i < newTerrainWidth; i++)
		{
			List<int> row = new List<int>();

			for(int j = 0; j < newTerrainHeight; j++)
			{
				row.Add(0);
			}	
			newLevelNodes.Add(row);
		}

		string saveText = "";
		
		// Write the string to a file.
		System.IO.StreamWriter file = new System.IO.StreamWriter(newMapName);
		
		// concentanate the entire string into one long ass line
		for(int i = 0; i < newLevelNodes.Count; i++)
		{
			for(int j = 0; j < newLevelNodes[0].Count; j++)
			{
				saveText += newLevelNodes[i][j];
				
				if(j != newLevelNodes.Count - 1)
				{
					saveText += " ";
				}
			}
			
			if(i != newLevelNodes.Count - 1)
			{
				saveText += "\n";
			}
		}
		
		file.WriteLine(saveText);
		
		file.Close();


		saveText = "";

		// Write the string to the waypoint file
		System.IO.StreamWriter waypointFile = new System.IO.StreamWriter(newWaypointName);
		
		// concentanate the entire string into one long ass line
		for(int i = 0; i < newLevelNodes.Count; i++)
		{
			for(int j = 0; j < newLevelNodes[0].Count; j++)
			{
				saveText += newLevelNodes[i][j];
				
				if(j != newLevelNodes.Count - 1)
				{
					saveText += " ";
				}
			}
			
			if(i != newLevelNodes.Count - 1)
			{
				saveText += "\n";
			}
		}
		
		waypointFile.WriteLine(saveText);
		
		waypointFile.Close();
	}



	
	
	// Reading from text file, through " " delim.
	string[][] readFile(string file)
	{
		string text = System.IO.File.ReadAllText(file);
		string[] lines = Regex.Split(text,"\n");
		int rows = lines.Length;
		
		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++)
		{
			string[] stringsOfLine = Regex.Split(lines[i], " ");
			levelBase[i] = stringsOfLine;
		}
		return levelBase;
	}

	// Clears the pathnamed text file to be completely empty
	void ClearTextFile(string path)
	{
		System.IO.File.WriteAllText(path, string.Empty);
	}

	void SaveFile()
	{
		string saveText = "";

		// Write the string to a file.
		System.IO.StreamWriter file = new System.IO.StreamWriter(levelName);

		// concentanate the entire string into one long ass line
		for(int i = 0; i < levelNodes.Count; i++)
		{
			for(int j = 0; j < levelNodes[0].Count; j++)
			{
				saveText += (levelNodes[i][j].GetComponent<NodeDetails>().id).ToString();

				if(j != levelNodes.Count - 1)
				{
					saveText += " ";
				}
			}

			if(i != levelNodes.Count - 1)
			{
				saveText += "\n";
			}
		}

		if(saveText.EndsWith("\n"))
		{
			Debug.Log("There is a carriage return");
		}

		file.WriteLine(saveText);
		
		file.Close();
	}

	void SaveWaypointFile()
	{
		string saveText = "";
		
		// Write the string to a file.
		System.IO.StreamWriter file = new System.IO.StreamWriter(waypointName);
		
		// concentanate the entire string into one long ass line
		for(int i = 0; i < levelNodes.Count; i++)
		{
			for(int j = 0; j < levelNodes[0].Count; j++)
			{
				saveText += (levelNodes[i][j].GetComponent<NodeDetails>().waypointID).ToString();
				
				if(j != levelNodes.Count - 1)
				{
					saveText += " ";
				}
			}
			
			if(i != levelNodes.Count - 1)
			{
				saveText += "\n";
			}
		}
		
		if(saveText.EndsWith("\n"))
		{
			Debug.Log("There is a carriage return");
		}
		
		file.WriteLine(saveText);
		
		file.Close();
	}

	public void SaveLevel()
	{
		ClearTextFile(levelName);
		SaveFile();
		SaveWaypointFile ();
	}

	public void LoadChoosenLevel()
	{
		// if there were any old gameobjects that were loaded then we delete them all

		for (int i = 0; i < levelNodes.Count; i++)
		{
			for (int j = 0; j < levelNodes[0].Count; j++)
			{
				Destroy(levelNodes[i][j]);
			}
		}

		levelNodes.Clear ();


		
		// read the text file to check what is the height and length
		string[][] jagged = readFile(levelName);

		waypointName = levelName;
		waypointName = waypointName.Insert (waypointName.Length - 4, waypointStringDifference);

		string[][] jaggedWaypoints = readFile (waypointName);

		Debug.Log (jagged.Length);
		Debug.Log (jagged[0].Length);
		
		// assign how big our terrain should be! [:
		terrainLength = jagged[0].Length;
		terrainHeight = jagged.Length;
		
		Debug.Log("Terrain Height: " + terrainHeight);
		Debug.Log("Terrain Length: " + terrainLength);

		Debug.Log("Waypoint Height: " + jaggedWaypoints[0].Length);
		Debug.Log("Waypoint Length: " + jaggedWaypoints.Length);
		
		
		for (int y = 0; y < jagged.Length - 1; y++)
		{
			List<GameObject> firstDimensionArray = new List<GameObject>();
			
			for (int x = 0; x < jagged[0].Length; x++)
			{
				int Value = int.Parse(jagged[y][x]);
				//Debug.Log("X: " + x);
				//Debug.Log("Y: " + y);
				GameObject assignedNode = Instantiate(emptyNode, new Vector3(x * i_SizeDiff, 0, y * i_SizeDiff), Quaternion.identity) as GameObject;
				
				//Assign the node a specific colour!
				assignedNode.GetComponent<NodeDetails>().id = int.Parse(jagged[y][x]);
				assignedNode.GetComponent<NodeDetails>().x = x;
				assignedNode.GetComponent<NodeDetails>().y = y;
				assignedNode.GetComponent<NodeDetails>().waypointID = int.Parse(jaggedWaypoints[y][x]); // assign the waypoint to the node
				assignedNode.GetComponent<NodeDetails>().SetTypeOfPrefab();

				// Finally store the node
				firstDimensionArray.Add(assignedNode);
			}
			
			levelNodes.Add(firstDimensionArray);
		}

		// set terrain variables here
		// Terrain height has to minus one because of the EXTRA LINE SPACE IN THE TEXT FILE!
		GameObject.Find ("Terrain").GetComponent<Terrain> ().terrainData.size = new Vector3 ((float)terrainLength, 1.0f, (float)terrainHeight - 1);
	}



	// Checks if the last checkpoint is valid for the user to place for the AI to move from the previous point
	int CheckIfWaypointIsValid(int x, int y)
	{
		int latestWaypoint = 0;
		int enemyStartX = 0;
		int enemyStartY = 0;

		// list of passable tiles the Ai can walk through
		List<int> passableTiles = new List<int>();
		passableTiles.Add (0);
		passableTiles.Add (4);
		passableTiles.Add (5);
		passableTiles.Add (6);

		for (int i = 0; i < levelNodes.Count; i++)
		{
			for (int j = 0; j < levelNodes[0].Count; j++)
			{
				// first we check what is the latest waypoint we have specified in the list
				if(levelNodes[i][j].GetComponent<NodeDetails>().waypointID > latestWaypoint)
				{
					// save the latest waypoint's position into memory
					latestWaypoint = levelNodes[i][j].GetComponent<NodeDetails>().waypointID;
					enemyStartX = j;
					enemyStartY = i;
				}
			}
		}
		Debug.Log ("Latest Waypoint in the CSV: " + latestWaypoint);

		if(levelNodes[y][x].GetComponent<NodeDetails>().waypointID == 0)
		{

		// this means that we have not specified a waypoint before [:
		if(latestWaypoint == 0)
		{
			// obtain the location of the enemy spawn point
			for (int i = 0; i < levelNodes.Count; i++)
			{
				for (int j = 0; j < levelNodes[0].Count; j++)
				{
					// we check where is the enemy spawn point and we obtain x and y coordinates
					if(levelNodes[i][j].GetComponent<NodeDetails>().id == enemyPrefabNumber)
					{
						enemyStartX = levelNodes[i][j].GetComponent<NodeDetails>().x;
						enemyStartY = levelNodes[i][j].GetComponent<NodeDetails>().y;
						break;
					}
				}
			}
			
			//compare if the waypoint declared by the user is possible to move to from the enemy location on the map
			// First we compare to see if the row or column that the user has picked is within the same row or column
			if(x == enemyStartX|| y == enemyStartY)
			{
				// Next we check if theres anything inbetween the two nodes that we cannot walk through
				if(x == enemyStartX)
				{
					// We can confirm that the user has clicked on the left side of the starting node
					if(y < enemyStartY)
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = y; i < enemyStartY; i++)
						{
							if(!passableTiles.Contains(levelNodes[i][x].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
					else
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = enemyStartY; i <= y; i++)
						{
							if(!passableTiles.Contains(levelNodes[i][x].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}

				}
				else if(y == enemyStartY)
				{
					// We can confirm that the user has clicked on the left side of the starting node
					if(x < enemyStartX)
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = x; i < enemyStartX; i++)
						{
							if(!passableTiles.Contains(levelNodes[y][i].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
					else
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = enemyStartX; i <= x; i++)
						{
							if(!passableTiles.Contains(levelNodes[y][i].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
				}

				return 1;
			}
		}
		else
		{
//			// it's not our first waypoint
//			// so we take the latest waypoint's node's position and we compare it to our clicked position!
//			// compare if the waypoint declared by the user is possible to move to from the enemy location on the map
//			// First we compare to see if the row or column that the user has picked is within the same row or column

			// The user has selected another waypoint
			if(x == enemyStartX && y == enemyStartY)
			{
				// remove that waypoint and another other waypoint numbers that are after this waypoint
				//UnselectedWaypoint(x, y);
				return 0;
			}
			else if(x == enemyStartX || y == enemyStartY)
			{
				// Next we check if theres anything inbetween the two nodes that we cannot walk through
				if(x == enemyStartX)
				{
					// We can confirm that the user has clicked on the left side of the starting node
					if(y < enemyStartY)
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = y; i < enemyStartY; i++)
						{
							if(!passableTiles.Contains(levelNodes[i][x].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
					else
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = enemyStartY; i <= y; i++)
						{
							if(!passableTiles.Contains(levelNodes[i][x].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
					
				}
				else if(y == enemyStartY)
				{
					// We can confirm that the user has clicked on the left side of the starting node
					if(x < enemyStartX)
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = x; i < enemyStartX; i++)
						{
							if(!passableTiles.Contains(levelNodes[y][i].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
					else
					{
						// Check everything between both nodes if there are any nodes that are impassable
						for(int i = enemyStartX; i <= x; i++)
						{
							if(!passableTiles.Contains(levelNodes[y][i].GetComponent<NodeDetails>().id))
							{
								return 0;
							}
						}
					}
				}
				
					return latestWaypoint + 1;
				}

			}
		}
		// this means that the user is attempting to delete a waypoint
		else
		{
			UnselectedWaypoint(x, y);
		}

		return 0;
	}

	// remove all waypoints that are after this node's waypoint ID
	void UnselectedWaypoint(int x, int y)
	{
		int removeFromWaypointID = levelNodes [y] [x].GetComponent<NodeDetails> ().waypointID;
		Debug.Log ("Remove from CSV any waypoint above: " + removeFromWaypointID);

		// do until the waypoints id in the levelNodes list doesnt increase again
		for (int i = 0; i < levelNodes.Count; i++)
		{
			for (int j = 0; j < levelNodes[0].Count; j++)
			{
				// we get the next waypoint in the list (IF THERE SUCH A WAYPOINT HAHA)
				if(levelNodes[i][j].GetComponent<NodeDetails>().waypointID > removeFromWaypointID)
				{
					// set the waypoint value to 0
					levelNodes[i][j].GetComponent<NodeDetails>().waypointID = 0;
				}
			}
		}
	}


	void Awake()
	{
		GameObject.Find ("Terrain").transform.position -= new Vector3 (0.5f, 0.0f, 0.5f);
		
		LoadChoosenLevel ();
	}

	// store the waypoint positions into a List
	void StoreWaypointLine()
	{
		lineConnector.Clear ();

		// obtain the location of the enemy spawn point
		for (int i = 0; i < levelNodes.Count; i++)
		{
			for (int j = 0; j < levelNodes[0].Count; j++)
			{
				// we check where is the enemy spawn point and we obtain x and y coordinates
				if(levelNodes[i][j].GetComponent<NodeDetails>().id == enemyPrefabNumber)
				{
					lineConnector.Add(levelNodes[i][j].transform.position + new Vector3(0.0f, 0.5f, 0.0f));
					break;
				}
			}
		}

		int previousWaypointFigure = 0;
		int currentWaypointFigure = 0;

		// do until the waypoints id in the levelNodes list doesnt increase again
		do
		{
			previousWaypointFigure++;

			for (int i = 0; i < levelNodes.Count; i++)
			{
				for (int j = 0; j < levelNodes[0].Count; j++)
				{
					// we get the next waypoint in the list (IF THERE SUCH A WAYPOINT HAHA)
					if(levelNodes[i][j].GetComponent<NodeDetails>().waypointID == currentWaypointFigure + 1)
					{
						// save the latest waypoint's position into memory
						lineConnector.Add(levelNodes[i][j].transform.position + new Vector3(0.0f, 0.5f, 0.0f));
						currentWaypointFigure++;
						break;
					}
				}
			}

		}while(currentWaypointFigure >= previousWaypointFigure);

	}
	
	// Update is called once per frame
	void Update () 
	{
		// Hover ray for object outline
		Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit1 = new RaycastHit();
		
		if (Physics.Raycast (ray1, out hit1))
		{
			Debug.Log("Hit something");
			// if we hit a transformable gameobject	
			if(hit1.collider.transform.gameObject.GetComponent<ToggleOutline>() != null)
			{
				
				if(previousNode == hit1.collider.gameObject)
				{
					hit1.transform.gameObject.GetComponent<NodeDetails>().OnNodeOutline();
				}
				else
				{
					// can optimise this to change only the old node's shader!
					for(int i = 0; i < levelNodes.Count; i++)
					{
						for(int j = 0; j < levelNodes[0].Count; j++)
						{
							levelNodes[i][j].GetComponent<NodeDetails>().OffNodeOutline();
						}
					}
				}

				previousNode = hit1.collider.gameObject;

				Debug.Log("Turn on the outline!");
			}
		}
		else
		{
			for(int i = 0; i < levelNodes.Count; i++)
			{
				for(int j = 0; j < levelNodes[0].Count; j++)
				{
					levelNodes[i][j].GetComponent<NodeDetails>().OffNodeOutline();
				}
			}
		}


		// Click to change a node's prefab
		if(Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast (ray, out hit))
			{
					// if we have a gameobject that is not null
					if(hit.collider.transform.gameObject.GetComponent<NodeDetails>() != null)
					{
							// if we hit a transformable gameobject and we've not selected a waypoint
							if(GetComponent<DisplayEditorGUI>().inputType == TypeOfInput.NORMAL_TILE)
							{
								if(userSelection == playerPrefabNumber  || userSelection == enemyPrefabNumber) // this we have specified a player/enemy spawn location we have to remove any other player/enemy start points
								{
									for(int i = 0; i < levelNodes.Count; i++)
									{
										for(int j = 0; j < levelNodes[0].Count; j++)
										{
											if(levelNodes[i][j].GetComponent<NodeDetails>().id == userSelection)
											{
												levelNodes[i][j].GetComponent<NodeDetails>().id = 0;
												levelNodes[i][j].GetComponent<NodeDetails>().SetTypeOfPrefab();
											}
										}	
									}
								}

								hit.transform.gameObject.GetComponent<NodeDetails>().id = userSelection;
								hit.transform.gameObject.GetComponent<NodeDetails>().SetTypeOfPrefab();

								// if we accidentally tried to place a block on a waypoint remove the rest of the waypoints!
								if(hit.transform.gameObject.GetComponent<NodeDetails>().waypointID != 0)
								{
									UnselectedWaypoint(hit.transform.gameObject.GetComponent<NodeDetails>().x, hit.transform.gameObject.GetComponent<NodeDetails>().y);
									hit.transform.gameObject.GetComponent<NodeDetails>().waypointID = 0;
							}
							}
						
							// We check if the waypoint is valid and then we apply the waypoint indication number and then we set that node in the level to 0 which is clear block
							else if(GetComponent<DisplayEditorGUI>().inputType == TypeOfInput.WAYPOINT)
							{
								// calculate if we can set a waypoint here!
								hit.transform.gameObject.GetComponent<NodeDetails>().waypointID = CheckIfWaypointIsValid(hit.transform.gameObject.GetComponent<NodeDetails>().x, hit.transform.gameObject.GetComponent<NodeDetails>().y);
								Debug.Log("New Waypoint number is : " + hit.transform.gameObject.GetComponent<NodeDetails>().waypointID);
							}
					}

			}
		}

		// Detection for mouse scrolling the camera top and down
		if(!GetComponent<DisplayEditorGUI>().showLevelsList)
		{
			// Dont scroll on the camera
			Camera.main.transform.position += -(Vector3.up * Input.GetAxis("Mouse ScrollWheel"));
		}

		Debug.Log ("Mouse Scroll Wheel value : " + (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel")));

		if(Input.GetKey(KeyCode.Alpha1))
		{
			SaveLevel();
		}

		StoreWaypointLine ();
	}

}