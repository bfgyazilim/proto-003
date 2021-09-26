using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball4Dir : MonoBehaviour
{
    public GameObject fprefab;
    public float throwForce = 2f;
    public float spawnInterval;

    public bool directionLeft, directionRight,directionUp,directionDown;
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
        if(directionDown)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
            Destroy(fireball, 10f);
        }

        if (directionUp)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
            Destroy(fireball, 10f);
        }

        if (directionLeft)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(-transform.right * throwForce, ForceMode.Impulse);
            Destroy(fireball, 10f);
        }

        if (directionRight)
        {
            GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
            fireball.GetComponent<Rigidbody>().AddForce(transform.right * throwForce, ForceMode.Impulse);
            Destroy(fireball, 10f);
        }


        Invoke("ThrowFireball", spawnInterval);

    }
}
