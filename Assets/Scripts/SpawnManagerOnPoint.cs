using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerOnPoint : MonoBehaviour
{
    public GameObject block;
    private float spawnRangeX = 20.0f;
    private float spawnRangeZ = 20.0f;
    private float spawnRangeY;
    public float rangeX,rangeY,rangeZ;
    public int spawnCount;

    public int depth = 2;
    public int height = 2;
    public int width = 2;
    [SerializeField]
    Vector3 offset;

    private void Awake()
    {
        spawnRangeX = transform.position.x + rangeX;
        spawnRangeY = transform.position.y + rangeY;
        spawnRangeZ = transform.position.z + rangeZ;

    }

    // Start is called before the first frame update
    void Start()
    {        
        //StartCoroutine(BuildBlocksRandom(offset.x, offset.y , offset.z));

        StartCoroutine(BuildBlocksFixed(offset.x, offset.y, offset.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRandomOrder()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
            float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            float spawnPosY = Random.Range(-spawnRangeY, spawnRangeY);

            Vector3 randomPos = new Vector3(spawnPosX, spawnRangeY, spawnPosZ);
            Instantiate(block, randomPos, block.transform.rotation);
        }
    }

    private void GenerateBoxFormation()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPos = new Vector3(transform.position.x + i, transform.position.y, transform.position.z + i);
            Instantiate(block, randomPos, block.transform.rotation);
        }
    }

    /// <summary>
    /// Build generation co-routine
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildBlocksRandom(float ox, float oy, float oz)
    {
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y >= height - 2 && Random.Range(0, 100) < 50) continue;
                    Vector3 pos = new Vector3(x, y, z);
                    Vector3 offset = new Vector3(ox, oy, oz);
                    GameObject cube = GameObject.Instantiate(block, transform.position + pos + offset, Quaternion.identity);
                    //cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                    cube.name = "V_" + x + "_" + y + "_" + z;
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// Build generation co-routine
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildBlocksFixed(float ox, float oy, float oz)
    {
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    Vector3 offset = new Vector3(ox, oy, oz);
                    GameObject cube = GameObject.Instantiate(block, transform.position + pos, Quaternion.identity);
                    //cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                    cube.name = "V_" + x + "_" + y + "_" + z;
                }
                yield return null;
            }
        }
    }
}
