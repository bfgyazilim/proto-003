using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetter : MonoBehaviour
{
    public GameObject gameObj;
    public Transform oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPosition()
    {
        gameObj.transform.position = oldPosition.position + new Vector3(Random.Range(0,2), oldPosition.position.y, oldPosition.position.z);
        Debug.Log("Position reset");
    }
}
