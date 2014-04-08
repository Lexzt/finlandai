using UnityEngine;
using System.Collections;
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
    public int i_SizeDiff = 1;              // Size of each tile in 3D.
	private string filename = "";

    // Reading from text file, through " " delim.
    string[][] readFile(string file)
    {
        string text = System.IO.File.ReadAllText(file);
        string[] lines = Regex.Split(text, "\r\n");
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
		string[][] jagged = readFile("Level1.txt");
        
//        // Random Map Layout. Temp Use
//        // Temp use only, Comment out when done.
//        for (int y = 0; y < 25; y++)
//        {
//            for (int x = 0; x < 25; x++)
//            {
//                int Value = Random.Range(0,4);
//                //Debug.Log((x * y + y) + ": " + Value);
//                Instantiate(PrefabArray[Value], new Vector3(0.5f + (x * i_SizeDiff), 0.5f, 0.5f + (y * i_SizeDiff)), Quaternion.identity);
//            }
//        }
		
        // Map Generator Through Text file
        // Remember to comment back
        for (int y = 0; y < jagged.Length; y++)
        {
            for (int x = 0; x < jagged[0].Length; x++)
            {
                int Value = int.Parse(jagged[y][x]);
				//var Prefab = PrefabArray[Value];

                Debug.Log((x * y + y) + ": " + Value);
				Instantiate(PrefabArray[Value], new Vector3((x * i_SizeDiff) + 0.5f, 0.5f, (y * i_SizeDiff) + 0.5f), Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
