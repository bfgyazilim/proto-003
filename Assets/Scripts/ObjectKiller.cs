using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectKiller : MonoBehaviour
{

    public UnityEvent onCollisionEvent;

    public string colliderTag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ragdoll")
        {
            Debug.Log("Ragdoll Hit by Cannon Ball");
            //DestroyObject(other.gameObject, .3f);
            collision.gameObject.GetComponent<EnemyController>().Die();
        }

        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(colliderTag))
        {
            if (onCollisionEvent != null)
            {
                Debug.Log("Target collided with Player");
                onCollisionEvent.Invoke();
            }
        }

        Debug.Log("Collision");
    }
    */
}
