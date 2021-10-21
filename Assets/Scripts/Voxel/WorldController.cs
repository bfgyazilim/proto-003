using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class WorldController : MonoBehaviour
{
    public GameObject block;
    public int depth = 2;
    public int height = 2;
    public int width = 2;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Color emissionColor;
    public static WorldController instance;

    // use this for initialization
    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    /// <summary>
    /// Build generation co-routine
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildUnit(float ox, float oy, float oz)
    {
        Vector3 offset = new Vector3(ox, oy, oz);
        //GameObject cube = GameObject.Instantiate(block, offset, Quaternion.identity);

        // Instantiate the prefab in 45 degree slight angle
        GameObject cube = GameObject.Instantiate(block, offset, Quaternion.Euler(0, 45, 0));

        //cube.GetComponent<Renderer>().material.SetColor("_EmissionColor", emissionColor);
        cube.name = "V_" + offset.x + "_" + offset.y + "_" +  offset.z;

        // Cinemachine Camera Shake feedback
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        // DO Tween Shake Scale
        cube.transform.DOShakeScale(.5f, .2f, 10, 90, true);

        // Handle Feedback FX
        FXManager.instance.HandleTriggerFeedbackParticlesByOrder(1);
        yield return null;
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
                    GameObject cube = GameObject.Instantiate(block, pos + offset, Quaternion.identity);
                    //cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                    cube.name = "V_" + x + "_" + y + "_" + z;
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// Generates random blocks in the given size and shape!
    /// </summary>
    public void GenerateBlocks(float x, float y, float z)
    {
        StartCoroutine(BuildUnit(x, y, z));
    }
}
