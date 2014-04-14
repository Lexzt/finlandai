using UnityEngine;
using System.Collections;

public class CurrentNodeScript : MonoBehaviour {

    public GameObject currentNode;
    public bool m_bisAI = true;

	// Use this for initialization
	void Start () 
    {
        SetCurrentNode();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!m_bisAI || currentNode == null)
        {
            SetCurrentNode();
        }
	}

    void SetCurrentNode()
    {
        float f_ShortestDistance = Mathf.Infinity;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EnvironmentCube"))
        {
            float f_Distance = (obj.transform.position - transform.position).magnitude;
            if (f_Distance < f_ShortestDistance)
            {
                f_ShortestDistance = f_Distance;
                currentNode = obj;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "EnvironmentCube")
        {
            currentNode = col.gameObject;
        }
    }
}
