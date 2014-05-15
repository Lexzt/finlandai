using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurrentNodeScript : MonoBehaviour {

    public GameObject currentNode;
    public bool m_bisAI = true;
	private List<GameObject> ListArray;
	private GameObject[] FinalArray;

    private GameObject LevelGeneratorClass;
    private LevelGenerator LevelGeneratorInstance;
	
	// Use this for initialization
	void Start () 
    {
        LevelGeneratorClass = GameObject.FindGameObjectWithTag("LevelGenerator");
        LevelGeneratorInstance = LevelGeneratorClass.GetComponent<LevelGenerator>();

        //GameObject[] ObjectArray 	= GameObject.FindGameObjectsWithTag ("Bits");
        //GameObject[] BigBitsArray 	= GameObject.FindGameObjectsWithTag ("BigBits");
        //GameObject[] EmptyArray 	= GameObject.FindGameObjectsWithTag ("Empty");
        //GameObject EnemySpawn 		= GameObject.FindGameObjectWithTag 	("EnemySpawn");
        //GameObject PlayerSpawn	 	= GameObject.FindGameObjectWithTag 	("PlayerSpawn");
        //ListArray = new List<GameObject>();
		
        //foreach (GameObject v in ObjectArray) 
        //{
        //    ListArray.Add(v);
        //}
		
        //foreach (GameObject v in BigBitsArray) 
        //{
        //    ListArray.Add(v);
        //}
		
        //foreach (GameObject v in EmptyArray) 
        //{
        //    ListArray.Add(v);
        //}		
        //ListArray.Add(EnemySpawn);
        //ListArray.Add(PlayerSpawn);
		
        //FinalArray = ListArray.ToArray ();

        FinalArray = LevelGeneratorInstance.CurrentActiveLevel();
        SetCurrentNode();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_bisAI || currentNode == null)
        {
            SetCurrentNode();
        }
	}

    void SetCurrentNode()
    {
        float f_ShortestDistance = Mathf.Infinity;
		foreach (GameObject obj in FinalArray)
        {
            float f_Distance = (obj.transform.position - transform.position).magnitude;
            if (f_Distance < f_ShortestDistance)
            {
                f_ShortestDistance = f_Distance;
                currentNode = obj;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bits")
        {
            currentNode = col.gameObject;
        }
    }
}
