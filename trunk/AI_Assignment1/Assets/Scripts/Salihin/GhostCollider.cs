using UnityEngine;
using System.Collections;

public class GhostCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//function to check if Player has entered Bit's sphere collider
	private void OnTriggerEnter(Collider BitCollider)
	{
		var PlayerScript = GetComponent<PlayerScript>();
		var Ghost = GameObject.FindGameObjectWithTag("EnemySpawn");
		if(BitCollider.gameObject.tag == "Ghost")
		{
			if(PlayerScript.invulnerable)
			{
				//BitCollider.gameObject.transform.position = Ghost.transform.position;
				if(BitCollider.gameObject.name == "Shadow(Clone)")
				{
					Instantiate(GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().AIArray[0], Ghost.transform.position, Quaternion.identity);
					Destroy(BitCollider.gameObject);
				}
				else if(BitCollider.gameObject.name == "Prediction AI(Kinnear)(Clone)")
				{
//					Instantiate(GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().AIArray[1], Ghost.transform.position, Quaternion.identity);
					BitCollider.gameObject.transform.position = Ghost.transform.position;
				}
				else if(BitCollider.gameObject.name == "Waypoint AI(Kinnear)(Clone)")
				{
//					Instantiate(GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().AIArray[2], Ghost.transform.position, Quaternion.identity);
					BitCollider.gameObject.transform.position = Ghost.transform.position;
				}
				else if(BitCollider.gameObject.name == "AStarAI(Clone)")
				{
					Instantiate(GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().AIArray[3], Ghost.transform.position, Quaternion.identity);
					Destroy(BitCollider.gameObject);
				}
				else if(BitCollider.gameObject.name == "DijkstraAI(Clone)")
				{
					Instantiate(GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().AIArray[4], Ghost.transform.position, Quaternion.identity);
					Destroy(BitCollider.gameObject);
				}
			}
			else
			{
				PlayerScript.ReduceLives();
			}
		}
	}
}
