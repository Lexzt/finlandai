using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//enumerator for the Bits pieces in the environment
	public enum BITPIECE
	{
		BITS = 0,
		BIGBITS
	};

	//enumerator for Pacman's colour
	private enum MATCOLOUR
	{
		YELLOW = 0,
		RED
	};

	[HideInInspector]public bool invulnerable = false;					//component for invulnerability mode
	public float invulnerable_time;										//component for invulnerability time

	private float ticks = 0.0f;											//component for timer
	
	// Update is called once per frame
	void Update () {
		Invulnerable();
	}

	//function to increase PlayerPoint variable in PlayerHUD script
	public void AddBitPoint(BITPIECE BP)
	{
		var PlayerHUD = GetComponent<PlayerHUD>();
		switch(BP)
		{
		case BITPIECE.BITS:
			PlayerHUD.PlayerPoint++;
			break;
		case BITPIECE.BIGBITS:
			PlayerHUD.PlayerPoint += 3;
			break;
		}
	}

	//function that causes Pacman's colour to change at an interval
	private void FlickerColour()
	{
		if((int)ticks % 2 == 0)
		{
			SetColour(MATCOLOUR.RED);
		}
		else
		{
			SetColour(MATCOLOUR.YELLOW);
		}
	}

	//function to set Pacman's colour to a specific colour
	private void SetColour(MATCOLOUR MC)
	{
		var Sphere = GetComponentInChildren<MeshRenderer>();
		switch(MC)
		{
		case MATCOLOUR.YELLOW:
			Sphere.renderer.material.color = Color.yellow;
			break;
		case MATCOLOUR.RED:
			Sphere.renderer.material.color = Color.red;
			break;
		}
	}

	//function for the toggling invulnerability mode within a duration
	private void Invulnerable()
	{
		if(invulnerable)
		{
			if(ticks >= invulnerable_time)
			{
				invulnerable = false;
				SetColour(MATCOLOUR.YELLOW);
				ticks = 0.0f;
			}
			else
			{
				ticks += Time.deltaTime;
				FlickerColour();
			}
		}
	}

	//function that reduces PlayerHealth variable in PlayerHUD script
	public void ReduceLives()
	{
		var PlayerHUD = GetComponent<PlayerHUD>();

		PlayerHUD.PlayerHealth--;
		ReSpawn();
		if(PlayerHUD.PlayerHealth < 0)
		{
			PlayerHUD.PlayerHealth = 0;
		}
	}

	//function to respawn at PlayerSpawn
	private void ReSpawn()
	{
		transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
	}
}
