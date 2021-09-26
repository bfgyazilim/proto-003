using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyController : MonoBehaviour
{
    Animator animator;
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    bool died;
    public static AllyController instance;
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        waypoints[0] = GameObject.Find("Waypoint").transform;
        animator = GetComponent<Animator>();//need this...
        animator.SetTrigger("DynIdle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            navMeshAgent.SetDestination(waypoints[0].position);
            Debug.Log("Ally triggered by Player");
            animator.SetBool("isRunning", true);
        }
    }
}
