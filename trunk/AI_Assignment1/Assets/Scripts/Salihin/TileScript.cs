using UnityEngine;
using System.Collections;

public class TileScript
{
	public int id;
	public int row;
	public int column;

	//public Vector3 Position;

	public float F = 0.0f;					//F = G + H
	public float G = 0.0f;					//movement cost from start point to current point
	public float H = 0.0f;					//estimated cost from current square to destination point

	public TileScript()
	{
		F = H = G = id = row = column = 0;
	//	Position = Vector3.zero;
	}

	public TileScript(int tile_id, int r, int c)
	{
		id = tile_id;
		row = r;
		column = c;
		//Position = new Vector3(c, 0.0f, r);
	}

	// Use this for initialization
	void Awake ()
	{
		F = H = G = id = row = column = 0;
	//	Position = Vector3.zero;
	}



	//pass in start point, current square to calculate G
//	float CalcG(Vector3 StartPoint, Vector3 CurrPoint)
//	{
//		G = Mathf.Abs(((int)CurrPoint.x - (int)StartPoint.x) + ((int)CurrPoint.z - (int)StartPoint.z));
//		return G;
//	}
	
	//pass in destination point, current square to calculate H 
	public float CalcH(int DestPointRow, int DestPointColumn, int CurrPointRow, int CurrPointColumn)
	{
		H = Mathf.Abs((DestPointRow - CurrPointRow) + (DestPointColumn - CurrPointColumn));
		return H;
	}
	
	//pass in start point, destination point and current square to calculate F
//	public void CalcF(Vector3 StartPoint, Vector3 DestPoint, Vector3 CurrPoint)
//	{
//	//	F = (int)CalcG (StartPoint, CurrPoint) + (int)CalcH (DestPoint, CurrPoint);
//	}
}

