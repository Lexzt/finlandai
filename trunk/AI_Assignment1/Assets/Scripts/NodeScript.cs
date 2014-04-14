using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeScript : MonoBehaviour {

	public List<GameObject> neighbors;
    public GameObject goal;

    void Start()
    {
        Vector3 CurrentPos = transform.position;
        foreach (GameObject v in GameObject.FindGameObjectsWithTag("EnvironmentCube"))
        {
            // Check Left
            if (v.transform.position.x == CurrentPos.x + 1 &&
                v.transform.position.z == CurrentPos.z)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Right
            if (v.transform.position.x == CurrentPos.x - 1 &&
                v.transform.position.z == CurrentPos.z)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Up
            if (v.transform.position.z == CurrentPos.z + 1 &&
                v.transform.position.x == CurrentPos.x)
            {
                neighbors.Add(v);
                continue;
            }

            // Check Down
            if (v.transform.position.z == CurrentPos.z - 1 &&
                v.transform.position.x == CurrentPos.x)
            {
                neighbors.Add(v);
                continue;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        foreach (GameObject neighbor in neighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
            Gizmos.DrawWireSphere(neighbor.transform.position, 0.25f);
        }

        if (goal)
        {
            Gizmos.color = Color.green;
            GameObject current = gameObject;
            Stack<GameObject> path = DijkstraAlgorithm.Dijkstra(GameObject.FindGameObjectsWithTag("EnvironmentCube"), gameObject, goal);

            foreach (GameObject obj in path)
            {
                Debug.Log("Got here also!");
                Gizmos.DrawWireSphere(obj.transform.position, 1.0f);
                Gizmos.DrawLine(current.transform.position, obj.transform.position);
                current = obj;
            }
        }
    }
}
