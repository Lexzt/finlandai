using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarAlgorithm {
	private static Stack<GameObject> ReconstructPath(Dictionary<GameObject,GameObject> cameFrom, GameObject current)
	{
		List<GameObject> path = new List<GameObject> ();

		while(cameFrom.ContainsKey(current))
		{
			path.Add(current);
			current = cameFrom[current];
		}
		path.Add (current);

		Stack<GameObject> ReturnStack = new Stack<GameObject> (path);
		return ReturnStack;
	}

	public static float HeuristicCostEstimate(GameObject from, GameObject to)
	{
		float score = 0.0f;
		score = Vector3.Distance (from.transform.position, to.transform.position);
		return score;
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

	public static Stack<GameObject> AStar(GameObject[] Graph, GameObject Source, GameObject Target)
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
			float bestValue = -1.0f;
			GameObject Current = null;
			foreach(GameObject Position in openSet)
			{
				if(bestValue == -1.0f || f_score[Position] < bestValue)
				{
					Current = Position;
					bestValue = f_score[Position];
				}
			}

			if(Current == Target)
			{
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
}
