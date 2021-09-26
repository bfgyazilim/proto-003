using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            Debug.Log("Collision velocity Magnitude:" + collision.relativeVelocity.magnitude);
            audioSource.Play();
            DestructObject();
        }
            
    }

    private void DestructObject()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);

    }
    private void OnMouseDown()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
