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
				BitCollider.gameObject.transform.position = Ghost.transform.position;
			}
			else
			{
				PlayerScript.ReduceLives();
			}
		}
	}
}
