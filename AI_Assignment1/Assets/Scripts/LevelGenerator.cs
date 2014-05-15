using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelGenerator : MonoBehaviour {
    // Things need to do
    // I need to add all their objects with tag AI.
    // I need to add it to destroy
    // When changing level, use Load Level

    // Currently not in use. 
    //public Object Player;                   // Player prefab, to spawn when created.

    public Object[] PrefabArray;            // Array of walls
    public float f_SizeDiff = 1;              // Size of each tile in 3D.
	public GameObject Player;
	public GameObject[] AIArray;

	// .txt file to load you level from
	public string levelLocation = "Assets/Resources/";
	
	private string waypointStringDifference = "PatrolWaypoints";
	
	public string waypointName;

	// stores the new level's data if theres such a thing
    public List<List<List<int>>> TotalwaypointNodes = new List<List<List<int>>>();
    public List<List<int>> waypointNodes;

	// Stores all the map data in numbers
    public List<List<List<int>>> TotalmapData = new List<List<List<int>>>();
    public List<List<int>> mapData;
	
    // Load Level Instance
    private LoadLevels LevelFiles;
    private int CurrentLevelNo;

    private List<GameObject> LoadLevelDestroyObjects;
    private List<List<GameObject>> GameObjectsArray;
    private List<Vector3> PlayerSpawnLocations;

    public static LevelGenerator LevelGeneratorInstance;
	private List<Vector3> TerrainList = new List<Vector3>();
	//private terrainLength = jagged[0].Length;
	//terrainHeight = jagged.Length;

	//GameObject.Find ("Terrain").GetComponent<Terrain> ().terrainData.size = new Vector3 ((float)terrainLength, 1.0f, (float)terrainHeight - 1);
	

    void Awake()
    {
        if (LevelGeneratorInstance != null && LevelGeneratorInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            LevelGeneratorInstance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


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
        // Level Counter
        CurrentLevelNo = 0;

        // Level Files to retrive File Data from Resource Folder
        LevelFiles = GetComponent<LoadLevels>();
        LevelFiles.LoadLevelsIntoMemory();

        // Initalize First Level
        GameObjectsArray = new List<List<GameObject>>();
        PlayerSpawnLocations = new List<Vector3>();

        // Parent of All Objects in Scene (Testing out Optimizations)
        GameObject ListOfLevels = new GameObject();
        ListOfLevels.name = "ListOfLevels";

        // Load All levels in the resource folder from Kinnear Code.
        for (int i = 0; i < LevelFiles.levels.Count; i++)
        {
			mapData = new List<List<int>>();
			waypointNodes = new List<List<int>>();

            GameObject LevelNo = new GameObject();
            LevelNo.name = "Level " + (i + 1);
            LevelNo.transform.parent = ListOfLevels.transform;

            // Returns Array of All objects. 
            List<GameObject> TempArray = LoadLevelInit(levelLocation + LevelFiles.levels[i].name + ".txt",i);
            // Create the player at the Spawn Point
            foreach (GameObject v in TempArray)
            {
                v.SetActive(false);
                v.transform.parent = LevelNo.transform;
            }
            // Add the Level Objects into the 2d Array, disabled to not show anything.
            GameObjectsArray.Add(TempArray);

			TotalmapData.Add(mapData);
			TotalwaypointNodes.Add(waypointNodes);
        }

        // Load First Level
        LoadLevel(CurrentLevelNo);
		
		mapData = TotalmapData[CurrentLevelNo];
		waypointNodes = TotalwaypointNodes[CurrentLevelNo];
		
		// Initalize Player
        Instantiate(Player, FindPlayerWaypoint(CurrentLevelNo), Quaternion.identity);
        for (int i = 0; i < AIArray.Length; i++)
        {
            Instantiate(AIArray[i], FindEnemyWaypoint(CurrentLevelNo), Quaternion.identity);
        }

		GameObject.Find ("Terrain").GetComponent<Terrain> ().terrainData.size = TerrainList[CurrentLevelNo];
    }

    List<GameObject> LoadLevelInit(string LevelName,int LoopNo)
    {
        // List of New Game Object per Level
        List<GameObject> LevelValues = new List<GameObject>();

        // clear the list if there was anything inside it just in case
        waypointNodes.Clear();

        string[][] jagged = readFile(LevelName);

        waypointName = LevelName;
        waypointName = waypointName.Insert(waypointName.Length - 4, waypointStringDifference);

        string[][] jaggedWaypoints = readFile(waypointName);

        // Map Generator Through Text file
        // Remember to comment back
        for (int y = 0; y < jagged.Length - 1; y++)
        {
            List<int> firstWaypointArray = new List<int>();
            List<int> firstMapData = new List<int>();

            for (int x = 0; x < jagged[0].Length; x++)
            {
                int Value = int.Parse(jagged[y][x]);
                LevelValues.Add(Instantiate(PrefabArray[Value], new Vector3(f_SizeDiff / 2 + (x * f_SizeDiff), f_SizeDiff / 2, f_SizeDiff / 2 + (y * f_SizeDiff)), Quaternion.identity) as GameObject);
                firstWaypointArray.Add(int.Parse(jaggedWaypoints[y][x])); // assign the waypoint to the node
                firstMapData.Add(int.Parse(jagged[y][x]));
            }
            waypointNodes.Add(firstWaypointArray);
            mapData.Add(firstMapData);
        }

		TerrainList.Add (new Vector3 (jagged.Length - 1,1.0f,jagged[0].Length));
        DisableMeshRenderer("PlayerSpawn");
        DisableMeshRenderer("EnemySpawn");

        return LevelValues;
    }

    void LoadLevel(int LevelNo)
    {
        foreach (GameObject v in GameObjectsArray[LevelNo])
        {
            v.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LoadNextLevel();
        }
	}

    void DestroyAllObjects()
    {
        LoadLevelDestroyObjects = new List<GameObject>();

        GameObject[] ObjectArray        = GameObject.FindGameObjectsWithTag ("Bits");
        GameObject[] BigBitsArray       = GameObject.FindGameObjectsWithTag ("BigBits");
        GameObject[] EmptyArray         = GameObject.FindGameObjectsWithTag ("Empty");
        GameObject[] EnvironmentArray   = GameObject.FindGameObjectsWithTag ("EnvironmentCube");
        GameObject[] AIList             = GameObject.FindGameObjectsWithTag ("Ghost");
        GameObject EnemySpawn           = GameObject.FindGameObjectWithTag  ("EnemySpawn");
        GameObject PlayerSpawn          = GameObject.FindGameObjectWithTag  ("PlayerSpawn");


        foreach (GameObject v in ObjectArray)
        {
            LoadLevelDestroyObjects.Add(v);
        }

        foreach (GameObject v in BigBitsArray)
        {
            LoadLevelDestroyObjects.Add(v);
        }

        foreach (GameObject v in EmptyArray)
        {
            LoadLevelDestroyObjects.Add(v);
        }

        foreach (GameObject v in EnvironmentArray)
        {
            LoadLevelDestroyObjects.Add(v);
        }

        foreach (GameObject v in AIList)
        {
            LoadLevelDestroyObjects.Add(v);
        }
        LoadLevelDestroyObjects.Add(EnemySpawn);
        LoadLevelDestroyObjects.Add(PlayerSpawn);

        foreach (GameObject v in LoadLevelDestroyObjects)
        {
            Destroy(v);
        }
    }

    void DisableLevel(int LevelNo)
    {
        foreach (GameObject v in GameObjectsArray[LevelNo])
        {
            v.SetActive(false);
        }
    }

    void EnableLevel(int LevelNo)
    {
        foreach (GameObject v in GameObjectsArray[LevelNo])
        {
            v.SetActive(true);
        }
    }

	void DisableMeshRenderer(string tag)
	{
		GameObject.FindGameObjectWithTag(tag).GetComponent<MeshRenderer>().enabled = false;
	}

	object SpawnPrefab(ref GameObject obj, string tag)
	{
		obj = Instantiate((Object)obj, GameObject.FindGameObjectWithTag(tag).transform.position, Quaternion.identity) as GameObject;
        return obj;
	}

    object SpawnPrefab(ref GameObject obj, Vector3 tempPos)
    {
        obj = Instantiate((Object)obj, tempPos, Quaternion.identity) as GameObject;
        return obj;
    }

    void LoadNextLevel()
    {
        // Disable Current Level AI
        GameObject[] GhostObj = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject v in GhostObj)
        {
            v.SetActive(false);
            Destroy(v);
        }
        GameObject[] SpawnObj = GameObject.FindGameObjectsWithTag("Spawner");
		foreach (GameObject v in SpawnObj)
		{
			v.SetActive(false);
			Destroy(v);
		}

		// Disable Current Level and Enable next level
        DisableLevel(CurrentLevelNo);
        EnableLevel(++CurrentLevelNo);
		GameObject.Find ("Terrain").GetComponent<Terrain> ().terrainData.size = TerrainList[CurrentLevelNo];
		
        mapData = TotalmapData[CurrentLevelNo];
        waypointNodes = TotalwaypointNodes[CurrentLevelNo];

        // Change Current Player Position
        GameObject Playerobj = GameObject.FindGameObjectWithTag("Player");
        Destroy(Playerobj);

        Instantiate(Player, FindPlayerWaypoint(CurrentLevelNo), Quaternion.identity);

        // Initalize a New Set of AI
        for (int i = 0; i < AIArray.Length; i++)
        {
            Instantiate(AIArray[i], FindEnemyWaypoint(CurrentLevelNo), Quaternion.identity);
        }
    }

    Vector3 FindPlayerWaypoint(int LevelNo)
    {
        for (int i = 0; i < GameObjectsArray[LevelNo].Count; i++)
        {
            if (GameObjectsArray[LevelNo][i].tag == "PlayerSpawn")
            {
                Vector3 tempPos = GameObjectsArray[LevelNo][i].transform.position;
                Debug.Log(tempPos.x + ", " + tempPos.y + ", " + tempPos.z);
                return tempPos;
            }
        }
        return new Vector3(0, 0, 0);
    }

    Vector3 FindEnemyWaypoint(int LevelNo)
    {
        for (int i = 0; i < GameObjectsArray[LevelNo].Count; i++)
        {
            if (GameObjectsArray[LevelNo][i].tag == "EnemySpawn")
            {
                Vector3 tempPos = GameObjectsArray[LevelNo][i].transform.position;
                Debug.Log(tempPos.x + ", " + tempPos.y + ", " + tempPos.z);
                return tempPos;
            }
        }
        return new Vector3(0, 0, 0);
    }

    public GameObject[] CurrentActiveLevel ()
    {
        List<GameObject> ReturnArray = new List<GameObject>();
        foreach (GameObject v in GameObjectsArray[CurrentLevelNo])
        {
            ReturnArray.Add(v);
        }
        return ReturnArray.ToArray();
    }
}
