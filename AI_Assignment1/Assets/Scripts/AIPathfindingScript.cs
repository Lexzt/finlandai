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

        //if (player.GetComponent<CurrentNodeScript>().GetType() != playerCurrent.GetComponent<CurrentNodeScript>().GetType())
        //{
        //    s_Path.Clear();
        //}
        //Debug.Log("Player: " + player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z);
        //Debug.Log("Current: " + playerTrans.position.x + " " + playerTrans.position.y + " " + playerTrans.position.z);
        //if (!(player.transform.position.Equals(playerTrans)) && s_Path != null)
        //{
        //    s_Path.Clear();
        //    Debug.Log("Recalc");
        //}

        //Debug.Log("EndPt : " + currentTarget.transform.position.x + " " + currentTarget.transform.position.y + " " + currentTarget.transform.position.z);
        //Debug.Log("Player: " + player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z);
        //Debug.Log("Recalc: " + m_bReCalc);



        //if (m_bReCalc == true)
        //{
        //    s_Path.Clear();
        //    s_Path = DijkstraAlgorithm.Dijkstra(
        //             GameObject.FindGameObjectsWithTag("EnvironmentCube"),
        //             gameObject.GetComponent<CurrentNodeScript>().currentNode,
        //             player.GetComponent<CurrentNodeScript>().currentNode);

        //    currentTarget.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        //    m_bReCalc = false;
        //}

        //if (player.rigidbody.velocity.magnitude > 0 && s_Path != null)
        //{
        //    s_Path.Clear();
        //    s_Path = DijkstraAlgorithm.Dijkstra(
        //             GameObject.FindGameObjectsWithTag("EnvironmentCube"),
        //             gameObject.GetComponent<CurrentNodeScript>().currentNode,
        //             player.GetComponent<CurrentNodeScript>().currentNode);
        //}

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
            Debug.Log("Re calculating Path");

            m_bReCalc = false;
            currentTarget.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            s_Path.Clear();
            s_Path = DijkstraAlgorithm.Dijkstra(
                     GameObject.FindGameObjectsWithTag("Bits"),
                     gameObject.GetComponent<CurrentNodeScript>().currentNode,
                     player.GetComponent<CurrentNodeScript>().currentNode);
        }
        else if (s_Path != null)
        {
            if (currentEnd == null || transform.position == currentEnd.transform.position)
            {
                Debug.Log("Pop Stack");
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
        //Debug.Log(s_Path.Count);

        //foreach (GameObject obj in s_Path)
        //{
        //    Debug.Log(obj.transform.position.x + " " + obj.transform.position.y + " " + obj.transform.position.z);
        //}
	}
        //Debug.Log(player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z);
//		//If the dude can be "seen" (Via raycast) then chase
//		RaycastHit hit;
//		
//		
//		Physics.SphereCast(transform.position, m_fSphereCastRadius, player.transform.position - transform.position, out hit);
//		
//		if(hit.collider.tag == "Player")
//		{
//			Vector3 playerPosition = player.transform.position;
//			Vector3 playerDirection = playerPosition - transform.position;
//			playerDirection.y = 0.0f;
//			Vector3 normalizedPlayerDirection = playerDirection.normalized;
//			transform.position += transform.forward * m_fSpeed * Time.deltaTime;
//			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(normalizedPlayerDirection), m_fTurnSpeed *Time.deltaTime);
//			gameObject.GetComponent<CurrentNodeScript>().currentNode = null;
//		}
//		else
//		{
//	        Stack<GameObject> s_Path = DijkstraAlgorithm.Dijkstra(
//	                                    GameObject.FindGameObjectsWithTag("EnvironmentCube"),
//	                                    gameObject.GetComponent<CurrentNodeScript>().currentNode,
//	                                    player.GetComponent<CurrentNodeScript>().currentNode);
//	        //Debug.Log(s_Path.Peek().transform.position.x + " " + s_Path.Peek().transform.position.y + " " + s_Path.Peek().transform.position.z);
//	        if (s_Path != null)
//	        {
//	            goal = s_Path.Pop();
//	            Debug.Log(goal.transform.position.x + " " + goal.transform.position.y + " " + goal.transform.position.z);
//	            Vector3 v_goalPosition = goal.transform.position;
//	            Vector3 v_goalDirection = v_goalPosition - transform.position;
//	            v_goalDirection.y = 0.0f;
//	            Vector3 v_normalizedDirection = v_goalDirection.normalized;
//	            transform.position += transform.forward * m_fSpeed * Time.deltaTime;
//	            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(v_normalizedDirection), m_fTurnSpeed * Time.deltaTime);
//	        }
//		}

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
