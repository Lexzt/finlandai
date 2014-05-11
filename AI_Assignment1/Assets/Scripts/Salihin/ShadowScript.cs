using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowScript : MonoBehaviour {

	public int Speed;
	private Vector3 Direction;

	private GameObject Pacman;

	public int Trigger_Dist;

	AStarPathfinding AStar;

	private GameObject LvlGen;

	float delay = 2.0f;
	float ticks = 0.0f;
	Vector3 Target;
	int TargetId = 0;

	// Use this for initialization
	void Start () {
		LvlGen = GameObject.Find("LevelGenerator");
		Pacman = LvlGen.GetComponent<LevelGenerator>().Player;

		// This is to intialise the shadow AI and player
		AStar = new AStarPathfinding(transform.gameObject, Pacman, LvlGen);
	}
	
	// Update is called once per frame
	void Update () {

		if(CalcDistance(Pacman.transform.position) <= Trigger_Dist)
		{
			AStar.Init(transform.gameObject, Pacman);
			AStar.InitAStar();
			AStar.Iteration();

			TargetId = AStar.PathList.Count - 1;
		
			if(TargetId > 0)
			{
				if(transform.position == new Vector3(AStar.PathList[TargetId].column + 0.5f, transform.position.y, AStar.PathList[TargetId].row + 0.5f))
				{

					TargetId--;
					Target = new Vector3(AStar.PathList[TargetId].column + 0.5f, transform.position.y, AStar.PathList[TargetId].row + 0.5f);
				}
			}
			else
			{
				Target = transform.position;
			}
			//Debug.Log(transform.position);
			transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed);
		}
	}

	private float CalcDistance(Vector3 Target)
	{
		return ((Target - transform.position).magnitude);
	}
}
