using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAStarScript : MonoBehaviour {

    private GameObject goal;
    public List<GameObject> s_Path = null;

    public GameObject currentEnd = null;

    private float startTime;
    private float journeyLength;

    private GameObject currentTarget;
    //private Transform playerTrans;
    private bool m_bReCalc = false;
    private Transform currentEndTrans;

    void Start()
    {

		
		//GameObject Newplayer = GameObject.FindGameObjectWithTag("Player");
        //currentTarget = new GameObject();
        //currentTarget.transform.position = new Vector3(Newplayer.transform.position.x, Newplayer.transform.position.y, Newplayer.transform.position.z);

        

        
    }

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.B))
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			GameObject tempCurrentNode1 = gameObject.GetComponent<CurrentNodeScript> ().currentNode;
			GameObject tempCurrentNode2 = player.GetComponent<CurrentNodeScript>().currentNode;
			
			s_Path = AStarAlgorithm.AStarNew(
				GameObject.FindGameObjectsWithTag("Bits"),
				tempCurrentNode1,
				tempCurrentNode2);
			
			foreach (GameObject obj in s_Path)
			{
				Debug.Log(obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.position.z);
			}
		}
	}
	
	//// Update is called once per frame
    //void Update()
    //{
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");

    //    if (currentTarget.transform.position.x != player.transform.position.x &&
    //        currentTarget.transform.position.z != player.transform.position.z)
    //    {
    //        m_bReCalc = true;
    //    }
    //    else
    //    {
    //        m_bReCalc = false;
    //    }

    //    if (s_Path == null)
    //    {
    //        Vector2 StartPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
    //        Vector2 EndPos = new Vector2(player.transform.position.x, player.transform.position.z);

    //        s_Path = AStarAlgorithm.AStar(
    //                 GameObject.FindGameObjectsWithTag("Bits"),
    //                 StartPos,
    //                 EndPos);
    //    }
    //    else if (m_bReCalc == true)
    //    {
    //        Debug.Log("Re calculating Path");

    //        m_bReCalc = false;
    //        currentTarget.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

    //        Vector2 StartPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
    //        Vector2 EndPos = new Vector2(player.transform.position.x, player.transform.position.z);

    //        s_Path.Clear();
    //        s_Path = AStarAlgorithm.AStar(
    //                 GameObject.FindGameObjectsWithTag("Bits"),
    //                 StartPos,
    //                 EndPos);
    //    }
    //    else if (s_Path != null)
    //    {
    //        if (currentEnd == null || transform.position == currentEnd.transform.position)
    //        {
    //            Debug.Log("Pop Stack");
    //            Vector2 tempEnd = s_Path.Pop();

    //            GameObject checkObj = new GameObject();
    //            checkObj.transform.position = new Vector3(tempEnd.x,currentEnd.transform.position.y,tempEnd.y);
    //            currentEnd = checkObj;

    //            startTime = Time.time;
    //            journeyLength = Vector3.Distance(transform.position, currentEnd.transform.position);
    //        }
    //        else
    //        {
    //            float distCovered = (Time.time - startTime) * m_fSpeed;
    //            float fracJourney = distCovered / journeyLength;

    //            transform.position = Vector3.Lerp(transform.position, currentEnd.transform.position, fracJourney);
    //        }
    //    }
    //    //Debug.Log(s_Path.Count);

    //    //foreach (GameObject obj in s_Path)
    //    //{
    //    //    Debug.Log(obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.position.z);
    //    //}
    //}
}
