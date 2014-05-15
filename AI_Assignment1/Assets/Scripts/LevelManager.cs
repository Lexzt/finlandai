using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
    public List<GameObject> DontDestroyOnLoadGameObjects = new List<GameObject>();
    public bool m_bCallOnce;

    private LevelGenerator LevelGeneratorInstance;

    void Awake()
    {
        m_bCallOnce = false;

        for (int i = 0; i < DontDestroyOnLoadGameObjects.Count; ++i)
        {
            if (GameObject.FindGameObjectWithTag(DontDestroyOnLoadGameObjects[i].tag) == null)
            {
                GameObject newObj = Instantiate(DontDestroyOnLoadGameObjects[i]) as GameObject;
                newObj.name = DontDestroyOnLoadGameObjects[i].tag;
            }
        }
    }
}
