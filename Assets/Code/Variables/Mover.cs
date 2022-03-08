using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class Mover : MonoBehaviour
{
    private Touch touch;
    private float speedModifier;
    public StringVariable ThingName;


    // Start is called before the first frame update
    void Start()
    {
        speedModifier = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            // Index finger
            touch = Input.GetTouch(0);
            // If this is selected with TouchPhase.Began then move
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedModifier,
                    transform.position.y + touch.deltaPosition.y * speedModifier,
                    transform.position.z);
            }
        }
    }
}
