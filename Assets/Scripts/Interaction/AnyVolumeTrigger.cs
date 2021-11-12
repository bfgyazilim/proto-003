using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class AnyVolumeTrigger : MonoBehaviour
{
    [SerializeField]
    Player.PlayerStateType state;
    [SerializeField] UnityEvent OnVolumeEnterEvent, OnVolumeExitEvent;

    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action<int> OnVolumeTrigger;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Crate" && other.gameObject.name == "Player")
        if (other.gameObject.name == "Player")
        {
            //// Trigger OnVolumeTrigger Unity Event for ADDITIONAL editor setup of gameobjects            
                OnVolumeEnterEvent.Invoke();

            // Optional to change Player state on AnyVolume that is triggered this event!!!
            // Player.instance.ChangePlayerState(state);
            // Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);
            // GameManager.instance.OffsetCamera();
            
            Debug.Log("Any-VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Crate" && other.gameObject.name == "Player")
        if (other.gameObject.name == "Player")
        {
            //// UI Manager registers to the OnDie event via inGameView Canvas to
            //// show feedback on screen...
            //OnVolumeTrigger += UIManager.instance.inGameView.ShowFeedbackTextGeneric;

            //// Trigger OnVolumeTrigger Event
            //OnVolumeTrigger?.Invoke();
            OnVolumeExitEvent.Invoke();

            //Player.instance.ChangePlayerState(state);
            //Player.instance.ChangePlayerState(Player.PlayerStateType.WALKING);
            // Test for Mission08 Camera Offset - From GameManager
            //GameManager.instance.DeOffsetCamera();

            Debug.Log("Any-VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
        }
    }
}
