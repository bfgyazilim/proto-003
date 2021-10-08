using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIUtils : MonoBehaviour
{
    Camera cam;
    public int screenX, screenY;

    public static UIUtils instance;
    public Transform target;

    void Start()
    {
        instance = this;

        cam = GetComponent<Camera>();

        Vector3 screenPos = cam.WorldToScreenPoint(Player.instance.transform.position);
        Debug.Log("Player is " + screenPos.x + " X " + screenPos.y + " Y");

        screenPos = cam.WorldToScreenPoint(target.transform.position);
        Debug.Log("Target is " + screenPos.x + " X " + screenPos.y + " Y");
    }

    void Update()
    {
        //Vector3 screenPos = cam.WorldToScreenPoint(Player.instance.transform.position);
        //Debug.Log("Player is " + screenPos.x + " X " + screenPos.y + " Y");

        //screenPos = cam.WorldToScreenPoint(target.transform.position);
        //Debug.Log("Target is " + screenPos.x + " X " + screenPos.y + " Y");
    }

    public Vector3 GetScreenPositionOfObject(Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);

        return screenPos;
    }

    public float GetScreenXPositionOfObject(Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);

        return screenPos.x;
    }

    public float GetScreenYPositionOfObject(Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);

        return screenPos.y;
    }
}
