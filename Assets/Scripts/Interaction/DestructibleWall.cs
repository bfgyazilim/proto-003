using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructibleWall : MonoBehaviour
{
    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action OnDestructProgress;

    [SerializeField]
    private Transform pfWallBroken;

    [SerializeField]
    float damageAmount = 100;

    float totalHp;
    static float remainingHp = 400.0f;
    bool zeroHp;

    private static int walltasksInMission = 4;

    /// <summary>
    /// UIManager Registers to the OnDestructComplete evet (Triggered when the object is destructed)
    /// </summary>
    private void OnEnable()
    {
        //OnDestructProgress += UIManager.instance.inGameView.ShowFeedbackTextGeneric;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDestructProgress += UIManager.instance.inGameView.ShowFeedbackTextGeneric;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision target)
    {
        if (target.gameObject.tag == "PlayerSaw")
        {
            Debug.Log("PlayerSaw collided with wall");

            if (remainingHp > 0)
            {
                remainingHp -= damageAmount;
                AudioManager.instance.PlaySFX(3);
                // Trigger OnDestructComplete Event
                OnDestructProgress?.Invoke();
            }
            else
            {
                zeroHp = true;
                Instantiate(pfWallBroken, transform.position, transform.rotation);
                Destroy(gameObject);
                Debug.Log("Destroyed!");
                if(transform.gameObject.tag == "Tree")
                {
                    // Decrease number of tasks remaining to complete the current mission
                    GameManager.instance.HandleMissionProgress((int)GameManager.MissionType.DESTRUCTTREES);
                    Debug.Log("Tree object collided with: " + target.gameObject.name + " Player State: " + Player.instance.state);

                }
                else
                {
                    // Decrease number of tasks remaining to complete the current mission
                    GameManager.instance.HandleMissionProgress((int)GameManager.MissionType.DESTRUCTWALLS);
                    Debug.Log("Wall object collided with: " + target.gameObject.name + " Player State: " + Player.instance.state);
                }
            }
        }
    }
}
