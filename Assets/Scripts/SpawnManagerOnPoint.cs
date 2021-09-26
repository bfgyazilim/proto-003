using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerOnPoint : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRangeX = 20.0f;
    private float spawnRangeZ = 20.0f;
    private float spawnRangeY;
    public float rangeX,rangeY,rangeZ;
    public int spawnCount;

    private void Awake()
    {
        spawnRangeX = transform.position.x + rangeX;
        spawnRangeY = transform.position.y + rangeY;
        spawnRangeZ = transform.position.z + rangeZ;

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< spawnCount; i++)
        {
            float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
            float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            float spawnPosY = Random.Range(-spawnRangeY, spawnRangeY);

            Vector3 randomPos = new Vector3(spawnPosX, spawnRangeY, spawnPosZ);
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
