using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RoboRyanTron.Unite2017.Variables;

public class SwipeDetection : MonoBehaviour
{
    private Vector2 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;
    public StringVariable SwipeDirection;
    public UnityEvent SwipeUpEvent;
    public UnityEvent SwipeDownEvent;
    public UnityEvent SwipeLeftEvent;
    public UnityEvent SwipeRightEvent;
    public UnityEvent SwipeEndedEvent;

    private void Update()
    {
        if(fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }

        if(fingerDown)
        {
            if(Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                Debug.Log("Swipe up");
                SwipeDirection.Value = "Up";
                SwipeUpEvent.Invoke();
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                Debug.Log("Swipe down");
                SwipeDirection.Value = "Down";
                SwipeDownEvent.Invoke();
            }
            else if(Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                Debug.Log("Swipe left");
                SwipeDirection.Value = "Left";
                SwipeLeftEvent.Invoke();
            }
            else if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                Debug.Log("Swipe right");
                SwipeDirection.Value = "Right";
                SwipeRightEvent.Invoke();
            }
        }

        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            Debug.Log("Swipe ended");
            fingerDown = false;
            SwipeEndedEvent.Invoke();
            SwipeDirection.Value = "none";
        }
    }

}
