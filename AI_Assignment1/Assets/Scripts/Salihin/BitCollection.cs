using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BitCollection : MonoBehaviour {

	//function to check if Player has entered Bit's sphere collider
	private void OnTriggerEnter(Collider BitCollider)
	{
		var PlayerScript = GetComponent<PlayerScript>();
		if(BitCollider.gameObject.tag == "Bits")
		{
			//Destroy(BitCollider.gameObject);												//destroy the Bit Player has collided against

            BitCollider.GetComponent<MeshRenderer>().enabled = false;
            BitCollider.GetComponent<SphereCollider>().enabled = false;
            BitCollider.GetComponent<SphereCollider>().isTrigger = false;
			PlayerScript.AddBitPoint(PlayerScript.BITPIECE.BITS);							//add point upon collision with a Bit
		}
		else if(BitCollider.gameObject.tag == "BigBits")
		{
			//Destroy(BitCollider.gameObject);												//destroy the Bit Player has collided against
			
			BitCollider.GetComponent<MeshRenderer>().enabled = false;
			BitCollider.GetComponent<SphereCollider>().enabled = false;
			BitCollider.GetComponent<SphereCollider>().isTrigger = false;
			PlayerScript.AddBitPoint(PlayerScript.BITPIECE.BIGBITS);						//add 3 points upon collision with a BigBit
			PlayerScript.invulnerable = true;
		}
	}
}
