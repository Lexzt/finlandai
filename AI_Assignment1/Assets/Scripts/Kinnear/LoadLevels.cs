using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadLevels : MonoBehaviour {

	// this class for loading and storing of textfiles
	public TextAsset [] collectiveLevels;
	public List<TextAsset> waypoints;
	public List<TextAsset> levels;


	public string waypointStringExtension = "PatrolWaypoints";

	public Vector2 scrollPosition = Vector2.zero;
	


	// Use this for initialization
	void Awake () {
	}

	public void DeleteLevelsFromMemory()
	{
		levels.Clear();
		waypoints.Clear();
		System.Array.Clear(collectiveLevels, 0, collectiveLevels.Length);
	}

	public void LoadLevelsIntoMemory()
	{
		collectiveLevels = Resources.LoadAll <TextAsset>("");

		// split text files into waypoint and levels list
		for(int i = 0; i < collectiveLevels.Length; i++)
		{
			if(collectiveLevels[i].name.Contains(waypointStringExtension))
			{
				waypoints.Add(collectiveLevels[i]);
			}
			else
			{
				levels.Add(collectiveLevels[i]);
			}
		}
	}
}