using UnityEngine;
using UnityEngine.Events;

public class ActivateObject : MonoBehaviour
{
    public UnityEvent onPressedEvent;

    public string colliderTag;   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPressedEvent.Invoke();
            Debug.Log("Player onPressedEvent Invoked");
        }
    }
}
