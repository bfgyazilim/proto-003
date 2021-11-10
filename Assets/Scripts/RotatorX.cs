using UnityEngine;
using System.Collections;

public class RotatorX : MonoBehaviour
{    
	bool limitDelta;
	float currentAngle;
    float maxAngle;
    
    public float speed;
    [SerializeField]
    private float minX, maxX;

    public bool right, dontMove;
    private bool stop;

	// Before rendering each frame..
	void Update () 
	{
        /*
		if(limitDelta && currentAngle > maxAngle)
        {
			// Rotate the game object that this script is attached to by 15 in the X axis,
			// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
			// rather than per frame.
			transform.Rotate(new Vector3(currentAngle, 0f, 0f));
			currentAngle -= Time.deltaTime * turnSpeed;
			Debug.Log(currentAngle);
		}
		else
        {
			currentAngle = 0;
			limitDelta = false;
        }
        */
        // Came from MoveUpandDownX adapt same logic to the rotator code to limit the angle and go backwards
        if (!stop && !dontMove)
        {
            //Debug.Log(currentAngle);

            if (right)
            {
                currentAngle += Time.deltaTime * speed;
                transform.Rotate(new Vector3(currentAngle, 0f, 0f));

                //transform.position += Vector3.forward * speed * Time.deltaTime;
                if (currentAngle >= maxX)
                {
                    right = false;
                    currentAngle = 0;
                }
            }
            else
            {
                currentAngle -= Time.deltaTime * speed;
                transform.Rotate(new Vector3(currentAngle, 0f, 0f));

                //transform.position += Vector3.back * speed * Time.deltaTime;
                if (currentAngle <= minX)
                {
                    right = true;
                    currentAngle = 0;
                }
            }
        }
    }
}