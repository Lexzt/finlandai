using UnityEngine;
using System.Collections;

public class NodeDetails : MonoBehaviour {

	// Id of the prefab count
	public int id = 0;

	public int waypointID = 0;

	// x and y position in the list
	public int x = 0;
	public int y = 0;

	public Object[] PrefabArray;

	public GameObject currentObject = null;

	// Decides what prefab to spawn out
	public void SetTypeOfPrefab()
	{
		if(currentObject != null)
		{
			Destroy(currentObject);
		}

		currentObject = Instantiate(PrefabArray[id], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		currentObject.GetComponent<NodeScript> ().enabled = false;

		if(currentObject.collider)
		{
			currentObject.collider.enabled = false;
		}

		currentObject.transform.parent = GetComponent<Transform>();
		currentObject.transform.localPosition = Vector3.zero;
	}
	
	public void TrackOldShader()
	{
		GetComponent<ToggleOutline> ().OriginalShader (currentObject.GetComponent<Renderer>().material.shader);
	}

	public void OnNodeOutline()
	{
		GetComponent<ToggleOutline> ().TurnOnOutline (currentObject);
	}

	public void OffNodeOutline()
	{
		GetComponent<ToggleOutline> ().TurnOffOutline (currentObject);
	}
}