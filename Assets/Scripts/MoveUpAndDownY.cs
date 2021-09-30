using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDownY : MonoBehaviour
{
    public float speed, distance;
    [SerializeField]
    private float minY, maxY;

    public bool right, dontMove;
    private bool stop;

    void Start()
    {
        //maxY = transform.position.Y + distance;
        //minY = transform.position.Y - distance;
    }

    void Update()
    {
        if(!stop && !dontMove)
        {
            if (right)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y >= maxY)
                    right = false;
            }
            else
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y <= minY)
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
