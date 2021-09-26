using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitchDeactivation : MonoBehaviour
{
    public GameObject laser;
    public Material unlockedMat, unlockedMatInside;
    [SerializeField] private Renderer renderer, subRenderer;
    private GameObject player;
    public GameObject[] prisoner;
    private bool deactivatedSwitch;

    public GameObject[] prisonerParticles;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LaserDeactivation();
            Debug.Log("Player Deactivation");
        }
    }

    void LaserDeactivation()
    {
        laser.SetActive(false);
        // Set in ad out of the button color
        renderer.material = unlockedMat;
        subRenderer.material = unlockedMatInside;
        gameObject.GetComponent<AudioSource>().Play();

        if (!deactivatedSwitch)
        {
            // Set every prisoner in the array to follow player
            foreach (GameObject prisonObject in prisoner)
            {
                prisonObject.GetComponent<PrisonerController>().followPlayer = true;
                // For every rescued prisoner, add to the player, at level end X multiply coins
                // by factor..
                Player.instance.RescuedPrisoners++;
            }
            deactivatedSwitch = true;

            // Show some effects for saving prisoners...
            Instantiate(prisonerParticles[0], transform.position, Quaternion.identity);
            Score.instance.ShowBonusText(transform.position);
        }
    }
}
