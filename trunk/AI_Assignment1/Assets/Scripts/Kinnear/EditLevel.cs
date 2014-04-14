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

	public void SaveLevel()
	{
		ClearTextFile(levelName);
		SaveFile();
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
		
		Debug.Log (jagged.Length);
		Debug.Log (jagged[0].Length);
		
		// assign how big our terrain should be! [:
		terrainLength = jagged[0].Length;
		terrainHeight = jagged.Length;
		
		Debug.Log("Terrain Height: " + terrainHeight);
		Debug.Log("Terrain Length: " + terrainLength);
		
		
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
				assignedNode.GetComponent<NodeDetails>().SetTypeOfPrefab();
				
				// Finally store the node
				firstDimensionArray.Add(assignedNode);
			}
			
			levelNodes.Add(firstDimensionArray);
		}
	}


	void Awake()
	{
		LoadChoosenLevel ();
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


		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast (ray, out hit))
			{
				//Debug.Log("Hit something");
				
				// if we hit a transformable gameobject
				if(hit.collider.transform.gameObject.GetComponent<NodeDetails>() != null)
				{
					hit.transform.gameObject.GetComponent<NodeDetails>().id = userSelection;
					hit.transform.gameObject.GetComponent<NodeDetails>().SetTypeOfPrefab();
				}
			}
		}

		if(Input.GetKey(KeyCode.Alpha1))
		{
			SaveLevel();
		}

	}

}