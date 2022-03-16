using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed;
    public Vector3 eulerAngles;
    [SerializeField] bool rotateEnabled;

	void Update ()
    {
        if(rotateEnabled)
        transform.Rotate(eulerAngles * speed * Time.deltaTime);
	}

    public void StartRotation(bool rotEnabled)
    {
        if(!rotateEnabled)
        {
            rotateEnabled = true;
            speed = -speed;
        }
    }

    public void StopRotation()
    {
        if (rotateEnabled)
        {
            rotateEnabled = false;            
        }
    }
}
