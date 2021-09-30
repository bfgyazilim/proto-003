using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDownX : MonoBehaviour
{
    public float speed, distance;
    [SerializeField]
    private float minX, maxX;

    public bool right, dontMove;
    private bool stop;

    void Start()
    {
        //maxZ = transform.position.z + distance;
        //minZ = transform.position.z - distance;
    }

    void Update()
    {
        if(!stop && !dontMove)
        {
            if (right)
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
                if (transform.position.x >= maxX)
                    right = false;
            }
            else
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
                if (transform.position.x <= minX)
                    right = true;
            }
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.tag == "White" && target.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1 
            || target.gameObject.name == "Player")
        {
            stop = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
