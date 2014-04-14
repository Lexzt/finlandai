using UnityEngine;
using System.Collections;

public class ToggleOutline : MonoBehaviour {

	public Shader normalShader = Shader.Find("Diffuse");
	public Shader outlinedShader = Shader.Find("Outlined/Silhouetted Diffuse");

	public void OriginalShader(Shader original)
	{
		normalShader = original;
	}

	public void TurnOnOutline(GameObject obj)
	{
		obj.gameObject.GetComponent<Renderer> ().material.shader = outlinedShader;
	}
	
	public void TurnOffOutline(GameObject obj)
	{
		obj.gameObject.GetComponent<Renderer> ().material.shader = normalShader;
	}
}