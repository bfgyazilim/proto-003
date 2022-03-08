using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class Mover : MonoBehaviour
{
    private Touch touch;
    private float speedModifier;
    public StringVariable ThingName;

    // Start is called before the first frame update
    void Start()
    {
        speedModifier = 0.01f;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    hit.collider.GetComponent<MeshRenderer>().material.color = newColor;
                    // Store the name of the sekected object to the SO Variable as a StringVariable
                    ThingName.Value = hit.collider.gameObject.name;
                    Debug.Log("Changed color" + hit.collider.gameObject.name);
                }
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    hit.collider.GetComponent<MeshRenderer>().material.color = newColor;
                }
            }
        }
#endif

        if(Input.touchCount > 0)
        {
            // Index finger
            touch = Input.GetTouch(0);
            // If this is selected with TouchPhase.Began then move
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedModifier,
                    transform.position.y + touch.deltaPosition.y * speedModifier,
                    transform.position.z);
            }
        }
    }
}
