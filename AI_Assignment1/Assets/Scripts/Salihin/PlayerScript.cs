using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
		transform.position = playerSpawn.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//function to increase PlayerPoint variable in PlayerHUD script
	public void AddBitPoint()
	{
		GetComponent<PlayerHUD>().PlayerPoint++;
	}
}
