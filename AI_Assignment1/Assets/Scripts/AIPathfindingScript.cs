using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathfindingScript : MonoBehaviour {

    public float m_fSpeed               = 1.0f;
    public float m_fTurnSpeed           = 30.0f;
    public float m_fSphereCastRadius    = 0.5f;

    private GameObject goal;
	private Stack<GameObject> s_Path = null;

	public GameObject currentEnd = null;

	private float startTime;
	private float journeyLength;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

		if(s_Path == null)
		{
			s_Path = DijkstraAlgorithm.Dijkstra(
					 GameObject.FindGameObjectsWithTag("EnvironmentCube"),
					 gameObject.GetComponent<CurrentNodeScript>().currentNode,
					 player.GetComponent<CurrentNodeScript>().currentNode);
		}
		else if(s_Path != null)
		{
			if(currentEnd == null || transform.position == currentEnd.transform.position)
			{
				currentEnd = s_Path.Pop();

				startTime = Time.time;
				journeyLength = Vector3.Distance(transform.position, currentEnd.transform.position);
			}
			else
			{
				float distCovered = (Time.time - startTime) * m_fSpeed;
				float fracJourney = distCovered / journeyLength;

				transform.position = Vector3.Lerp(transform.position,currentEnd.transform.position, fracJourney);
			}
//				Debug.Log("Current End: "+currentEnd.transform.position.x+" "+currentEnd.transform.position.y+" "+currentEnd.transform.position.z);
//				//transform.position = currentEnd.transform.position;
//
//
//				Vector3 v_goalPosition = currentEnd.transform.position;
//				Vector3 v_goalDirection = v_goalPosition - transform.position;
//				v_goalDirection.y = 0.0f;
//				Vector3 v_normalizedDirection = v_goalDirection.normalized;
//				transform.position += transform.forward * m_fSpeed * Time.deltaTime;
//				//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(v_normalizedDirection), m_fTurnSpeed * Time.deltaTime);
//			}
		}
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
