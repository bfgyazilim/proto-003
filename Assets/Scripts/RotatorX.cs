using UnityEngine;
using System.Collections;

public class RotatorX : MonoBehaviour
{
	[SerializeField]
	private float turnSpeed = 1.0f;
	[SerializeField]
	bool limitDelta;
	float currentAngle;
	[SerializeField]
    float maxAngle;

	// Before rendering each frame..
	void Update () 
	{
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
	}
}	