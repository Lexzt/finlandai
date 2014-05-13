using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAlgorithm {
	private static List<GameObject> ReconstructPath(Dictionary<GameObject,GameObject> cameFrom, GameObject current)
	{
		List<GameObject> path = new List<GameObject> ();

		while(cameFrom.ContainsKey(current))
		{
			path.Add(current);
			current = cameFrom[current];
		}
		path.Add (current);
		path.Reverse ();
		return path;
	}

	private static List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 Current)
	{
        // Reconstruct path by inverse the path
        // Return path values
        List<Vector2> path = new List<Vector2>();
        while (cameFrom.ContainsKey(Current))
        {
            path.Add(Current);
            Current = cameFrom[Current];
        }

        path.Add(Current);
        path.Reverse();
        return path;

        //Stack<Vector2> S = new Stack<Vector2>();
        //while (cameFrom.ContainsKey(Current))
        //{
        //    S.Push(Current);
        //    Current = cameFrom[Current];
        //}
        //return S;
	}

	// Basic Heuristic Calculation by Distance, Can change to Manhatten if really needed
	public static float HeuristicCostEstimate(Vector2 from, Vector2 to)
	{
		float score = 0.0f;
		score = Vector2.Distance (from, to);
		return score;
	}

	public static float HeuristicCostEstimate(GameObject from, GameObject to)
	{
		float score = 0.0f;
		score = Vector3.Distance (from.transform.position, to.transform.position);
		return score;
	}
	
	// Get lowest value from the list, to add to the path.
	public static Vector2 FindLowestValue(List<Vector2> fromList, Dictionary<Vector2,float> table)
	{
		float bestValue = -1.0f;
		Vector2 bestPosition = new Vector2 ();
		foreach(Vector2 Position in fromList)
		{
			if(bestValue == -1.0f || table[Position] < bestValue)
			{
				bestPosition = Position;
				bestValue = table[Position];
			}
		}

		return bestPosition;
	}

	public static GameObject FindLowestValue(List<GameObject> fromList, Dictionary<GameObject,float> table)
	{
		float bestValue = -1.0f;
		GameObject bestPosition = new GameObject ();
		foreach(GameObject Position in fromList)
		{
			if(bestValue == -1.0f || table[Position] < bestValue)
			{
				bestPosition = Position;
				bestValue = table[Position];
			}
		}
		
		return bestPosition;
	}

	public static List<GameObject> AStarNew(GameObject[] Graph, GameObject Source, GameObject Target)
	{
		// Open and Closed list
		List<GameObject> closedSet 	= new List<GameObject>();
		List<GameObject> openSet 	= new List<GameObject>();

		Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject> ();
		Dictionary<GameObject, float> g_score 		= new Dictionary<GameObject, float> ();
		Dictionary<GameObject, float> f_score 		= new Dictionary<GameObject, float> ();

		openSet.Add (Source);

		// Cost from start is zero
		g_score [Source] = 0.0f;
		
		// Estimate cost from goal through y
		f_score [Source] = g_score [Source] + HeuristicCostEstimate (Source, Target);

		while(openSet.Count > 0)
		{
			// Find Lowest F Score
			GameObject Current = FindLowestValue(openSet,f_score);

			if(Current == Target)
			{
				// Fig out later
				Debug.Log("Constructing Path......");
				return ReconstructPath(cameFrom,Current);
			}
			openSet.Remove(Current);
			closedSet.Add(Current);

			foreach (GameObject v in Current.GetComponent<NodeScript>().neighbors)
			{
				if(closedSet.Contains(v))
				{
					continue;
				}

				float tentative_g_score = g_score[Current] + Vector3.Distance(Current.transform.position,v.transform.position);

				if(openSet.Contains(Current) == false || tentative_g_score < g_score[v])
				{
					cameFrom[v] = Current;
					g_score[v] = tentative_g_score;
					f_score[v] = g_score[v];
					if(openSet.Contains(v) == false)
					{
						openSet.Add(v);
					}
				}
			}
		}
		return null;
	}

	// A Star Pathfinding according to Wikipedia.
	public static List<Vector2> AStar(GameObject[] Graph, Vector2 StartPos, Vector2 EndPos)
	{
		// Can Make optimizations here.
		// Dont run into Function and return null immediatly
		// If start/end pos cannot be found in grid Data

		// Open and Closed list
		List<Vector2> closedSet = new List<Vector2>();
		List<Vector2> openSet = new List<Vector2>();

		// Add original Position
		openSet.Add (StartPos);

		Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2> ();
		Dictionary<Vector2, float> g_score = new Dictionary<Vector2, float> ();
		Dictionary<Vector2, float> f_score = new Dictionary<Vector2, float> ();

		// Cost from start is zero
		g_score [StartPos] = 0.0f;

		// Estimate cost from goal through y
		f_score [StartPos] = g_score [StartPos] + HeuristicCostEstimate (StartPos, EndPos);

		// Whilel openSet is not empty
		while(openSet.Count > 0)
		{
			// Get node that has the lowest score
			Vector2 current = FindLowestValue(openSet,f_score);

			// If the current position is at the end position
			if(current == EndPos)
			{
				// Remake path, Talk about this later
				return ReconstructPath(cameFrom,current);
			}

			// Remove Current from openSet
			openSet.Remove(current);

			// Add Current to closedSet
			closedSet.Add (current);

			// Neighbours Values to check Left/Right/Top/Bottom
			Vector2[] neighbours = 	{current + new Vector2(1,0),
									current + new Vector2(0,1),
									current + new Vector2(-1,0),
									current + new Vector2(0,-1)
			};

			// Loop through all Neighbours
			foreach(Vector2 tempVec in neighbours)
			{
//				// Need to check if the surrounding is walkable.
//				// Fig this out later.
//				// This ontop is weird. 
//				if(GridData[tempVec.x][tempVec.y].GetComponent<NodeScript>().enabled == false)
//				{
//					continue;
//				}
				// This below is okay
				if(closedSet.Contains(tempVec))
				{
					continue;
				}

				// Add const, cause no diagonal. If diagonal, change this.
				float totalGScore = g_score[current] + 1.0f;

				// Check if the score is Lowest.
				if(openSet.Contains(tempVec) == false || totalGScore <= g_score[tempVec])
				{
					cameFrom[tempVec] 	= current;
					g_score	[tempVec] 	= totalGScore;
					Vector2 tempVecHold = tempVec;
					f_score	[tempVec] 	= g_score[tempVec] + HeuristicCostEstimate(tempVecHold,EndPos);
					if(openSet.Contains(tempVec) == false)
					{
						openSet.Add(tempVec);
					}
				}
			}
		}

		return null;
	}
}
