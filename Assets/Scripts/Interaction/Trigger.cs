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
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            // Change player state to carrying something and jogging
            Player.OnTriggerAttach += Player.instance.AttachObject;
            Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);
            // Parent object to player
            transform.SetParent(jointPoint, false);            
            transform.rotation = jointPoint.transform.rotation;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;


            //transform.parent = jointPoint;
            //transform.position = jointPoint.transform.position;
            // switch material change animation to give feedback to the user
            switchOn = true;
        }      
        else if (other.gameObject.name == "Drop Plane")
        {
            transform.parent = null;
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }
     */
 }
   