using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BitCollection : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//function to check if Player has entered Bit's sphere collider
	public void OnTriggerEnter(Collider BitCollider)
	{
		if(BitCollider.gameObject.tag == "Bits")
		{
			Destroy(BitCollider.gameObject);												//destroy the Bit Player has collided against
			GetComponent<PlayerScript>().AddBitPoint();										//add point upon collision with a Bit
		}
	}
}
