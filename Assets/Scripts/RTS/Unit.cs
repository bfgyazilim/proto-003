using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
	public UnitState state = UnitState.Idle;
	public UnitTemplate template;

	//references
	private NavMeshAgent navMeshAgent;
	private Animator animator;
	private SpriteRenderer selectionCircle;

	//private bool isSelected; //is the Unit currently selected by the Player
	private Unit targetOfAttack;
	private Unit[] hostiles;
	private float lastGuardCheckTime, guardCheckInterval = 1f;
	private bool isReady = false;

	public UnityAction<Unit> OnDie;

	// Patrolling variables
	public bool followPlayer, prepareLevelEnding;
	public Transform[] waypoints;
	int m_CurrentWaypointIndex;
	bool died;

	public GameObject anchor;
	public GameObject controlledBy;
	bool isWalkingTowards = false;
	bool sittingOn = false;
	bool redirectedToLastWaypoint;
	public GameObject character;
	[SerializeField] float lerpSpeed = 0.5f;
	public GameObject panel;

	// Events to notify other objects
	public UnityEvent onProgressionStatusEvent;

	/// <summary>
	/// 
	/// </summary>
	void Awake ()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		//selectionCircle = transform.Find("SelectionCircle").GetComponent<SpriteRenderer>();

		//Randomization of NavMeshAgent speed. More fun!
		float rndmFactor = navMeshAgent.speed * .15f;
		navMeshAgent.speed += Random.Range(-rndmFactor, rndmFactor);
	}

	/// <summary>
    /// 
    /// </summary>
	private void Start()
	{
		template = Instantiate<UnitTemplate>(template); //we copy the template otherwise it's going to overwrite the original asset!

		//Set some defaults, including the default state
		//SetSelected(false);
		//Guard();
		GoToAndSit(waypoints[0].position);
		// Initialize navmesh
		//navMeshAgent.SetDestination(waypoints[0].position);
		// Set states for the rigidbody to true, and colliders for the ragdoll to false
		//SetRigidbodyState(true);
		//SetColliderState(false);
	}



	/// <summary>
	/// 
	/// </summary>
	private void FixedUpdate()
	{
		AnimLerp();
	}

	/// <summary>
    /// Handle State Machine
    /// </summary>
	void Update()
	{
		//Little hack to give time to the NavMesh agent to set its destination.
		//without this, the Unit would switch its state before the NavMeshAgent can kick off, leading to unpredictable results
		if(!isReady)
		{
			isReady = true;
			return;
		}

		switch(state)
		{
			case UnitState.MoveToExit:
				if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + .1f)
				{
					Debug.Log("MoveToExit state in Update called");
					GoToAndSit(waypoints[0].position);
				}
				break;

			case UnitState.MovingToSpotSit:
				if (controlledBy == null)
				{
					if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
					{
						// So close to walk in, give management of further adjusting (Lerp) position and rotation to the current Anchor point manually!
						Debug.Log("Control given to anchor " + m_CurrentWaypointIndex);
						navMeshAgent.isStopped = true;
						navMeshAgent.velocity = Vector3.zero;
						controlledBy = anchor;						
					}
				}
				else
				{
					Debug.Log("Control in anchor");
					character.transform.rotation = anchor.transform.rotation;
					AutoWalkTowards();
				}
				break;

			case UnitState.MovingToSpotIdle:
				if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + .1f)
				{
					Stop();
				}
				break;

			case UnitState.MovingToSpotGuard:
				if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + .1f)
				{
					Guard();
				}
				break;

			case UnitState.MovingToTarget:
				//check if target has been killed by somebody else
				if(IsDeadOrNull(targetOfAttack))
				{
					Guard();
				}
				else
				{
					//Check for distance from target
					if(navMeshAgent.remainingDistance < template.engageDistance)
					{
						navMeshAgent.velocity = Vector3.zero;
						StartAttacking();
					}
					else
					{
						navMeshAgent.SetDestination(targetOfAttack.transform.position); //update target position in case it's moving
					}
				}

				break;

			case UnitState.Guarding:
				if(Time.time > lastGuardCheckTime + guardCheckInterval)
				{
					lastGuardCheckTime = Time.time;
					Unit t = GetNearestHostileUnit();
					if(t != null)
					{
						MoveToAttack(t);
					}
				}
				break;
			case UnitState.Attacking:
				//check if target has been killed by somebody else
				if(IsDeadOrNull(targetOfAttack))
				{
					Guard();
				}
				else
				{
					//look towards the target
					Vector3 desiredForward = (targetOfAttack.transform.position - transform.position).normalized;
					transform.forward = Vector3.Lerp(transform.forward, desiredForward, Time.deltaTime * 10f);
				}
				break;
		}

		//float navMeshAgentSpeed = navMeshAgent.velocity.magnitude;
		//animator.SetFloat("Speed", navMeshAgentSpeed * .05f);
	}
	/*
	/// <summary>
	/// 
	/// </summary>
	// Update is called once per frame
	void Update()
	{
		// If reached the level end point, stop the prisoner...
		if (prepareLevelEnding)
		{
			if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + 1f)
			{
				//navMeshAgent.isStopped = true;

				// edit
				targetOfAttack = null;
				isReady = false;

				navMeshAgent.isStopped = true;
				navMeshAgent.velocity = Vector3.zero;
				// end edit


				animator.SetBool("isWalking", false);
				animator.SetTrigger("Chicken");
				//Debug.Log("SetTrigger - Chicken called on Boy1");
				prepareLevelEnding = false;
			}
			else
			{
				//Debug.Log("SetTrigger - Dance NOT called on Boy1");

				//navMeshAgent.speed += .5f;
			}
		}
		else
        {
			if(controlledBy == null)
            {
				if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && m_CurrentWaypointIndex != 1)
				{
					m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
					navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
					// So close to walk in, give management of further adjusting (Lerp) position and rotation to the current Anchor point manually!
					Debug.Log("Control given to anchor " + m_CurrentWaypointIndex);
					navMeshAgent.isStopped = true;
					navMeshAgent.velocity = Vector3.zero;
					controlledBy = anchor;			
				}
            }
			else
            {
				Debug.Log("Control in anchor");
				character.transform.rotation = anchor.transform.rotation;
				AutoWalkTowards();
				//animator.SetTrigger("Walking");
			}
		}
	}
	*/
	private void AutoWalkTowards()
	{

		Vector3 targetDir;
		targetDir = new Vector3(anchor.transform.position.x - character.transform.position.x, 0.0f, anchor.transform.position.z - character.transform.position.z);
		Quaternion rot = Quaternion.LookRotation(targetDir);
		character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rot, 0.05f);
		// character.transform.Translate(Vector3.forward * 0.01f);

		//Debug.Log(Vector3.Distance(character.transform.position, anchor.transform.position));
		if (Vector3.Distance(character.transform.position, anchor.transform.position) < 0.8f)
		{

			print("Less than 0.6f");
			//animator.SetBool("isSitting", true);
			animator.SetBool("isWalking", false);
			animator.SetTrigger("SitClap");

			character.transform.rotation = anchor.transform.rotation;

			isWalkingTowards = false;
			sittingOn = true;
			// Sitting now, broadcast an event message, so that others can take actions accordingly!!!
			onProgressionStatusEvent.Invoke();
		}
	}

	void AnimLerp()
	{

		if (!sittingOn) return;

		if (Vector3.Distance(character.transform.position, anchor.transform.position) > 0.1f)
		{

			character.transform.rotation = Quaternion.Lerp(character.transform.rotation, anchor.transform.rotation, Time.deltaTime * lerpSpeed);
			character.transform.position = Vector3.Lerp(character.transform.position, anchor.transform.position, Time.deltaTime * lerpSpeed);
		}
		else
		{

			character.transform.rotation = anchor.transform.rotation;
			character.transform.position = anchor.transform.position;
		}
	}

	public void ExecuteCommand(AICommand c)
	{
		if(state == UnitState.Dead)
		{
			//already dead
			return;
		}

		switch(c.commandType)
		{
			case AICommand.CommandType.GoToAndIdle:
				GoToAndIdle(c.destination);
				break;

			case AICommand.CommandType.GoToAndGuard:
				GoToAndGuard(c.destination);
				break;

			case AICommand.CommandType.Stop:
				Stop();
				break;

			case AICommand.CommandType.AttackTarget:
				MoveToAttack(c.target);
				break;
			
			case AICommand.CommandType.Die:
				Die();
				break;
		}
	}

	/// <summary>
	/// Get out to the next Waypoint and wait!
	/// </summary>
	public void GoToExit()
	{
		state = UnitState.MoveToExit;
		controlledBy = null;
		navMeshAgent.SetDestination(waypoints[2].position);
		// So close to walk in, give management of further adjusting (Lerp) position and rotation to the current Anchor point manually!
		Debug.Log("Going  to exit, " + m_CurrentWaypointIndex);
		navMeshAgent.isStopped = false;
		sittingOn = false;
		redirectedToLastWaypoint = true;
		animator.SetBool("isWalking", true);
	}

	//move to a position and be idle
	private void GoToAndSit(Vector3 location)
	{
		state = UnitState.MovingToSpotSit;
		targetOfAttack = null;
		isReady = true;

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(location);
		animator.SetBool("isWalking", true);

		Debug.Log("GoToAndSit called");
	}

	//move to a position and be idle
	public void GoToAndIdle(Vector3 location)
	{
		state = UnitState.MovingToSpotIdle;
		targetOfAttack = null;
		isReady = false;

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(location);
		
		Debug.Log("GoToAndIdle called");
	}

	//move to a position and be idle
	public void GoToAndWaitThenGoToNext()
	{
		state = UnitState.MovingToSpotIdle;
		targetOfAttack = null;
		isReady = false;

		navMeshAgent.isStopped = false;
		sittingOn = false;
		animator.SetBool("isWalking", true);

		// Go to Desk
		navMeshAgent.SetDestination(waypoints[1].position);

		Debug.Log("GoToAndWaitThenGoToNext called");
	}

	//move to a position and be guarding
	private void GoToAndGuard(Vector3 location)
	{
		state = UnitState.MovingToSpotGuard;
		targetOfAttack = null;
		isReady = false;

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(location);
	}

	//stop and stay Idle
	private void Stop()
	{
		state = UnitState.Idle;
		targetOfAttack = null;
		isReady = false;

		navMeshAgent.isStopped = true;
		navMeshAgent.velocity = Vector3.zero;

		animator.SetBool("isWalking", false);
		animator.SetBool("isIdle", true);
	}

	//stop but watch for enemies nearby
	public void Guard()
	{
		state = UnitState.Guarding;
		targetOfAttack = null;
		isReady = false;

		navMeshAgent.isStopped = true;
		navMeshAgent.velocity = Vector3.zero;
	}

	//move towards a target to attack it
	private void MoveToAttack(Unit target)
	{
		if(!IsDeadOrNull(target))
		{
			state = UnitState.MovingToTarget;
			targetOfAttack = target;
			isReady = false;

			navMeshAgent.isStopped = false;
			navMeshAgent.SetDestination(target.transform.position);
		}
		else
		{
			//if the command is dealt by a Timeline, the target might be already dead
			Guard();
		}
	}

	//reached the target (within engageDistance), time to attack
	private void StartAttacking()
	{
		//somebody might have killed the target while this Unit was approaching it
		if(!IsDeadOrNull(targetOfAttack))
		{
			state = UnitState.Attacking;
			isReady = false;
			navMeshAgent.isStopped = true;
			StartCoroutine(DealAttack());
		}
		else
		{
			Guard();
		}
	}

	/// <summary>
    /// Unit waits at the spot for the given time
    /// </summary>
    /// <returns></returns>
	private IEnumerator WaitAtSpot()
    {
		yield return new WaitForSeconds(3f);
	}

	//the single blows
	private IEnumerator DealAttack()
	{
		while(targetOfAttack != null)
		{
			animator.SetTrigger("DoAttack");
			targetOfAttack.SufferAttack(template.attackPower);

			yield return new WaitForSeconds(1f / template.attackSpeed);

			//check is performed after the wait, because somebody might have killed the target in the meantime
			if(IsDeadOrNull(targetOfAttack))
			{
				animator.SetTrigger("InterruptAttack");
				break;

			}

			if(state == UnitState.Dead)
			{
				yield break;
			}

			//Check if the target moved away for some reason
			if(Vector3.Distance(targetOfAttack.transform.position, transform.position) > template.engageDistance)
			{
				MoveToAttack(targetOfAttack);
			}
		}


		//only move into Guard if the attack was interrupted (dead target, etc.)
		if(state == UnitState.Attacking)
		{
			Guard();
		}
	}

	//called by an attacker
	private void SufferAttack(int damage)
	{
		if(state == UnitState.Dead)
		{
			//already dead
			return;
		}

		template.health -= damage;

		if(template.health <= 0)
		{
			template.health = 0;
			Die();
		}
	}

	/*
	//called in SufferAttack, but can also be from a Timeline clip
	private void Die()
	{
		state = UnitState.Dead; //still makes sense to set it, because somebody might be interacting with this script before it is destroyed
		animator.SetTrigger("DoDeath");

		//Remove itself from the selection Platoon

		//GameManager.instance.RemoveFromSelection(this);

		SetSelected(false);
		
		//Fire an event so any Platoon containing this Unit will be notified
		if(OnDie != null)
		{
			OnDie(this);
		}

		//To avoid the object participating in any Raycast or tag search
		gameObject.tag = "Untagged";
		gameObject.layer = 0;

		//Remove unneeded Components
		Destroy(selectionCircle);
		Destroy(navMeshAgent);
		Destroy(GetComponent<Collider>()); //will make it unselectable on click
		Destroy(animator, 4f); //give it some time to complete the animation
		Destroy(this);
	}
	*/
	private bool IsDeadOrNull(Unit u)
	{
		return (u == null || u.state == UnitState.Dead);
	}

	private Unit GetNearestHostileUnit()
	{
		hostiles = GameObject.FindGameObjectsWithTag(template.GetOtherFaction().ToString()).Select(x => x.GetComponent<Unit>()).ToArray();

		Unit nearestEnemy = null;
		float nearestEnemyDistance = 1000f;
		for(int i=0; i<hostiles.Count(); i++)
		{
			if(IsDeadOrNull(hostiles[i]))
			{
				continue;
			}

			float distanceFromHostile = Vector3.Distance(hostiles[i].transform.position, transform.position);
			if(distanceFromHostile <= template.guardDistance)
			{
				if(distanceFromHostile < nearestEnemyDistance)
				{
					nearestEnemy = hostiles[i];
					nearestEnemyDistance = distanceFromHostile;
				}
			}
		}

		return nearestEnemy;
	}

	public void SetSelected(bool selected)
	{
		//Set transparency dependent on selection
		Color newColor = selectionCircle.color;
		newColor.a = (selected) ? 1f : .3f;
		selectionCircle.color = newColor;
	}

	public enum UnitState
	{
		Idle,
		Guarding,
		Attacking,
		MovingToTarget,
		MovingToSpotIdle,
		MovingToSpotGuard,
		MovingToSpotSit,
		MoveToExit,
		Dead,
	}

	private void OnDrawGizmos()
	{
		if(navMeshAgent != null
			&& navMeshAgent.isOnNavMesh
			&& navMeshAgent.hasPath)
		{
			Gizmos.DrawLine(transform.position, navMeshAgent.destination);
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
			if (collider.gameObject.name != "PointOfView")
			{
				collider.enabled = state;
			}
			else
			{
				if (died)
				{
					collider.enabled = false;
					//observerCone.SetActive(false);
				}
			}
		}

		GetComponent<Collider>().enabled = !state;
	}
}