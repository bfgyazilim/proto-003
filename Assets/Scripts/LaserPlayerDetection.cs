using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlayerDetection : MonoBehaviour
{
    public GameObject player;
    Renderer renderer;
    public GameEnding gameEnding;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(renderer.enabled)
        {
            if(other.gameObject == player)
            {
                Player.instance.ChangePlayerStateToDie();
            }
        }
    }
}
