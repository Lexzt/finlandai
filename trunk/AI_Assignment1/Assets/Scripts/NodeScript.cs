using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeScript : MonoBehaviour {

	public List<GameObject> neighbors;
    public GameObject goal;

    void Start()
    {
        Vector3 CurrentPos = transform.position;
		GameObject[] ObjectArray 	= GameObject.FindGameObjectsWithTag ("Bits");
		GameObject[] BigBitsArray 	= GameObject.FindGameObjectsWithTag ("BigBits");
		GameObject[] EmptyArray 	= GameObject.FindGameObjectsWithTag ("Empty");
		GameObject EnemySpawn 		= GameObject.FindGameObjectWithTag 	("EnemySpawn");
		GameObject PlayerSpawn 		= GameObject.FindGameObjectWithTag 	("PlayerSpawn");
		List<GameObject> FinalArray = new List<GameObject>();
		
		foreach (GameObject v in ObjectArray) 
		{
			FinalArray.Add(v);
		}

		foreach (GameObject v in BigBitsArray) 
		{
			FinalArray.Add(v);
		}

		foreach (GameObject v in EmptyArray) 
		{
			FinalArray.Add(v);
		}

		FinalArray.Add(EnemySpawn);
		FinalArray.Add(PlayerSpawn);
		

		foreach (GameObject v in FinalArray)
        {
            // Check Left
            if (v.transform.position.x == CurrentPos.x + 1 &&
                v.transform.position.z == CurrentPos.z)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Right
            if (v.transform.position.x == CurrentPos.x - 1 &&
                v.transform.position.z == CurrentPos.z)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Up
            if (v.transform.position.z == CurrentPos.z + 1 &&
                v.transform.position.x == CurrentPos.x)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Down
            if (v.transform.position.z == CurrentPos.z - 1 &&
                v.transform.position.x == CurrentPos.x)
            {
                neighbors.Add(v);
                continue;
            }
        }
    }
}
