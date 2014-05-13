using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathfindingScript : MonoBehaviour {

    public float m_fSpeed               = 1.0f;
    public float m_fTurnSpeed           = 30.0f;
    public float m_fSphereCastRadius    = 0.5f;

    private GameObject goal;
	public Stack<GameObject> s_Path = null;

	public GameObject currentEnd = null;

	private float startTime;
	private float journeyLength;

    private GameObject currentTarget;
    //private Transform playerTrans;
    private bool m_bReCalc = false;
    private Transform currentEndTrans;

	private List<GameObject> ListArray;
	private GameObject[] FinalArray;
    void Start () 
    {
        GameObject Newplayer = GameObject.FindGameObjectWithTag("Player");
        currentTarget = new GameObject();
        currentTarget.transform.position = new Vector3(Newplayer.transform.position.x, Newplayer.transform.position.y, Newplayer.transform.position.z);

		GameObject[] ObjectArray 	= GameObject.FindGameObjectsWithTag ("Bits");
		GameObject[] BigBitsArray 	= GameObject.FindGameObjectsWithTag ("BigBits");
		GameObject[] EmptyArray 	= GameObject.FindGameObjectsWithTag ("Empty");
		GameObject EnemySpawn 		= GameObject.FindGameObjectWithTag 	("EnemySpawn");
		GameObject PlayerSpawn	 	= GameObject.FindGameObjectWithTag 	("PlayerSpawn");
		ListArray = new List<GameObject>();
		
		foreach (GameObject v in ObjectArray) 
		{
			ListArray.Add(v);
		}

		foreach (GameObject v in BigBitsArray) 
		{
			ListArray.Add(v);
		}
		
		foreach (GameObject v in EmptyArray) 
		{
			ListArray.Add(v);
		}		
		ListArray.Add(EnemySpawn);
		ListArray.Add(PlayerSpawn);

		FinalArray = ListArray.ToArray ();
    }

	// Update is called once per frame
	void Update () 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (currentTarget.transform.position.x != player.transform.position.x &&
            currentTarget.transform.position.z != player.transform.position.z)
        {
            m_bReCalc = true;
        }
        else
        {
            m_bReCalc = false;
        }

        if(s_Path == null)
        {
			s_Path = DijkstraAlgorithm.Dijkstra(
						FinalArray,
					 	gameObject.GetComponent<CurrentNodeScript>().currentNode,
					 	player.GetComponent<CurrentNodeScript>().currentNode);
		}
        else if (m_bReCalc == true)
        {
            //Debug.Log("Re calculating Path");

            m_bReCalc = false;
            currentTarget.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            s_Path.Clear();
            s_Path = DijkstraAlgorithm.Dijkstra(
						FinalArray,
                     	gameObject.GetComponent<CurrentNodeScript>().currentNode,
                    	player.GetComponent<CurrentNodeScript>().currentNode);
        }
        
		if (gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position != player.GetComponent<CurrentNodeScript>().currentNode.transform.position)
		{
			if (s_Path != null)
	        {
				if ((currentEnd == null || transform.position == currentEnd.transform.position) && s_Path.Count != 0)
	            {
	                //Debug.Log("Pop Stack");
	                currentEnd = s_Path.Pop();

	                startTime = Time.time;
	                journeyLength = Vector3.Distance(transform.position, currentEnd.transform.position);
	            }
	            else
	            {
	                float distCovered = (Time.time - startTime) * m_fSpeed;
	                float fracJourney = distCovered / journeyLength;

	                transform.position = Vector3.MoveTowards(transform.position, currentEnd.transform.position, fracJourney);
	            }
	        }
		}
		else
		{
			// Player minus health over here.
			// Add later
		}
	}
 
//    void OnDrawGizmos()
//    {
//        GameObject player = GameObject.FindGameObjectWithTag("Player");
//
//        Gizmos.color = Color.green;
//        Gizmos.DrawLine(transform.position, goal.transform.position);
//
//        RaycastHit hit;
//
//
//        Physics.SphereCast(transform.position, m_fSphereCastRadius, player.transform.position - transform.position, out hit);
//
//        if (hit.collider.tag != "Player")
//        {
//            Gizmos.color = Color.red;
//        }
//
//        Gizmos.DrawLine(transform.position, player.transform.position);
//    }
}
