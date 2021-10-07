using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTrigger : MonoBehaviour
{
    [SerializeField]
    Player.PlayerStateType state;

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
            //Player.instance.ChangePlayerState(state);
            Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);
            Debug.Log("VolumeTrigger collided with: " + other.gameObject.name + "Player State: " + Player.instance.state);
        }
    }
}
