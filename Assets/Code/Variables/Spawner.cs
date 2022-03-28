using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private float spawnRangeX = 1.0f;
    private float spawnRangeZ = 1.0f;
    private float spawnRangeY;
    public float rangeX, rangeY, rangeZ;
    public int spawnCount;
    [SerializeField]
    private float waitInterval;
    // came from fireball
    //public GameObject fprefab;
    public float throwForce = 2f;
    public UnityEvent OnSpawnedEvent;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ThrowFireball", waitInterval);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ThrowFireball()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x + Random.Range(0, spawnRangeX), transform.position.y, transform.position.z);
        GameObject fireball = Instantiate(enemyPrefab[Random.Range(0, 3)], spawnPoint, Quaternion.identity) as GameObject;
        fireball.GetComponent<Rigidbody>().AddForce(-transform.up * throwForce, ForceMode.Impulse);
        OnSpawnedEvent.Invoke();
        Invoke("ThrowFireball", 4f);
    }
}

