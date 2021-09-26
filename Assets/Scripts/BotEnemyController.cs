using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotEnemyController : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    bool died;
    public bool followPlayer;
    //public GameObject observerCone;

    private void Awake()
    {
        ActivateWaypoint();
    }
    /// <summary>
    /// 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {             
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

    /// <summary>
    /// 
    /// </summary>
    public void EnableBot()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ActivateWaypoint()
    {
        int rnd = Random.Range(0, 1);

        waypoints[0] = GameObject.Find("BotEnemyWaypoint1").transform;

        /*
        if (rnd == 0)
        {
            waypoints[0] = GameObject.Find("BotEnemyWaypoint1").transform;
        }
        else
        {
            waypoints[0] = GameObject.Find("BotEnemyWaypoint2").transform;
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        /* Do something else with wall, maybe jump or wait here to kill the Blue Bots
        if (other.gameObject.tag == "Wall")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            //GameObject go = Instantiate(bot, transform.position, Quaternion.identity);
            other.GetComponent<Destructible>().DestructWall();

            Debug.Log("Triggered Floor");
        }
        */
        if (other.gameObject.tag == "Prisoner")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            //GameObject go = Instantiate(bot, transform.position, Quaternion.identity);

            // Kill em'all in first contact, we are more crowded then they are!
            other.GetComponent<BotController>().Die();

            // I must die too
            Die();

            Debug.Log("Triggered Blue Bots");
        }
    }
}
