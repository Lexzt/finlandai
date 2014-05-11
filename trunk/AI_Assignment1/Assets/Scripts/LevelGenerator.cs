using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelGenerator : MonoBehaviour {
    /* 
     * Things to do, 
     * 1. All the tile map in Pac man, to Prefab
     * 2. Plotting out array of blocks for A*
     * 3. Enemy Prefab
     * 4. Start Point
     * 5. Level Design
     * 6. Hard code generate first, followed by soft code if there is time.
     * 7. 
     * 8.
     * 9.
     * 10.
    */

    // Currently not in use. 
    //public Object Player;                   // Player prefab, to spawn when created.

    public Object[] PrefabArray;            // Array of walls
    public float f_SizeDiff = 1;              // Size of each tile in 3D.
	public GameObject Player;

	// .txt file to load you level from
	public string levelName = "Assets/Resources/LEVEL_1.txt";
	
	private string waypointStringDifference = "PatrolWaypoints";
	
	public string waypointName;

	// stores the new level's data if theres such a thing
	public List<List<int>> waypointNodes = new List<List<int>>();

	// Stores all the map data in numbers
	public List<List<int>> mapData = new List<List<int>>();


	// to obtain player's information if any AI's need it
	public GameObject playerPointer;


    // Reading from text file, through " " delim.
    string[][] readFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        string[] lines = Regex.Split(text, "\n");
        int rows = lines.Length;

        string[][] levelBase = new string[rows][];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], " ");
            levelBase[i] = stringsOfLine;
        }
        return levelBase;
    }

	// Use this for initialization
    void Start()
    {
		//transform.position = playerSpawn.transform.position;

		// clear the list if there was anything inside it just in case
		waypointNodes.Clear ();

        string[][] jagged = readFile("Assets/Resources/LEVEL_1.txt");

		waypointName = levelName;
		waypointName = waypointName.Insert (waypointName.Length - 4, waypointStringDifference);
		
		string[][] jaggedWaypoints = readFile (waypointName);

        // Map Generator Through Text file
        // Remember to comment back
        for (int y = 0; y < jagged.Length - 1; y++)
        {
			List<int> firstWaypointArray = new List<int>();
			List<int> firstMapData = new List<int>();

            for (int x = 0; x < jagged[0].Length; x++)
            {
                int Value = int.Parse(jagged[y][x]);
                //Debug.Log((x * y + y) + ": " + Value);
                Instantiate(PrefabArray[Value], new Vector3(f_SizeDiff / 2 + (x * f_SizeDiff), f_SizeDiff / 2, f_SizeDiff / 2 + (y * f_SizeDiff)), Quaternion.identity);
				firstWaypointArray.Add(int.Parse(jaggedWaypoints[y][x])); // assign the waypoint to the node
				firstMapData.Add(int.Parse(jagged[y][x]));
            }
			waypointNodes.Add(firstWaypointArray);
			mapData.Add(firstMapData);
        }





		//GameObject playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
		DisableMeshRenderer();

		// Create the player at the Spawn Point
		playerPointer = Instantiate(Player, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, Quaternion.identity) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void DisableMeshRenderer()
	{
		//GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<MeshRenderer>().enabled = false;
		//GameObject.FindGameObjectWithTag("EnemySpawn").GetComponent<MeshRenderer>().enabled = false;
	}
}
