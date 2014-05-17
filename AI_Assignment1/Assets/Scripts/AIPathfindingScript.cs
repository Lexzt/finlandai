using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathfindingScript : MonoBehaviour {
	public float m_fSpeed = 5.0f;
    private GameObject goal;
	private Stack<GameObject> s_Path = null;

	public GameObject currentEnd = null;

	private float startTime;
	private float journeyLength;

    private GameObject currentTarget;
    private bool m_bReCalc = false;
    private Transform currentEndTrans;

	private List<GameObject> ListArray;
	private GameObject[] FinalArray;

	private GameObject LevelGeneratorClass;
    private LevelGenerator LevelGeneratorInstance;

    void Start () 
    {
		LevelGeneratorClass = GameObject.FindGameObjectWithTag ("LevelGenerator");
        LevelGeneratorInstance = LevelGeneratorClass.GetComponent<LevelGenerator>();

        GameObject Newplayer = GameObject.FindGameObjectWithTag("Player");
        currentTarget = new GameObject();
        currentTarget.transform.position = new Vector3(Newplayer.transform.position.x, Newplayer.transform.position.y, Newplayer.transform.position.z);

		FinalArray = LevelGeneratorInstance.CurrentNodeActiveLevel();
    }

	void Update () 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

		if(player.GetComponent<PlayerScript>().invulnerable == false && player != null)
		{
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
//				//Debug.Log("RunErrorCheck1");
				s_Path = DijkstraAlgorithm.Dijkstra(FinalArray,
													gameObject.GetComponent<CurrentNodeScript>().currentNode,
													player.GetComponent<CurrentNodeScript>().currentNode);
				//Debug.Log("Error1");
			}
	        else if (m_bReCalc == true)
	        {
//				//Debug.Log("RunErrorCheck2");
	            m_bReCalc = false;
	            currentTarget.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
//				//Debug.Log("RunErrorCheck3");

				//Debug.Log(gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position.x + ", " + gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position.y + ", " + gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position.z);
				//Debug.Log(player.GetComponent<CurrentNodeScript>().currentNode.transform.position.x + ", " + player.GetComponent<CurrentNodeScript>().currentNode.transform.position.y + ", " + player.GetComponent<CurrentNodeScript>().currentNode.transform.position.z);
				//Debug.Log(FinalArray.Length);

	            s_Path.Clear();
	            s_Path = DijkstraAlgorithm.Dijkstra(FinalArray,
								                    gameObject.GetComponent<CurrentNodeScript>().currentNode,
								                    player.GetComponent<CurrentNodeScript>().currentNode);
				//Debug.Log("Error2");
	        }
	        
			if (gameObject.GetComponent<CurrentNodeScript>().currentNode.transform.position != player.GetComponent<CurrentNodeScript>().currentNode.transform.position)
			{
				if (s_Path != null)
		        {
					if ((currentEnd == null || transform.position == currentEnd.transform.position) && s_Path.Count != 0)
		            {
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
	}
}
