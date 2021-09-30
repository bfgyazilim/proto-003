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
    public bool followPlayer, prepareLevelEnding;
    //public GameObject observerCone;
    Animator anim;
    /// <summary>
    /// 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        navMeshAgent.SetDestination(waypoints[0].position);
        SetRigidbodyState(true);
        SetColliderState(false);
    }

    /// <summary>
    /// 
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        // If reached the level end point, stop the prisoner...
        if(prepareLevelEnding)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = true;
                anim.SetTrigger("Dance");

            }
            else
            {
                navMeshAgent.speed += 1f;
            }
        }
        else
        {
            if (!followPlayer)
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
    }

    /// <summary>
    /// Set Destination to the given waypoint
    /// </summary>
    /// <param name="waypoint"></param>
    public void ChangeDestination(Transform waypoint)
    {
        navMeshAgent.SetDestination(waypoint.transform.position);
        prepareLevelEnding = true;
    }

    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    void Explode()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddExplosionForce(50f, transform.position, 10f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
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
