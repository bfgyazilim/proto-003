using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBotEnemy : MonoBehaviour
{
    public GameObject fprefab;
    public float throwForce = 2f;
    public float spawnInterval;

    public bool directionLeft, directionRight, directionUp, directionDown;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ThrowFireball", spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ThrowFireball()
    {
        if (directionDown)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
        }

        if (directionUp)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        if (directionLeft)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(-transform.right * throwForce, ForceMode.Impulse);
        }

        if (directionRight)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(transform.right * throwForce, ForceMode.Impulse);
        }


        Invoke("ThrowFireball", spawnInterval);

    }
}
