using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Crate : MonoBehaviour
{
    // Add Event here for OnHit    
    public static event Action OnHit, OnDie;
    public static event Action<Crate> OnTriggerPlate;
    // Add Event here for OnTriggerFinish (Invoked when the player hits the FINISH trigger)    
    public static event Action OnTriggerFinish;
    public TextMeshPro capitalText;

    private void Start()
    {
        
    }


    /// <summary>
    /// If no crates left, Player can not continue to the game, there is no main object to stack above.
    /// </summary>
    void FailWithoutCrates()
    {
        if (Player.instance.GetChildCount() <= 0)
        {
            // UI Manager registers to the OnDie event via inGameView Canvas to
            // show feedback on screen...
            OnDie += FXManager.instance.HandleTriggerFailParticles;
            // Hit Sound
            AudioManager.instance.PlaySFX(1);
            // Invoke Die to broadcast for other classes, example (FX for splash effects, and
            // Score etc, GameManager)
            OnDie?.Invoke();

            // You DIE but come another time!!!
            GameManager.instance.GameOver();
        }
    }

    /// <summary>
    /// Check IF Crate collides with particular objects
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Invoke Hit to broadcast for other classes, example (FX for splash effects, and
            // Score etc, GameManager)
            OnHit?.Invoke();

            // In hamburger prefab model is inside a hamburger container, so change to parent.parent to deparent it from the Holder
            transform.parent.parent = null;

            // Crate model is not inside a container, so it's parent is Holder, we can deparent it below.
            //transform.parent = null;


            FailWithoutCrates();

            Debug.Log("Crate Invoked OnHit");
        }
        if (other.gameObject.tag == "Plate")
        {
            // Register to the OnTriggerPlate event on Crate (For the object whom is the collider)
            SpeechBubble sb = other.transform.parent.GetComponentInChildren<SpeechBubble>();
            OnTriggerPlate += sb.HandlePoints;
            // Coin Sound
            AudioManager.instance.PlaySFX(0);
            // Invoke Hit to broadcast for other classes, example (FX for splash effects, and
            // Score etc, GameManager)
            OnTriggerPlate?.Invoke(this);

            transform.parent.parent = null;

            FailWithoutCrates();

            Debug.Log("Crate Invoked OnTriggerPlate");
        }
        if (other.gameObject.tag == "Killer")
        {
            // UI Manager registers to the OnDie event via inGameView Canvas to
            // show feedback on screen...
            OnDie += FXManager.instance.HandleTriggerFailParticles;
            // Hit Sound
            AudioManager.instance.PlaySFX(1);
            // Invoke Die to broadcast for other classes, example (FX for splash effects, and
            // Score etc, GameManager)
            OnDie?.Invoke();

            // Fall apart player we died!!!
            Player.instance.DieCameraAnimation();

            // You DIE but come another time!!!
            GameManager.instance.GameOver();
        }
        if (other.gameObject.tag == "Finish")
        {
            // Set the player to levelFinished, so that the movement logic and etc, notice the change
            Player.instance.levelFinished = true;
            // Level Finished Sound
            AudioManager.instance.PlaySFX(2);
            // Invoke Hit to broadcast for other classes, example (FX for splash effects, and
            // Score etc, GameManager)
            OnTriggerFinish?.Invoke();

            // Wait 10 sec, then enable UI Manager WinView
            StartCoroutine(GameManager.instance.WaitForGivenTime(2f));
        }
    }
}
