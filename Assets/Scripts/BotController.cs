using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    bool died;
    public bool followPlayer;
    //public GameObject observerCone;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(waypoints[0].position);
        SetRigidbodyState(true);
        SetColliderState(false);
        ActivateWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(!followPlayer)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    public void Die()
    {
        // Stuff that happens when enemy dies
        //Destroy(gameObject, 3f);
        GetComponent<Animator>().enabled = false;
        navMeshAgent.isStopped = true;

        died = true;

        SetRigidbodyState(false);
        SetColliderState(true);

        Explode();
    }

    void Explode()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddExplosionForce(50f, transform.position, 10f);
        }
    }

    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.name != "PointOfView") {
                collider.enabled = state;
            }
            else
            {
                if(died)
                {
                    collider.enabled = false;
                    //observerCone.SetActive(false);
                }
            }
        }

        GetComponent<Collider>().enabled = !state;
    }

    public void EnableBot()
    {
        gameObject.SetActive(true);
    }

    public void ActivateWaypoint()
    {
        int rnd = Random.Range(0, 1);
        if(rnd == 0)
        {
            waypoints[0] = GameObject.Find("BotWaypoint1").transform;
        }
        else
        {
            waypoints[0] = GameObject.Find("BotWaypoint2").transform;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.tag == "Wall")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            //GameObject go = Instantiate(bot, transform.position, Quaternion.identity);
            other.GetComponent<Destructible>().DestructWall();

            Debug.Log("Triggered Floor");
        }
        */
        // Now player has finished jumping, come to level end
        if (other.transform.tag == "TowerE")
        {
            /*
            // Give extra coins for every rescued prisoner, multiply by prisoner count..
            if (RescuedPrisoners > 0)
            {
                rescueText.text = "Rescued " + RescuedPrisoners + " Hostages";

                Score.instance.MultiplyScore(RescuedPrisoners + 1);
            }
            // turn face to the user for dancing...
            transform.rotation = Quaternion.Euler(0, 180.0f, 0);
            anim.SetBool("isDancing", true);

            // call the helicopter...
            helicopter.GetComponent<Animator>().enabled = true;

            //GetComponent<Rigidbody>().velocity = Vector3.zero;

            playerWin = true;

            */

            PlayerController.instance.ChangePlayerStateToWin();
        }
    }


}
