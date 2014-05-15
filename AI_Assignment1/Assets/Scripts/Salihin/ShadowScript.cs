using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowScript : MonoBehaviour {

	//components to be edited in inspector
	public int Speed;
	public int Trigger_Dist;

	//access data for A* Pathfinding
	AStarPathfinding AStar;

	//components to assist in A* Pathfinding
	private GameObject LvlGen;
	private Vector3 Direction;
	private GameObject Pacman;	
	Vector3 Target;
	int TargetId = 0;

	// Use this for initialization
	void Start () {
        LvlGen = GameObject.Find("LevelGenerator");
        Pacman = GameObject.FindGameObjectWithTag("Player");

		// This is to intialise the shadow AI and player
		AStar = new AStarPathfinding(transform.gameObject, Pacman, LvlGen);
	}
	
	// Update is called once per frame
	void Update () {
		//as long as Pacman is in range, start the process of plotting the path towards Pacman using A* Pathfinding
		if(CalcDistance(Pacman.transform.position) <= Trigger_Dist)
		{
			//process plotting of path using A* Pathfinding
			AStar.Init(transform.gameObject, Pacman);
			AStar.InitAStar();
			AStar.Iteration();

			//assign to number of indexes PathList has
			TargetId = AStar.PathList.Count - 1;
		
			//traverse through the indexes as long as there is an index to traverse
			if(TargetId > 0)
			{
				//decrease index value and set target for AI to move towards only if it is at a specific position
				if(transform.position == new Vector3(AStar.PathList[TargetId].column + 0.5f, transform.position.y, AStar.PathList[TargetId].row + 0.5f))
				{
					TargetId--;
					Target = new Vector3(AStar.PathList[TargetId].column + 0.5f, transform.position.y, AStar.PathList[TargetId].row + 0.5f);
				}

			}
			else
			{
				//set targetted position to be the Pacman's position
				Target = Pacman.transform.position;
			}

			//update position move towards Pacman according to the plotted path
			transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed);
		}
	}

	//function to calculate range
	private float CalcDistance(Vector3 Target)
	{
		return ((Target - transform.position).magnitude);
	}
}
