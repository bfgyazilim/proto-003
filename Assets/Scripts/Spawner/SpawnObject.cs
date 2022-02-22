using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject spawnObject;

    [SerializeField]
    int objectCount = 5;

    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < objectCount; i++)
        {
            Instantiate(spawnObject, transform.position, transform.rotation);
        }
    }    
}