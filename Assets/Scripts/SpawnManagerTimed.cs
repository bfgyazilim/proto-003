using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerTimed : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRangeX = 20.0f;
    private float spawnRangeZ = 20.0f;
    private float spawnRangeY;
    public float rangeX,rangeY,rangeZ;
    public int spawnCount;


    // came from fireball
    //public GameObject fprefab;
    public float throwForce = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ThrowFireball", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ThrowFireball()
    {
        GameObject fireball = Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
        fireball.GetComponent<Rigidbody>().AddForce(-transform.up * throwForce, ForceMode.Impulse);
        Invoke("ThrowFireball", 4f);

    }
}
