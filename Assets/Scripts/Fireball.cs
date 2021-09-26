using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject fprefab;
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
        GameObject fireball = Instantiate(fprefab, transform.position, Quaternion.identity) as GameObject;
        fireball.GetComponent<Rigidbody>().AddForce(-transform.forward * throwForce, ForceMode.Impulse);
        Invoke("ThrowFireball", 4f);

    }
}
