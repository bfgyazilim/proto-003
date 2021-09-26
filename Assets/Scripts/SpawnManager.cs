using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRangeX = 20.0f;
    private float spawnRangeZ = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< 20; i++)
        {
            float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
            float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);

            Vector3 randomPos = new Vector3(spawnPosX, 0.64f, spawnPosZ);
            Instantiate(enemyPrefab, randomPos, enemyPrefab.transform.rotation);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateSpawnPosition()
    {

    }
}
