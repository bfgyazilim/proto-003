using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrisonerController : MonoBehaviour
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
        navMeshAgent.SetDestination(waypoints[0].position);
        SetRigidbodyState(true);
        SetColliderState(false);
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
}
