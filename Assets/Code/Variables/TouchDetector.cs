using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RoboRyanTron.Unite2017.Variables;

public class TouchDetector : MonoBehaviour
{
    private Vector2 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;
    public UnityEvent FingerDownEvent, FingerUpEvent;

    private void Update()
    {
        if (fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
            FingerDownEvent.Invoke();
            Debug.Log("OnTouchAndHold Event Raised");
        }

        if (fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
            FingerUpEvent.Invoke();
        }
    }
}
