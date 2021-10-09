using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;
    [SerializeField]
    GameObject[] foods;
    int currentfoodIndex;

    // Invoke(Trigger) this event on Player contact
    // Collector and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public event Action<SpeechBubble> OnTriggerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "<size=8>New tiles!! <sprite=2> <sprite=2>";

        //InitializeOrder();
    }

    void InitializeOrder()
    {
        int index = UnityEngine.Random.Range(0, foods.Length);
        currentfoodIndex = index;
        //Instantiate(foods[index], transform.position + new Vector3(0,0,-8f), Quaternion.identity, transform);
        Instantiate(foods[index], transform.position + new Vector3(0, 0, -0.1f), Quaternion.identity, transform);

    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.LookAt(Camera.main.transform);        
    }

    public void HandlePoints(Crate crate)
    {
        //Debug.Log("********** " + transform.parent.parent.name + "********** ");
        if(crate.name.StartsWith("Ham") && currentfoodIndex == 0)
        {
            text.text = "<size=8>Thanks!! <sprite=2> <sprite=2>";
            Debug.Log("Good food ordered well done !!!!");
        }
        else if (crate.name.StartsWith("Ca") && currentfoodIndex == 1)
        {
            text.text = "<size=8>Thanks!! <sprite=12> <sprite=12>";
            Debug.Log("Good food ordered well done !!!!");
        }
        else if (crate.name.StartsWith("Ic") && currentfoodIndex == 2)
        {
            text.text = "<size=8>Thanks!! <sprite=13> <sprite=13>";
            Debug.Log("Good food ordered well done !!!!");
        }
        else
        {
            text.text = "<size=8>Wrong Order <sprite=11>";
            Debug.Log("Not Matched food ordered!");
        }        
    }
}
