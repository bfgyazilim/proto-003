using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    bool died;
    public GameObject observerCone;

    // Randomly spawn goods when enemy dies...
    public GameObject[] collectiblePrefabs;
    Vector3 spawnPos;

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
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void SpawnCollectibles()
    {
        int index = Random.Range(0, collectiblePrefabs.Length);
        if (index == 0)
        {
            spawnPos = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
        else
        {
            spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        Instantiate(collectiblePrefabs[index], spawnPos, Quaternion.identity);
    }

    public void Die()
    {
        // Stuff that happens when enemy dies
        //Destroy(gameObject, 3f);
        Debug.Log("Player killed Ragdoll");

        died = true;

        SpawnCollectibles();

        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        //SetRigidbodyState(false);
        //SetColliderState(true);

        StartCoroutine(Explode());

        //GetComponent<Animator>().enabled = false;
        //navMeshAgent.isStopped = true;
    }

    IEnumerator Explode()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(.2f);

        SetRigidbodyState(false);
        SetColliderState(true);

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddExplosionForce(500f, transform.position, 50f);
        }

        GetComponent<Animator>().enabled = false;
        navMeshAgent.isStopped = true;

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
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
                    observerCone.SetActive(false);
                }
            }
        }

        GetComponent<Collider>().enabled = !state;
    }
}
