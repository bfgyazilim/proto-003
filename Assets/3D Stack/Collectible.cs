using System;
using UnityEngine;
using TMPro;
public class Collectible : MonoBehaviour
{
    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action<Collectible> OnPickup;
    public event Action<Collectible> OnDrop;

    // Random capital letter of a city in the world
    //public string capitalText;
    GameObject textobj;
    //public TextMeshPro capitalText;

    private void Start()
    {
        //textobj = this.gameObject.transform.GetChild(0).gameObject;
        //capitalText = textobj.GetComponent<TextMeshPro>();
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Crate" && other.gameObject.name == "Player")
        if (other.gameObject.name == "Player")
        {
            // UI Manager registers to the OnDie event via inGameView Canvas to
            // show feedback on screen...
            OnPickup += UIManager.instance.inGameView.ShowFeedbackTextForCollectible;

            // Player registers to the collectible for stacking it!!!
            if (transform.gameObject.tag == "plank")
            {            
                OnPickup += Player.instance.HandleStacking;
            }

            // Trigger OnPickup Event
            OnPickup?.Invoke(this);

            gameObject.SetActive(false);

            Debug.Log("Crate Invoked Collectible->OnTriggerEnter");
        }

        //Debug.Log("Collectible collided with: " + other.gameObject.name);
    }
}