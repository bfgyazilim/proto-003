using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoveZ : MonoBehaviour
{
    Vector3 randomVector;
    public float animSpeed;
    float currentAccumulation;
    private Vector3 startPosition;
    public float rangeStart, rangeEnd;
    bool isUp, isDown, move;
    int count;
    public float accumulation;
    public bool X;
    public bool Y;
    public bool Z;

    bool waitPeriod;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(randomVector.y);
        currentAccumulation += Time.deltaTime;

        if (currentAccumulation < accumulation)
        {
            if(X)
            {
                // move spikes at speed
                startPosition.x += animSpeed;
                startPosition.y = transform.position.y;
                startPosition.z = transform.position.z;
            }
            if(Y)
            {
                // move spikes at speed
                startPosition.y += animSpeed;
                startPosition.x = transform.position.x;
                startPosition.z = transform.position.z;
            }
            if(Z)
            {
                // move spikes at speed
                startPosition.z += animSpeed;
                startPosition.y = transform.position.y;
                startPosition.x = transform.position.x;
            }

            transform.position = startPosition;
        }
        else
        {
            currentAccumulation = 0f;
            animSpeed *= -1f;
        }
    }
}
