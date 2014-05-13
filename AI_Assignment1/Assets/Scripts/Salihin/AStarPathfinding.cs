using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour {
	
	public List<TileScript> OpenList = new List<TileScript>();							//list to add adjacent tiles depending on position
	public List<TileScript> CloseList = new List<TileScript>();							//list to add tile from OpenList depending on smallest F value
	public List<TileScript> PathList = new List<TileScript>();							//list for AI to traverse through for the path ti follow
	
	private Vector3 StartPos, DestPos, CurrPos;											//store positions necessary in processing A* Pathfinding
	private GameObject LvlGen;															//access data from LevelGenerator to assist in A* Pathfinding

	//3 important tiles to assist in A* Pathfinding
	private TileScript StartTile;
	private TileScript DestTile;
	private TileScript CurrTile;

	//component used for finding the reserve path
	private int DestCount = 0;

	//get data from AI and Player and manipulates them for use in A* Pathfinding
	public AStarPathfinding(GameObject shadow, GameObject player, GameObject lvl)
	{
		StartPos = shadow.transform.position; 					//AI position
		DestPos = player.transform.position; 					//Final position
		CurrPos = shadow.transform.position; 					//AI position for starting our A*
		LvlGen = lvl;

		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		//convert positions into tiles
		StartTile = new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x);
		DestTile = new TileScript(LvlMapData[(int)DestPos.z][(int)DestPos.x], (int)DestPos.z, (int)DestPos.x);
		CurrTile = new TileScript(LvlMapData[(int)CurrPos.z][(int)CurrPos.x], (int)CurrPos.z, (int)CurrPos.x);
	}

	//function takes in the shadow and pacman
	public void Init(GameObject shadow, GameObject player)
	{
		StartPos = shadow.transform.position;
		DestPos = player.transform.position;
		CurrPos = StartPos;

		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;
		
		StartTile = new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x);
		DestTile = new TileScript(LvlMapData[(int)DestPos.z][(int)DestPos.x], (int)DestPos.z, (int)DestPos.x);
		CurrTile = new TileScript(LvlMapData[(int)CurrPos.z][(int)CurrPos.x], (int)CurrPos.z, (int)CurrPos.x);

		// just in case
		OpenList.Clear();
		CloseList.Clear();
	}

	//function to add current tile AI is in to the OpenList
	public void InitAStar()
	{
		// to wait for levelgenerator to initialise first
		if(LvlGen != null)
		{
			var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

			// 1. We add in our first tile to the open list
			OpenList.Add(new TileScript(StartTile.id, StartTile.row, StartTile.column));
		}
	}

	//function to perform the iteration to add tiles into OpenList and CloseList and to plot path from AI to Pacman
	public void Iteration()
	{
		do
		{
			//2 . get the tile with the lowest F value
			FindTileWithLowestF(ref CurrTile);
			
			//3. change current node to closed list
			for(int i = 0; i < OpenList.Count; i++)
			{
					if(OpenList[i].row == CurrTile.row && OpenList[i].column == CurrTile.column )
					{
						CloseList.Add(OpenList[i]);
						break;
					}
			}
			
			// Remove the tile from the open list
			foreach(TileScript tile in OpenList)
			{
				if(tile.row == CurrTile.row &&tile.column == CurrTile.column )
				{
					OpenList.Remove(tile);
					break;
				}
			}
			
			//4. check if we have reached the path
			if(CurrTile.row == DestTile.row && CurrTile.column == DestTile.column)
			{
				break;	
			}
			
			//5. Add adjacent nodes to open list
			AddAdjacentNodesToOpenList(CurrTile);
		} while(OpenList.Count > 0 );

		//start to plot the reverse path
		DestCount = (int)CloseList[CloseList.Count - 1].G;
		PathList.Add (DestTile);

		//iteration to plot reverse path
		do
		{
			AddAdjacentNodesToPathList(DestTile);
			DestTile = PathList[PathList.Count - 1];
		}while(DestCount > 0);
	}

	//function to find the lowest F value of the tiles in the OpenList
	private void FindTileWithLowestF(ref TileScript currentTile)
	{
		float lowestF = 10000000.0f;

		for(int i = 0; i < OpenList.Count; i++)
		{
			if(OpenList[i].F < lowestF)
			{
				lowestF = OpenList[i].F;
				currentTile = OpenList[i];
			}
		}
	}

	//function to add adjacent tiles to the current position into the OpenList for the path from AI to Pacman
	private void AddAdjacentNodesToOpenList(TileScript CurrTile)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		int curr_z = CurrTile.row;
		int curr_x = CurrTile.column;

		//check adjacent tiles and ensure they are not an Environment Tile
		//Check Left
		if(ValidateTile(curr_z, curr_x - 1, 1) && ValidateTile(curr_z, curr_x - 1, 2) && ValidateTile(curr_z, curr_x - 1, 3))
		{
			// adjacent tile in the closed list do not add.
			bool inopen = false;
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row && CloseList[i].column == CurrTile.column - 1)
				{
					inopen = true;
					break;
				}
			}

			if(!inopen)
			{
				TileScript adjacentTile = new TileScript(LvlMapData[curr_z][curr_x - 1], curr_z, curr_x - 1);

				adjacentTile.G = CurrTile.G + 1;
				adjacentTile.CalcH(DestTile.row, DestTile.column, curr_z, curr_x - 1);
				adjacentTile.F = adjacentTile.G + adjacentTile.H;

				OpenList.Add(adjacentTile);
			}
			inopen = false;
		}
		
		//Check Right
		if(ValidateTile(curr_z, curr_x + 1, 1) && ValidateTile(curr_z, curr_x + 1, 2) && ValidateTile(curr_z, curr_x + 1, 3))
		{
			bool inopen = false;
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row && CloseList[i].column == CurrTile.column + 1)
				{
					inopen = true;
					break;
				}
			}

			if(!inopen)
			{
				TileScript adjacentTile = new TileScript(LvlMapData[curr_z][curr_x + 1], curr_z, curr_x + 1);
				
				adjacentTile.G = CurrTile.G + 1;
				adjacentTile.CalcH(DestTile.row, DestTile.column, curr_z, curr_x + 1);
				adjacentTile.F = adjacentTile.G + adjacentTile.H;
				
				OpenList.Add(adjacentTile);
			}
			inopen = false;
		}
		
		//Check Up
		if(ValidateTile(curr_z - 1, curr_x, 1) && ValidateTile(curr_z - 1, curr_x, 2) && ValidateTile(curr_z - 1, curr_x, 3))
		{
			bool inopen = false;
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row - 1 && CloseList[i].column == CurrTile.column)
				{
					inopen = true;
					break;
				}
			}

			if(!inopen)
			{
				TileScript adjacentTile = new TileScript(LvlMapData[curr_z - 1][curr_x], curr_z - 1, curr_x);
				
				adjacentTile.G = CurrTile.G + 1;
				adjacentTile.CalcH(DestTile.row, DestTile.column, curr_z - 1, curr_x);
				adjacentTile.F = adjacentTile.G + adjacentTile.H;
				
				OpenList.Add(adjacentTile);
			}
			inopen = false;
		}
		
		//Check Down
		if(ValidateTile(curr_z + 1, curr_x, 1) && ValidateTile(curr_z + 1, curr_x, 2) && ValidateTile(curr_z + 1, curr_x, 3))
		{
			bool inopen = false;
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row + 1 && CloseList[i].column == CurrTile.column)
				{
					inopen = true;
					break;
				}
			}

			if(!inopen)
			{
				TileScript adjacentTile = new TileScript(LvlMapData[curr_z + 1][curr_x], curr_z + 1, curr_x);
				
				adjacentTile.G = CurrTile.G + 1;
				adjacentTile.CalcH(DestTile.row, DestTile.column, curr_z + 1, curr_x);
				adjacentTile.F = adjacentTile.G + adjacentTile.H;
				
				OpenList.Add(adjacentTile);
			}
			inopen = false;
		}
	}

	//function to add adjacent tiles to the current position into the OpenList for the path from Pacman to AI
	private void AddAdjacentNodesToPathList(TileScript CurrTile)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;
		
		int curr_z = CurrTile.row;
		int curr_x = CurrTile.column;

		//check adjacent tiles and ensure they are not an Environment Tile
		//Check Left
		if(ValidateTile(curr_z, curr_x - 1, 1) && ValidateTile(curr_z, curr_x - 1, 2) && ValidateTile(curr_z, curr_x - 1, 3))
		{
			// adjacent tile in the closed list do not add.
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row && CloseList[i].column == CurrTile.column - 1)
				{
					if(DestCount - 1 == CloseList[i].G)
					{
						PathList.Add (CloseList[i]);
						CurrTile = PathList[PathList.Count - 1];
						DestCount--;
					}
				}
			}
		}
		
		//Check Right
		if(ValidateTile(curr_z, curr_x + 1, 1) && ValidateTile(curr_z, curr_x + 1, 2) && ValidateTile(curr_z, curr_x + 1, 3))
		{
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row && CloseList[i].column == CurrTile.column + 1)
				{
					if(DestCount - 1 == CloseList[i].G)
					{
						PathList.Add (CloseList[i]);
						CurrTile = PathList[PathList.Count - 1];
						DestCount--;
					}
				}
			}
		}
		
		//Check Up
		if(ValidateTile(curr_z - 1, curr_x, 1) && ValidateTile(curr_z - 1, curr_x, 2) && ValidateTile(curr_z - 1, curr_x, 3))
		{
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row - 1 && CloseList[i].column == CurrTile.column)
				{
					if(DestCount - 1 == CloseList[i].G)
					{
						PathList.Add (CloseList[i]);
						CurrTile = PathList[PathList.Count - 1];
						DestCount--;
					}
				}
			}
		}
		
		//Check Down
		if(ValidateTile(curr_z + 1, curr_x, 1) && ValidateTile(curr_z + 1, curr_x, 2) && ValidateTile(curr_z + 1, curr_x, 3))
		{
			for(int i = 0; i < CloseList.Count; i++)
			{
				if(CloseList[i].row == CurrTile.row + 1 && CloseList[i].column == CurrTile.column)
				{
					if(DestCount - 1 == CloseList[i].G)
					{
						PathList.Add (CloseList[i]);
						CurrTile = PathList[PathList.Count - 1];
						DestCount--;
					}
				}
			}
		}
	}

	//function to check if current tile to be checked is valid in the A* Pathfinding process
	private bool ValidateTile(int r, int c, int check)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		if(LvlMapData[r][c] != check)
		{
			return true;
		}
		return false;
	}
}
