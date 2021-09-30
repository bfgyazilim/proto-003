using UnityEngine;
using System.Collections;

public class RotatorFixed : MonoBehaviour
{

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        transform.Rotate(new Vector3(-90f, 0, 0f));
    }
    // Before rendering each frame..
    void Update()
    {
        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        transform.Rotate(new Vector3(0f, 0, 30f) * Time.deltaTime);
    }
}