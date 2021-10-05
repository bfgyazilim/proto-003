using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager instance;

    [SerializeField]
    GameObject[] scoreParticles;
    [SerializeField]
    GameObject[] triggerParticles;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Crate.OnTriggerFinish += HandleTriggerFeedbackParticles;
        Crate.OnTriggerPlate += HandleTriggerFeedbackPoints;
        Crate.OnDie += HandleTriggerFailParticles;
    }

    public void HandleTriggerFailParticles()
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(triggerParticles[4], Player.instance.transform.position, Camera.main.transform.rotation);
    }

    void HandleTriggerFeedbackPoints(Crate crate)
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(triggerParticles[5], Player.instance.transform.position, Camera.main.transform.rotation);
    }

    public void HandleFeedbackParticles(Collectible collectible)
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(scoreParticles[0], collectible.transform.position, Camera.main.transform.rotation);
    }

    public void HandleHitFeedbackParticles()
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(scoreParticles[1], Player.instance.transform.position, Camera.main.transform.rotation);
    }

    public void HandleTriggerFeedbackParticles()
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(triggerParticles[0], Player.instance.transform.position, Camera.main.transform.rotation);
    }

    public void HandleTriggerFeedbackParticlesByOrder(int i)
    {
        // Look at camera (face to the camera to have always direct showup)
        Instantiate(triggerParticles[i], Player.instance.transform.position, Camera.main.transform.rotation);
    }
}
