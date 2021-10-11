using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class VolumeTrigger : MonoBehaviour
{
    [SerializeField]
    Player.PlayerStateType state;
    [SerializeField] UnityEvent OnVolumeEnterEvent;

    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action OnVolumeTrigger;


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
            //// UI Manager registers to the OnDie event via inGameView Canvas to
            //// show feedback on screen...
            //OnVolumeTrigger += UIManager.instance.inGameView.ShowFeedbackTextGeneric;

            //// Trigger OnVolumeTrigger Event
            //OnVolumeTrigger?.Invoke();
            OnVolumeEnterEvent.Invoke();

            //Player.instance.ChangePlayerState(state);
            Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);
            Debug.Log("VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
        }
    }
}
