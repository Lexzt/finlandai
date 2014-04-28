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
	
    void Start () 
    {
        GameObject Newplayer = GameObject.FindGameObjectWithTag("Player");
        currentTarget = new GameObject();
        currentTarget.transform.position = new Vector3(Newplayer.transform.position.x, Newplayer.transform.position.y, Newplayer.transform.position.z);
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
					 GameObject.FindGameObjectsWithTag("Bits"),
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
                     GameObject.FindGameObjectsWithTag("Bits"),
                     gameObject.GetComponent<CurrentNodeScript>().currentNode,
                     player.GetComponent<CurrentNodeScript>().currentNode);
        }
        
		if (gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position != player.GetComponent<CurrentNodeScript>().currentNode.transform.position)
		{
			if (s_Path != null)
	        {
	            if (currentEnd == null || transform.position == currentEnd.transform.position)
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

	                transform.position = Vector3.Lerp(transform.position, currentEnd.transform.position, fracJourney);
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
