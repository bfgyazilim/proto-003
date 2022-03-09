using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class TouchMover : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    public void Move(Vector3 moveDirection)
    {
        targetPos += moveDirection;
    }

    public void Move(StringVariable direction)
    {
        if(direction.Value == "Up")
        {
            targetPos += Vector3.forward;
        }
        else if(direction.Value == "Left")
        {
            targetPos += Vector3.left;
        }
        else if (direction.Value == "Right")
        {
            targetPos += Vector3.right;
        }
        else if (direction.Value == "Down")
        {
            targetPos += Vector3.back;
        }

        // Turn face to the target position
        Debug.DrawLine(transform.position, targetPos);
        transform.forward = Vector3.Normalize(targetPos - transform.position);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
