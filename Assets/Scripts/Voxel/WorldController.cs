using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject block;
    public int depth = 2;
    public int height = 2;
    public int width = 2;
    public IEnumerator BuildWorld()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y >= height - 2 && Random.Range(0, 100) < 50) continue;
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);
                    cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                    cube.name = x + "_" + y + "_" + z;
                }
                yield return null;
            }
        }
    }

    // use this for initialization
    private void Start()
    {
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    private void Update()
    {

    }

}
