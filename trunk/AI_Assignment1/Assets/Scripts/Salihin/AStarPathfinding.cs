using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour {

	public List<TileScript> OpenList = new List<TileScript>();
	public List<TileScript> CloseList = new List<TileScript>();
	public List<TileScript> PathList = new List<TileScript>();

	private Vector3 StartPos, DestPos, CurrPos;
	private GameObject LvlGen;

	private TileScript StartTile;
	private TileScript DestTile;
	private TileScript CurrTile;

	private List<TileScript> AdjacentTile = new List<TileScript>();

	int DestCount = 0;

	void Start()
	{
	}

	// Initialise
	public AStarPathfinding(GameObject shadow, GameObject player, GameObject lvl)
	{
		StartPos = shadow.transform.position; // ai position
		//Debug.Log("StartPos: " + StartPos);
		DestPos = player.transform.position; // Final position
		//Debug.Log("DestPos: " + DestPos);
		CurrPos = shadow.transform.position; // ai position for starting our A*
		//Debug.Log("CurrPos: " + CurrPos);
		LvlGen = lvl;

		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		// convert positions into tiles
		StartTile = new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x);
		DestTile = new TileScript(LvlMapData[(int)DestPos.z][(int)DestPos.x], (int)DestPos.z, (int)DestPos.x);
		CurrTile = new TileScript(LvlMapData[(int)CurrPos.z][(int)CurrPos.x], (int)CurrPos.z, (int)CurrPos.x);
	}

	// This function takes in the shadow and pacman
	public void Init(GameObject shadow, GameObject player)
	{
		StartPos = shadow.transform.position;
//		Debug.Log("Init StartPos: " + StartPos);
		DestPos = player.transform.position;
//		Debug.Log("Init DestPos: " + DestPos);
		CurrPos = StartPos;
//		Debug.Log("Init CurrPos: " + CurrPos);

		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;
		
		StartTile = new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x);
		DestTile = new TileScript(LvlMapData[(int)DestPos.z][(int)DestPos.x], (int)DestPos.z, (int)DestPos.x);
		CurrTile = new TileScript(LvlMapData[(int)CurrPos.z][(int)CurrPos.x], (int)CurrPos.z, (int)CurrPos.x);

		// just in case
		OpenList.Clear();
		CloseList.Clear();
		AdjacentTile.Clear();
	}

	public void InitAStar()
	{
		// to wait for levelgenerator to initialise first
		if(LvlGen != null)
		{
			var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

			//OpenList.Add (new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x));

			// 1. We add in our first tile to the open list
			OpenList.Add(new TileScript(StartTile.id, StartTile.row, StartTile.column));
//			Debug.Log("InitAStar StartPos: " + StartPos);
//			Debug.Log("Add to OpenList");
//			Debug.Log("InitAStar OpenList Count: " + OpenList.Count);
			//OpenList[OpenList.Count - 1].CalcF(StartPos, DestPos, CurrPos);
			//CloseList.Add(new TileScript(LvlMapData[(int)StartPos.z][(int)StartPos.x], (int)StartPos.z, (int)StartPos.x));

			//Debug.Log("Shadow Spawn: " + LvlMapData[(int)CurrPos.z][(int)CurrPos.x]);
			//Debug.Log("Shadow Spawn: " + LvlMapData.Count);
			
			//AddToOpenList();
			//CalcFInOpenList(StartPos, DestPos);
		}
	}

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
//				Debug.Log("path is Found~!");
				break;	
			}
			
			//5. Add adjacent nodes to open list
			AddAdjacentNodesToOpenList(CurrTile);

	//		Debug.Log("openList count just before loop ends: " + OpenList.Count);

		} while(OpenList.Count > 0 );

//		for(int i = 0; i < CloseList.Count; i++)
//		{
//			Debug.Log("Closed List variables: Row: " + CloseList[i].row + " Column: " + CloseList[i].column + " F: " + CloseList[i].F + " G: " + CloseList[i].G + " H: "  + CloseList[i].H);
//		}

		DestCount = (int)CloseList[CloseList.Count - 1].G;
		//CurrTile = DestTile;

		do
		{
			AddAdjacentNodesToPathList(DestTile);
			DestTile = PathList[PathList.Count - 1];
		}while(DestCount > 0);

		if(DestCount == 0)
		{
//			Debug.Log("Reverse path found");
//			for(int i = PathList.Count - 1; i > -1; i--)
//			{
//				Debug.Log("Path List variables: Row: " + PathList[i].row + " Column: " + PathList[i].column + " F: " + PathList[i].F + " G: " + PathList[i].G + " H: "  + PathList[i].H);
//			}
		}











//		do
//		{
////			Debug.Log("CurrPos: " + CurrPos);
//			AddToCloseList();
//
//			if((int)CurrPos.x == (int)DestTile.Position.x && (int)CurrPos.z == (int)DestTile.Position.z)
//			{
//				//path found
////				Debug.Log("PATH FOUND");
//				break;
//			}
//
//			AddToAdjacentTile(CurrPos, CurrTile.row, CurrTile.column);
////			Debug.Log("Iteration CurrPos: " + CurrPos);
//
//			bool closed = false;
//			for(int i = 0; i < AdjacentTile.Count; i++)
//			{
//				for(int j = 0; j < CloseList.Count; j++)
//				{
//					//if adjacent tile is in close list, continue
//					if(AdjacentTile[i].row == CloseList[j].row && AdjacentTile[i].column == CloseList[j].column)
//					{
//						closed = true;
//						break;
//					}
//				}
//
//				if(!closed)
//				{
////					Debug.Log("AdjacentTile index: " + i);
//
//					OpenList.Add(AdjacentTile[i]);
////					Debug.Log("Added AdjacentTile into OpenList");
//					OpenList[OpenList.Count - 1].CalcF(StartPos, DestPos, CurrPos);
//				}
//				closed = false;
//			}
//			AdjacentTile.Clear();
////			Debug.Log("Iteration OpenList Count: " + OpenList.Count);
//		}while(OpenList.Count > 0);
//		Debug.Log("EXIT ITERATION");

		//int DestCount = (int)CloseList[CloseList.Count - 1].G;


//		do
//		{
//			AddToAdjacentTile(DestPos);
//
//			for(int i = 0; i < AdjacentTile.Count; i++)
//			{
//				for(int j = 0; j < CloseList.Count; j++)
//				{
//					Debug.Log("CloseList Pos0: " + CloseList[j].Position + ", F: " + CloseList[j].F + ", G: " + CloseList[j].G + ", H: " + CloseList[j].H);
//					//if adjacent tile is in close list, continue
//
//					if(AdjacentTile[i].row == CloseList[j].row && AdjacentTile[i].column == CloseList[j].column)
//					{
//						AdjacentTile[i] = CloseList[j];
//
////						if(DestCount - 1 == (int)AdjacentTile[i].G)
////						{
////							Debug.Log("DestPos: " + DestPos);
////							Debug.Log("AdjacentTile: " + AdjacentTile[i].row + ", " + AdjacentTile[i].column);
////							Debug.Log("AdjacentTile F: " + AdjacentTile[i].F + ", G: " + AdjacentTile[i].G + ", H: " + AdjacentTile[i].H);
////							Debug.Log("CloseList Pos: " + CloseList[j].Position + ", F: " + CloseList[j].F + ", G: " + CloseList[j].G + ", H: " + CloseList[j].H);
//							PathList.Add(CloseList[j]);
//							Debug.Log("CloseList Pos1: " + CloseList[j].Position + ", F: " + CloseList[j].F + ", G: " + CloseList[j].G + ", H: " + CloseList[j].H);
////							Debug.Log("PathList Pos: " + PathList[PathList.Count - 1].Position);
//							DestPos = PathList[PathList.Count - 1].Position;
//							DestCount--;
////							Debug.Log("DestCount: " + DestCount);
//						//}
//					}
//				}
//			}
//			AdjacentTile.Clear();
//		}while(DestCount > 0);
////		Debug.Log("REVERSE PATH FOUND");
	}

	void FindTileWithLowestF(ref TileScript currentTile)
	{
		float lowestF = 10000000.0f;

		for(int i = 0; i < OpenList.Count; i++)
		{
			if(OpenList[i].F < lowestF)
			{
				lowestF = OpenList[i].F;
				currentTile = OpenList[i];
//				currentTile.row = OpenList[i].row;
//				currentTile.column = OpenList[i].column;
//				Debug.Log("Obtained the tile with the lowest F!");
			}
		}
	}

	void AddAdjacentNodesToOpenList(TileScript CurrTile)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		int curr_z = CurrTile.row;
		int curr_x = CurrTile.column;

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
			//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
			//			Debug.Log("Added Left Adjacent");
			//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
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
			//AdjacentTile.Add(new TileScript(LvlMapData[curr_z][curr_x + 1], curr_z, curr_x + 1));
			//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
			//			Debug.Log("Added Right Adjacent");
			//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
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
			//AdjacentTile.Add(new TileScript(LvlMapData[curr_z - 1][curr_x], curr_z - 1, curr_x));
			//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
			//			Debug.Log("Added Up Adjacent");
			//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
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
			//AdjacentTile.Add(new TileScript(LvlMapData[curr_z + 1][curr_x], curr_z + 1, curr_x));
			//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
			//			Debug.Log("Added Down Adjacent");
			//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
		}

//		Debug.Log("openList count inside adjacent tiles: " + OpenList.Count);
	}

	void AddAdjacentNodesToPathList(TileScript CurrTile)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;
		
		int curr_z = CurrTile.row;
		int curr_x = CurrTile.column;
		
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
		
		//		Debug.Log("openList count inside adjacent tiles: " + OpenList.Count);
	}

	private void AddToAdjacentTile(Vector3 CurrPos, int z, int x)
	{		
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;
//		int curr_z = (int)CurrPos.z;
//		int curr_x = (int)CurrPos.x;

		int curr_z = z;
		int curr_x = x;

		//Check Left
		if(ValidateTile(curr_z, curr_x - 1, 1) && ValidateTile(curr_z, curr_x - 1, 2) && ValidateTile(curr_z, curr_x - 1, 3))
		{
			AdjacentTile.Add(new TileScript(LvlMapData[curr_z][curr_x - 1], curr_z, curr_x - 1));
//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
//			Debug.Log("Added Left Adjacent");
//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
		}

		//Check Right
		if(ValidateTile(curr_z, curr_x + 1, 1) && ValidateTile(curr_z, curr_x + 1, 2) && ValidateTile(curr_z, curr_x + 1, 3))
		{
			AdjacentTile.Add(new TileScript(LvlMapData[curr_z][curr_x + 1], curr_z, curr_x + 1));
//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
//			Debug.Log("Added Right Adjacent");
//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
		}

		//Check Up
		if(ValidateTile(curr_z - 1, curr_x, 1) && ValidateTile(curr_z - 1, curr_x, 2) && ValidateTile(curr_z - 1, curr_x, 3))
		{
			AdjacentTile.Add(new TileScript(LvlMapData[curr_z - 1][curr_x], curr_z - 1, curr_x));
//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
//			Debug.Log("Added Up Adjacent");
//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
		}

		//Check Down
		if(ValidateTile(curr_z + 1, curr_x, 1) && ValidateTile(curr_z + 1, curr_x, 2) && ValidateTile(curr_z + 1, curr_x, 3))
		{
			AdjacentTile.Add(new TileScript(LvlMapData[curr_z + 1][curr_x], curr_z + 1, curr_x));
//			Debug.Log("AdjacentTile: " + AdjacentTile[AdjacentTile.Count - 1].row + ", " + AdjacentTile[AdjacentTile.Count - 1].column);
//			Debug.Log("Added Down Adjacent");
//			Debug.Log("Adjacent Count: " + AdjacentTile.Count);
		}
	}

	private bool ValidateTile(int r, int c, int check)
	{
		var LvlMapData = LvlGen.GetComponent<LevelGenerator>().mapData;

		if(LvlMapData[r][c] != check)
		{
			return true;
		}
		return false;
	}

//	private void AddToCloseList()
//	{
//		int min_id = FindMinF();
//
//		CloseList.Add(OpenList[min_id]);
////		Debug.Log("CloseList Pos: " + CloseList[CloseList.Count - 1].Position + ", F: " + CloseList[CloseList.Count - 1].F + ", G: " + CloseList[CloseList.Count - 1].G + ", H: " + CloseList[CloseList.Count - 1].H);
////		Debug.Log("CloseList: " + CloseList[CloseList.Count - 1].row + ", " + CloseList[CloseList.Count - 1].column);
////		Debug.Log("Add to CloseList");
//		OpenList.Remove(OpenList[min_id]);
////		Debug.Log("Removed from OpenList");
//	}

//	private int FindMinF()
//	{
//		if(OpenList.Count > 0)
//		{
//			int min = (int)OpenList[0].F;
//			int min_id = 0;
//			CurrPos = OpenList[min_id].Position;
////			Debug.Log("CurrPosition1: " + CurrPos);
//			
//			for(int i = 0; i < OpenList.Count; i++)
//			{
//				if((int)OpenList[i].F < min)
//				{
//					min = (int)OpenList[i].F;
//					min_id = i;
//					CurrPos = OpenList[min_id].Position;
//					CurrTile = OpenList[min_id];
////					Debug.Log("CurrPosition1: " + CurrPos);
//				}
//			}
//			return min_id;
//		}
//		return -1;
//	}
}
