using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Material newMaterialRef;

    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action OnFloorTrigger;
    [SerializeField]
    GameManager.MissionType missionType;
    [SerializeField]
    float splashTimeInterval = 0.1f;
    int rand;

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag != "Player")
        {
            if (target.gameObject.tag == "red")
            {
                base.gameObject.GetComponent<Collider>().enabled = false;
                target.gameObject.GetComponent<MeshRenderer>().enabled = true;
                target.gameObject.GetComponent<MeshRenderer>().material = newMaterialRef;
                base.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.Impulse);
                //HeartsFun(target.gameObject);
                Destroy(base.gameObject, .5f);
                //print("Game Over");

                // UI Manager registers to the Floor Trigger to give visual feedback
                // but not always give some randomness
                rand = UnityEngine.Random.Range(0, 100);
                if (rand == 17)
                {
                    OnFloorTrigger += UIManager.instance.inGameView.ShowFeedbackTextGeneric;
                    // Trigger OnVolumeTrigger Event
                    OnFloorTrigger?.Invoke();
                    AudioManager.instance.PlaySFX(1);
                }
            }
            else if(target.gameObject.tag == "floor")
            {
                //Player.instance.ChangePlayerState(Player.PlayerStateType.THROW);
                //GameObject.Find("hitSound").GetComponent<AudioSource>().Play();
                base.gameObject.GetComponent<Collider>().enabled = false;
                GameObject gameObject = Instantiate(Resources.Load("splash3"), target.gameObject.transform, false) as GameObject;
                //gameObject.transform.parent = target.gameObject.transform;
                Destroy(gameObject, splashTimeInterval);
                target.gameObject.name = "color";
                target.gameObject.tag = "red";
                StartCoroutine(ChangeColor(target.gameObject));
                Debug.Log("Code reached ColorChanger else ================================");

                // Decrease number of tiles (in the first run, otherwise If collides with the same tile more than once, will count more times!!!)
                GameManager.instance.HandleMissionProgress((int)missionType);
            }
        }
    }

    IEnumerator ChangeColor(GameObject g)
    {
        // Wait for 0.1 second for our splash to disappear, then change the portion color
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Change Color: " + g.gameObject.name);
        g.gameObject.GetComponent<MeshRenderer>().enabled = true;
        g.gameObject.GetComponent<MeshRenderer>().material.color = BallManager.oneColor;
        Destroy(base.gameObject);
    }

    void HeartsFun(GameObject g)
    {
        int @int = PlayerPrefs.GetInt("hearts");
        if(@int == 1)
        {
            FindObjectOfType<BallHandler>().FailGame();
            FindObjectOfType<BallHandler>().HeartsLow();
        }
    }

}
