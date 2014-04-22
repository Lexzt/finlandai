using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowScript : MonoBehaviour {

	public int Speed;
	private Vector3 Direction;

	public GameObject Pacman;
	public GameObject Ground;

	public int Trigger_Dist;

	private List<Vector3> TargetPoints = new List<Vector3>();

	// Use this for initialization
	void Start () {
		Direction = Vector3.zero;

		for(int i = 0; i < 2; i++)
		{
			TargetPoints.Add(Vector3.zero);
		}
	}
	
	// Update is called once per frame
	void Update () {

		var TPos = transform.position;
		var PTPos = Pacman.transform.position;
		//var GTPos = Ground.transform.position;

//		if(TPos.y <= GTPos.y + transform.localScale.y * 0.5)
//		{
//			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
//		}

		if(CalcDistance(PTPos) <= Trigger_Dist)
		{
			SetTargetPoints(Pacman.transform.position);

//			if(transform.position.x <= TargetPoints[0].x + 1 || transform.position.x >= TargetPoints[0].x - 1)
//			{
//				SetDirection(TargetPoints[1]);
//				SetTargetPoints(Pacman.transform.position);
//			}
//
//			if(transform.position.z <= TargetPoints[1].z + 1 || transform.position.x >= TargetPoints[1].z - 1)
//			{
//				SetDirection(TargetPoints[0]);
//				SetTargetPoints(Pacman.transform.position);
//			}

			if(CalcDistance(TargetPoints[0]) <= 1)
			{
				SetDirection(TargetPoints[1]);
			}

			if(CalcDistance(TargetPoints[1]) <= 1)
			{
				SetDirection(TargetPoints[0]);
			}

//			Direction = PTPos - TPos;
//			Direction.Normalize();
			Debug.Log(Direction);
			
			transform.position += Direction * Speed * Time.deltaTime;
		}
	}

	void SetTargetPoints(Vector3 Position)
	{
		var TPos = transform.position;

		Vector3 Target0 = new Vector3(Position.x, TPos.y, TPos.z);
		TargetPoints[0] = Target0;

		Vector3 Target1 = new Vector3(TPos.x, TPos.y, Position.z);
		TargetPoints[1] = Target1;
	}

	void SetDirection(Vector3 Target)
	{
		Direction = Target - transform.position;
		Direction.Normalize();
	}

	float CalcDistance(Vector3 Target)
	{
		return ((Target - transform.position).magnitude);
	}
}
