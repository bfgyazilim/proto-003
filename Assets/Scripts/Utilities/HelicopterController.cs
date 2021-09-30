using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{

    public float speed, distanceZ, distanceY, maxDistanceZ, maxDistanceY;
    [SerializeField]
    private float minZ, maxZ;
    [SerializeField]
    private float minY, maxY;

    public bool right, up, dontMoveZ, dontMoveY;
    private bool stop;

    Transform[] transforms;

    void Start()
    {

    }

    /// <summary>
    /// Control movement around Z and Y axis simultaneously
    /// </summary>
    void Update()
    {
        // Z Axis Movement
        if (!stop && !dontMoveZ)
        {
            if (right)
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
                distanceZ += speed * Time.deltaTime;
                if (distanceZ >= maxDistanceZ)
                {
                    right = false;
                    distanceZ = 0;
                }                    
            }
            else
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
                distanceZ += speed * Time.deltaTime;
                if (distanceZ >= maxDistanceZ)
                {
                    right = true;
                    distanceZ = 0;
                }
            }
        }

        // Y Axis Movement
        if (!stop && !dontMoveY)
        {
            if (up)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                distanceY += speed * Time.deltaTime;
                if (distanceY >= maxDistanceY)
                {
                    up = false;
                    distanceY = 0;
                }
            }
            else
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                distanceY += speed * Time.deltaTime;
                if (distanceY >= maxDistanceY)
                {
                    up = true;
                    distanceY = 0;
                }
            }
        }

        /*
        // Y Axis Movement OLD
        if (!stop && !dontMoveY)
        {
            if (up)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y >= maxY)
                    up = false;
            }
            else
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y <= minY)
                    up = true;
            }
        }
        */
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "White" && target.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1
            || target.gameObject.name == "Player")
        {
            stop = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    /// <summary>
    /// Notify the helicopter that the Timeline animation finished, we can take over the control from Auto-pilot
    /// </summary>
    public void AnimationEnded()
    {

    }
}
