using UnityEngine;
using System.Collections;

public class TileScript
{
	//components that the tile contains
	public int id;
	public int row;
	public int column;

	public float F = 0.0f;								//F = G + H
	public float G = 0.0f;								//movement cost from start point to current point
	public float H = 0.0f;								//estimated cost from current square to destination point

	//initialize the components
	public TileScript()
	{
		F = H = G = id = row = column = 0;
	}

	//give value to the components 
	public TileScript(int tile_id, int r, int c)
	{
		id = tile_id;
		row = r;
		column = c;
	}

	// Use this for initialization
	void Awake ()
	{
//		F = H = G = id = row = column = 0;
	}
	
	//pass in destination point, current square to calculate H 
	public float CalcH(int DestPointRow, int DestPointColumn, int CurrPointRow, int CurrPointColumn)
	{
		H = Mathf.Abs((DestPointRow - CurrPointRow) + (DestPointColumn - CurrPointColumn));
		return H;
	}
}

