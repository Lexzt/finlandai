using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DijkstraAlgorithm{

    public static Stack<GameObject> Dijkstra(GameObject[] Graph, GameObject source, GameObject target)
    {
        Dictionary<GameObject, float> dist = new Dictionary<GameObject, float>();
        Dictionary<GameObject, GameObject> previous = new Dictionary<GameObject, GameObject>();
        List<GameObject> Q = new List<GameObject>();

        foreach (GameObject v in Graph)
        {
            dist[v] = Mathf.Infinity;
            previous[v] = null;
            Q.Add(v);
        }

        dist[source] = 0;

        while (Q.Count > 0)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject shortestDistanceNode = null;
            foreach (GameObject obj in Q)
            {
                if (dist[obj] < shortestDistance)
                {
                    shortestDistance = dist[obj];
                    shortestDistanceNode = obj;
                }
            }

            GameObject u = shortestDistanceNode;

            Q.Remove(u);


            //Check to see if we made it to the target
            if (u == target)
            {
                Stack<GameObject> S = new Stack<GameObject>();
                while (previous[u] != null)
                {
                    S.Push(u);
                    u = previous[u];
                }
                return S;
            }

            if (dist[u] == Mathf.Infinity)
            {
                break;
            }

            foreach (GameObject v in u.GetComponent<NodeScript>().neighbors)
            {
                float alt = dist[u] + (u.transform.position - v.transform.position).magnitude;

                if (alt < dist[v])
                {
                    dist[v] = alt;
                    previous[v] = u;
                }
            }
        }
        return null;
    }
}
