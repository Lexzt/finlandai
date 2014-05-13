using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarWaypoint : MonoBehaviour {

	enum Direction{
		TOP = 0,
		BOTTOM,
		LEFT,
		RIGHT
	};

	public Object referenceAI; // put in the AI prefab using this reference	

	public GameObject ai;
	public GameObject player; // pointer reference to the player
	
	public Vector3 nextPositionVector = Vector3.zero;

	public int nextElement = 0;
	
	List<List<NodeContainer>> mapReference = new List<List<NodeContainer>> ();
	

	GameObject levelReference; // Our level reference back to the level's nodes!

	public float speed = 3.0f; // Movement Speed value of the 

	private float nextActionTime = 0.0f;
	private float timeToCalculateNextPrediction = 1.0f;

	// Current position of the Player
	public int playerPositionX = 0;
	public int playerPositionY = 0;

	// Current position of the AI
	public int aiPositionX = 0;
	public int aiPositionY = 0;

	// Our final destination to get to
	public int finalPositionX = 0;
	public int finalPositionY = 0;

	// List of Positions the AI is supposed to go
	List<Vector3> finishedPath = new List<Vector3>();

	// Determines if the tile being 
	bool IsValidWalkableTile(int number)
	{
		// 0, 4, 5, 6 are the numbers that the AI can walk on
		int[] validTiles = {0, 4, 5, 6, 7}; 

		for(int i = 0; i < validTiles.Length; i++)
		{
			if(validTiles[i] == number)
			{
				return true;
			}
		}

		return false;
	}

	// Use this for initialization
	void Start () {

		//Spawn the enemy
		ai = Instantiate (referenceAI, nextPositionVector, Quaternion.identity) as GameObject;

		ai.transform.position = GameObject.FindGameObjectWithTag ("EnemySpawn").transform.position;

		levelReference = GameObject.Find ("LevelGenerator");

		for(int i = 0; i < levelReference.GetComponent<LevelGenerator>().mapData.Count; i++)
		{
			List<NodeContainer> temp = new List<NodeContainer>();

			for(int j = 0; j < levelReference.GetComponent<LevelGenerator>().mapData[0].Count; j++)
			{
				NodeContainer miniTemp = new NodeContainer();

				if(IsValidWalkableTile(levelReference.GetComponent<LevelGenerator>().mapData[i][j]))
				{
					miniTemp.walkable = AbleToWalkOn.WALKABLE;
				}
				else
				{
					miniTemp.walkable = AbleToWalkOn.UNWALKABLE;
				}

				temp.Add(miniTemp);
			}

			mapReference.Add(temp);
		}

		CalculatePath();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Calculate a new path every interval
		if (Time.time > nextActionTime ) 
		{
			nextActionTime += timeToCalculateNextPrediction;
			CalculatePath();
		}

		Move ();
	}

	void ValidateNodes(int currentPositionY, int currentPositionX, int offsetY, int offsetX)
	{
		if(currentPositionX + offsetX > 0
		   && currentPositionX + offsetX < mapReference.Count - 1
		   && currentPositionY + offsetY > 0
		   && currentPositionY + offsetY < mapReference.Count - 1)
		{
			if(mapReference[currentPositionY + offsetY][currentPositionX + offsetX].walkable == AbleToWalkOn.WALKABLE
			   && mapReference[currentPositionY + offsetY][currentPositionX + offsetX].nodeType != OpenClosedCheck.CLOSED)
			{
				mapReference[currentPositionY + offsetY][currentPositionX + offsetX].nodeType = OpenClosedCheck.OPEN;
				mapReference[currentPositionY + offsetY][currentPositionX + offsetX].G = mapReference[currentPositionY][currentPositionX].G + 1;
				mapReference[currentPositionY + offsetY][currentPositionX + offsetX].H = System.Math.Abs((currentPositionX + offsetX) - finalPositionX) + System.Math.Abs((currentPositionY + offsetY) - finalPositionY);
				mapReference[currentPositionY + offsetY][currentPositionX + offsetX].F = mapReference[currentPositionY + offsetY][currentPositionX + offsetX].G + mapReference[currentPositionY + offsetY][currentPositionX + offsetX].H;
			}
		}
	}


	void CalculatePath()
	{
		// reset our variables.
		for(int i = 0; i < mapReference.Count; i++)
		{
			for(int j = 0; j < mapReference[0].Count; j++)
			{
				mapReference[i][j].F = mapReference[i][j].G = mapReference[i][j].H = 0;
				mapReference[i][j].nodeType = OpenClosedCheck.UNDEFINED;
			}
		}

		finishedPath.Clear ();

		finalPositionX = playerPositionX;
		finalPositionY = playerPositionY;
		
		aiPositionX = (int)ai.transform.position.x;
		aiPositionY = (int)ai.transform.position.z;

		System.Random randomPosition = new System.Random ();
		int randomY = 0;
		int randomX = 0;

		do{

			randomY = randomPosition.Next(0, mapReference.Count - 1);
			randomX = randomPosition.Next(0, mapReference[0].Count - 1);

			if(mapReference[randomY][randomX].walkable == AbleToWalkOn.WALKABLE)
			{
				break;
			}

		}while(true);

		finalPositionY = randomY;
		finalPositionX = randomX;


		// Tile with currently Lowest F score
		int tileWithLowestFScoreX = 0;
		int tileWithLowestFScoreY = 0;


		// Now Calculate the path here
		//take for example the player's position
		int currentPositionX = aiPositionX;
		int currentPositionY = aiPositionY;

		bool noPath = false;

		//1. add AI's position to closed list!
		mapReference[currentPositionY][currentPositionX].nodeType  = OpenClosedCheck.OPEN;

		do{
			int lowestF = 10000000;

			// Get the square with the lowest F score!
			for(int i = 0; i < mapReference.Count; i++)
			{
				for(int j = 0; j < mapReference[0].Count; j++)
				{
					if(mapReference[i][j].nodeType == OpenClosedCheck.OPEN)
					{
						if(mapReference[i][j].F < lowestF)
						{
							lowestF = mapReference[i][j].F;
							currentPositionX = j;
							currentPositionY = i;
						}
					}
				}
			}

			mapReference[currentPositionY][currentPositionX].nodeType = OpenClosedCheck.CLOSED;

			// we've found the final destination!
			if(currentPositionX == finalPositionX && currentPositionY == finalPositionY)
			{
				break;
			}
			// get square with lowest F score
			// add adjacent tiles to the AI as open list tiles if its valid (walkable tiles)
			ValidateNodes (currentPositionY, currentPositionX, 1, 0);
			ValidateNodes (currentPositionY, currentPositionX, -1, 0);
			ValidateNodes (currentPositionY, currentPositionX, 0, 1);
			ValidateNodes (currentPositionY, currentPositionX, 0, -1);

			lowestF = 10000000;

			noPath = true;

			// Check to see if there are any more items that are open. if there is nothing that is open we say THERE IS NO PATH SORRY!
			for(int i = 0; i < mapReference.Count; i ++)
			{
				for(int j = 0; j < mapReference[0].Count; j ++)
				{
					if(mapReference[i][j].nodeType == OpenClosedCheck.OPEN)
					{
						noPath = false;
					}
				}
			}

			if(noPath)
			{
				// Debug.Log("We conclude that there is no path");
			}


		} while(!noPath);

		// Store the path to walk to inside a list of vectors.
		int destinationGCost = mapReference [finalPositionY] [finalPositionX].G;

		int positionGX = finalPositionX;
		int positionGY = finalPositionY;

		finishedPath.Add (new Vector3((positionGX * 1.0f) + 0.5f, 0.5f, (positionGY * 1.0f) + 0.5f));

		Direction direction = Direction.BOTTOM;

		do{
			if(positionGY + 1 < mapReference.Count)
			{
				if(mapReference[positionGY + 1][positionGX].G == destinationGCost - 1
				   && mapReference[positionGY + 1][positionGX].nodeType == OpenClosedCheck.CLOSED)
				{
					finishedPath.Add (new Vector3((positionGX * 1.0f) + 0.5f, 0.5f, ((positionGY + 1) * 1.0f) + 0.5f));
					direction = Direction.BOTTOM;
				}
			}

			if(positionGY - 1 > 0)
			{
				if(mapReference[positionGY - 1][positionGX].G == destinationGCost - 1
				   && mapReference[positionGY - 1][positionGX].nodeType == OpenClosedCheck.CLOSED)
				{
					finishedPath.Add (new Vector3((positionGX * 1.0f) + 0.5f, 0.5f, ((positionGY - 1) * 1.0f) + 0.5f));
					direction = Direction.TOP;
				}
			}

			if(positionGX + 1 < mapReference.Count)
			{
				if(mapReference[positionGY][positionGX + 1].G == destinationGCost - 1
				   && mapReference[positionGY][positionGX + 1].nodeType == OpenClosedCheck.CLOSED)
				{
					finishedPath.Add (new Vector3(((positionGX + 1) * 1.0f) + 0.5f, 0.5f, (positionGY  * 1.0f) + 0.5f));
					direction = Direction.LEFT;
				}
			}

			if(positionGX - 1 > 0)
			{
				if(mapReference[positionGY][positionGX - 1].G == destinationGCost - 1
				   && mapReference[positionGY][positionGX - 1].nodeType == OpenClosedCheck.CLOSED)
				{
					finishedPath.Add (new Vector3(((positionGX - 1) * 1.0f) + 0.5f, 0.5f, (positionGY  * 1.0f) + 0.5f));
					direction = Direction.RIGHT;
				}
			}

			if(direction == Direction.BOTTOM)
			{
				positionGY++;
			}else if(direction == Direction.TOP)
			{
				positionGY--;
			}else if(direction == Direction.LEFT)
			{
				positionGX++;
			}else if(direction == Direction.RIGHT)
			{
				positionGX--;
			}
			
			destinationGCost --;

		} while(destinationGCost > 0);
		
		nextElement = finishedPath.Count - 1;
	}
	
	void Move()
	{
		if(finishedPath.Count > 0)
		{
			if(ai.transform.position == finishedPath[nextElement])
			{
				if(nextElement > 0)
				{
					nextElement--;
				}
			}
		

			ai.transform.position = Vector3.MoveTowards(ai.transform.position, finishedPath[nextElement], speed * Time.deltaTime);
		}
	}
}
