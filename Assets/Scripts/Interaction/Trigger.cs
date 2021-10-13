using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    public float speed = 1.0f;
    public Color startColor;
    public Color endColor;
    public bool repeatable = false;
    float startTime;
    public bool switchOn;
    [SerializeField]
    Transform jointPoint;
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchOn)
        {
            if (!repeatable)
            {
                float t = (Time.time - startTime) * speed;
                GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
            }
            else
            {
                float t = (Mathf.Sin(Time.time - startTime) * speed);
                GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
            }

        }
    }
}   