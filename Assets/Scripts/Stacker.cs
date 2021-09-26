using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacker : MonoBehaviour
{
    public GameObject stackObject;
    [SerializeField] float stackSpacing;
    private float lastStack = 1.0f;
    [SerializeField]
    private bool vertical;
    GameObject stackObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Stackable")
        {
            // disable collected object from the scene
            other.gameObject.SetActive(false);

            // Stack according to the orientation
            if (vertical)
            {
                /*
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += new Vector3(0.5f, 0, 0);
                //(However long you want to wait)
                transform.localScale = new Vector3(2, 1, 1);
                transform.position += new Vector3(0.5f, 0, 0);
                */
                transform.localScale += new Vector3(1, 0, 0);
                //transform.position += new Vector3(0.5f, 0, 0);
                //(However long you want to wait)
                //transform.localScale += new Vector3(1, 0, 0);
                //transform.position += new Vector3(0.5f, 0, 0);
                //stackObj = Instantiate(stackObject, transform.position + new Vector3(stackSpacing * lastStack, 0 , 0), Quaternion.EulerRotation(0f,0f, 0f));
            }
            else
            {
                stackObj = Instantiate(stackObject, transform.position + new Vector3(0, stackSpacing * lastStack, 0), Quaternion.EulerRotation(0f, 0f, 0f));
            }
            stackObj.transform.parent = gameObject.transform;
            lastStack++; 
            Debug.Log("Stackable collided");
        }
    }
}
