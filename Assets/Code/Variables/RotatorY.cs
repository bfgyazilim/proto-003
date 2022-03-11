using UnityEngine;
using System.Collections;

public class RotatorY : MonoBehaviour
{
	[SerializeField]
	private float turnSpeed = 1.0f;

	// Before rendering each frame..
	void Update () 
	{
		// Rotate the game object that this script is attached to by 15 in the X axis,
		// 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
		// rather than per frame.
		transform.Rotate (new Vector3 (0f, 30, 0f) * Time.deltaTime * turnSpeed);
	}
}	