using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointAIScript : MonoBehaviour {

	// put in the AI prefab using this reference
	public Object referenceAI;

	public GameObject ai;

	public Vector3 nextPositionVector = Vector3.zero;

	List<Vector3> waypointReference = new List<Vector3>();

	int currentPosition = 0;
	int nextPosition = 0;

	GameObject levelReference;

	bool movingForward = true;

	public float speed = 3.0f;

	void Start () 
	{
		//Spawn the enemy
		ai = Instantiate (referenceAI, nextPositionVector, Quaternion.identity) as GameObject;

		ai.transform.position = GameObject.FindGameObjectWithTag ("EnemySpawn").transform.position;

		int previousWaypointFigure = 0;
		int currentWaypointFigure = 0;

		levelReference = GameObject.Find ("LevelGenerator");
		
		do
		{
			previousWaypointFigure++;

			for(int i = 0; i < levelReference.GetComponent<LevelGenerator>().waypointNodes.Count; i++)
			{
				for(int j = 0; j < levelReference.GetComponent<LevelGenerator>().waypointNodes[0].Count; j++)
				{
					if(levelReference.GetComponent<LevelGenerator>().waypointNodes[i][j] == currentWaypointFigure + 1)
					{
						Vector3 temp = new Vector3((j * 1.0f) + 0.5f, 0.5f, (i * 1.0f) + 0.5f);
						waypointReference.Add (temp);
						currentWaypointFigure++;
						break;
					}
				}
			}

		}while(currentWaypointFigure >= previousWaypointFigure);

		nextPositionVector = waypointReference[0];
	}
	
	void Update () 
	{
		MoveAI ();

	}

	void MoveAI()
	{
		if(ai.transform.position == nextPositionVector)
		{
			if(movingForward)
			{
				for(int i = 0; i < waypointReference.Count; i++)
				{
					if(i == currentPosition)
					{
						nextPositionVector = waypointReference[i];

						currentPosition ++;
						
						if(i == waypointReference.Count - 1)
						{
							movingForward = false;
							currentPosition = waypointReference.Count - 1;
						}
						break;
					}
				}
			}
			else
			{
				for(int i = waypointReference.Count - 1; i > 0; i--)
				{
					if(i == currentPosition)
					{
						nextPositionVector = waypointReference[i];
						currentPosition --;

						if(i == 1)
						{
							movingForward = true;
							currentPosition = 0;
						}
						break;
					}
				}
			}
		}
		ai.transform.position = Vector3.MoveTowards(ai.transform.position, nextPositionVector, speed * Time.deltaTime);
	}
}
