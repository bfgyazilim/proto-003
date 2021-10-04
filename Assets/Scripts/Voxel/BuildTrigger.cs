using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTrigger : MonoBehaviour
{
    [SerializeField]
    float unitOffsetX, unitOffsetY, unitOffsetZ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Crate" && other.gameObject.name == "Player")
        if (other.gameObject.name == "Player")
        {
            WorldController.instance.GenerateBlocks(transform.position.x + unitOffsetX, transform.position.y + unitOffsetY, transform.position.z + unitOffsetZ);
            Debug.Log("BuildTrigger collided with: " + other.gameObject.name);

        }
    }
}
