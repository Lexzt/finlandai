using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Prowler : MonoBehaviour {

	// Obtain the list of nodes that we are supposed to walk to
	List<Vector3> waypoints = new List<Vector3>();

	List<List<int>> waypointNodes = new List<List<int>>();
	

	// Used to store the AI gameobject
	GameObject prowler = new GameObject();

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
	void Awake () {
		// Spawn a prefab into the world!
		//Resources.Load ();

		// Load the waypoints into the AI


	}
	
	// Update is called once per frame
	void Update () {
	




	}
}
