using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Misc")]

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask groundLayer, playerLayer;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rover roverManager;

    private int[] choices = { 1, 2 };

    private int randomChoice;

    [Header("Sounds")]
    [SerializeField] private AudioSource footstep;

    [Header("Tavelling")]
    [SerializeField]
    private List<Transform>destinations;
    private Transform currentDestination;
    private int randInt;
    
    bool destinationPointSet;
    bool travelling;

    [Header("Patrolling")]
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float minPatrolTime;
    [SerializeField]
    private float maxPatrolTime;
    [SerializeField]
    private float minPatrolDist;
    [SerializeField]
    private float maxPatrolDist;
    
    float patrolTime;
    bool patrolPointSet = false;
    bool isPatrolling = false;
    Vector3 patrolPoint;

    [Header("Idle")]

    [SerializeField]
    private float minIdleTime;

    [SerializeField]
    private float maxIdleTime;

    [Header("Chase")]
    [SerializeField]
    private float chaseSpeed;

    [Header("Attacking")]
    
    [SerializeField]
    private bool canResetAttack;
    [SerializeField]
    private float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]

    [HideInInspector]
    public bool playerIsDead;
    [SerializeField] private Vector3 positionOffset;
    public float sightRange;
    public float attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private float idleTime;

    private void Awake()
    {
        playerIsDead = false;
        agent = GetComponent<NavMeshAgent>();
        travelling = true;
    }

    private void Update()
    {
        Vector3 spherePos = transform.position + positionOffset;
        playerInSightRange = Physics.CheckSphere(spherePos, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(spherePos, attackRange, playerLayer);
        
        if (!playerInSightRange && !playerInAttackRange)
        {
            agent.speed = patrolSpeed;
            Travel();
            
            if (isPatrolling)
            {
                patrolling();
            }
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            animator.Play("Walk");
            Chase();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            Attack();
        }

    }

    private void Travel()
    {
       
        if (!destinationPointSet)
        {
            SearchDestinationPoint();
        }

        if (destinationPointSet)
        {
            agent.SetDestination(currentDestination.position);
        }

        if (agent.remainingDistance == agent.stoppingDistance && travelling == true)
        {

            if (choices[randomChoice] == 1)
            {
                StartCoroutine("PatrolState");
            }

            if (choices[randomChoice] == 2)
            {
                // Checks if path is still being Completed;
                if (agent.pathPending)
                {
                    animator.Play("Walk");
                }

                else
                {
                    StartCoroutine("IdleState");
                }

            }
            
        }
    }

    private void SearchDestinationPoint()
    {
        randInt = Random.Range(0, destinations.Count);
        currentDestination = destinations[randInt];

        // Same as @ patrolling function
        if (Physics.Raycast(currentDestination.position, -transform.up, 2f, groundLayer))
        {
            destinationPointSet = true;
        }
    }

    IEnumerator PatrolState()
    {
        
        travelling = false;
        isPatrolling = true;
        animator.Play("Walk");
        patrolTime = Random.Range(minPatrolTime, maxPatrolTime);
        
        yield return new WaitForSeconds(patrolTime);
        
        destinationPointSet = false;
        isPatrolling = false;
        randomChoice = Random.Range(0, 2);
        travelling = true;
    }

    IEnumerator IdleState()
    {
        Debug.Log("Is Idle");
        animator.Play("Idle");
        travelling = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        footstep.enabled = false;

        yield return new WaitForSeconds(idleTime);

        footstep.enabled = true;
        destinationPointSet = false;
        randomChoice = Random.Range(0, 2);
        travelling = true;
        
    }

    private void patrolling()
    {
      
        if (!patrolPointSet)
        {
            float randomX = Random.Range(-minPatrolDist, maxPatrolDist);
            float randomZ = Random.Range(-minPatrolDist, maxPatrolDist);
            
            patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            
            // Checks to see if we are at patrol point NB it is gonna be false!!
            if (Physics.Raycast(patrolPoint, -transform.up, 2f, groundLayer))
            {
                patrolPointSet = true;
            }
        }

        if(patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
        }

        
        if(agent.remainingDistance <= agent.stoppingDistance && isPatrolling == true)
        {
            patrolPointSet = false;
        }
        footstep.enabled = true;

    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        // Enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            if (canResetAttack)
            {
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }

            else
            {
                footstep.enabled = false;
                roverManager.Destroyed();
                playerIsDead = true;

                
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 spherePos = transform.position + positionOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spherePos, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, attackRange);
    }
}
