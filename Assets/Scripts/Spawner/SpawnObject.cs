using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject spawnObject;

    [SerializeField]
    int objectCount = 5;

    [SerializeField]
    float destroyAfterInterval = 0.2f;

    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < objectCount; i++)
        {
           GameObject go = Instantiate(spawnObject, transform.position, transform.rotation);
           Destroy(go, destroyAfterInterval);
        }
    }    
}