using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeMovement : MonoBehaviour
{
    private bool grabbing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lr = gameObject.GetComponentInChildren<LineRenderer>();
        lr.startColor = Color.blue;
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //lr.SetWidth(0.08f, 0.08f);
        lr.startWidth = 0.08f;
        lr.endWidth = 0.08f;

        if(Input.GetMouseButtonDown(0))
        {
            grabbing = true;
        }

        if(Input.GetMouseButton(0))
        {
            lr.positionCount = 2;
            GameObject closest = FindNearest();

            if(grabbing)
            {
                lr.SetPosition(1, closest.transform.position);
                closest.GetComponentInChildren<HingeJoint2D>().connectedBody = gameObject.GetComponentInChildren<Rigidbody2D>();
                grabbing = false;
            }
            lr.SetPosition(0, transform.position);
        }

        if(Input.GetMouseButtonUp(0))
        {
            GameObject[] hinges;
            hinges = GameObject.FindGameObjectsWithTag("Hinge");
            lr.positionCount = 0;

            foreach (GameObject go in hinges)
            {
                go.GetComponentInChildren<HingeJoint2D>().connectedBody = null;
            }
        }
    }

    GameObject FindNearest()
    {
        GameObject[] hinges;
        hinges = GameObject.FindGameObjectsWithTag("Hinge");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in hinges)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }
}
