using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class VolumeTrigger : MonoBehaviour
{
    [SerializeField]
    Player.PlayerStateType state;
    [SerializeField] UnityEvent OnVolumeEnterEvent, OnVolumeExitEvent;

    [SerializeField]
    GameManager.MissionType missionType;
    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action<int> OnVolumeTrigger;

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Crate" && other.gameObject.name == "Player")
        if (other.gameObject.name == "Player")
        {
            // Invoke for editor setup of gameobjects on and off             
            //OnVolumeTrigger += UIManager.instance.inGameView.ShowFeedbackTextGeneric;
            // Mission complete triggered, so GameManager knows about the game state, and Updates
            //OnVolumeTrigger += GameManager.instance.HandleMissionProgress;
            //OnVolumeTrigger?.Invoke((int)missionType);

            //// Trigger OnVolumeTrigger Unity Event for ADDITIONAL editor setup of gameobjects            
            OnVolumeEnterEvent.Invoke();

            // If you have enough resources for this mission
            if(GameManager.instance.GetMissionStatus())
            {
                if(missionType == GameManager.MissionType.MISSION0)
                {
                    OnVolumeTrigger += GameManager.instance.HandleMissionProgress;
                    OnVolumeTrigger?.Invoke((int)missionType);
                    //float unitOffsetX = 0, unitOffsetY = -0.5f, unitOffsetZ = 3;
                    //WorldController.instance.GenerateBlocks(transform.position.x + unitOffsetX, transform.position.y + unitOffsetY, transform.position.z + unitOffsetZ);
                }
                    //Player.instance.ChangePlayerState(state);
                    Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);

                // Test for Mission08 Camera Offset - From GameManager
                GameManager.instance.OffsetCamera();
            }
            Debug.Log("VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
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
            Player.instance.ChangePlayerState(Player.PlayerStateType.WALKING);

            // Test for Mission08 Camera Offset - From GameManager
            GameManager.instance.DeOffsetCamera();

            Debug.Log("VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
        }
    }
}
