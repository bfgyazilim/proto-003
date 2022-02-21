using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject spawnObject;

    // Start is called before the first frame update
    void OnEnable()
    {
        Instantiate(spawnObject, transform.position, transform.rotation);     
    }
    
}