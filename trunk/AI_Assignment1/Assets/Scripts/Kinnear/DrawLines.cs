using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLines : MonoBehaviour {

	static Material lineMaterial;
	static void CreateLineMaterial() {
		if( !lineMaterial ) {
			lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
			                            "SubShader { Pass { " +
			                            "    Blend SrcAlpha OneMinusSrcAlpha " +
			                            "    ZWrite Off Cull Off Fog { Mode Off } " +
			                            "    BindChannels {" +
			                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
			                            "} } }" );
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}


	void DrawQuad(Vector3 position, float size, float height)
	{
		GL.Begin (GL.QUADS);
			GL.Vertex3 (position.x - size, height, position.z - size);
			GL.Vertex3 (position.x - size, height, position.z + size);
			GL.Vertex3 (position.x + size, height, position.z + size);
			GL.Vertex3 (position.x + size, height, position.z - size);
		GL.End ();
	}


	// Draws the lines of the waypoints on render
	void OnPostRender() 
	{
		List<Vector3> lines = GameObject.Find("GameManager").GetComponent<EditLevel>().lineConnector;

//		//Debug.Log ("Amount of items in list: " + lines.Count);

		CreateLineMaterial();
		// set the current material
		lineMaterial.SetPass( 0 );

		GL.Begin( GL.LINES );
		GL.Color(new Color(1f,1f,1f,1f));

		for(int i = 0; i < lines.Count; i++)
		{
			GL.Vertex3( lines[i].x, lines[i].y, lines[i].z );

			if(i > 1)
			{
				GL.Vertex3( lines[i - 1].x, lines[i - 1].y, lines[i - 1].z );
			}
		}

		GL.End();		


		// Draw the waypoin quads
		for(int i = 0; i < lines.Count; i++)
		{
			DrawQuad (lines[i], 0.2f, 0.5f);
		}

	}
}
