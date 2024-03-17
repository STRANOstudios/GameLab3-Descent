using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class imcrying : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public Vector3 walkPoint;

    bool walkPointSet;

    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange;
    public float attackRange;


    public bool playerInSightInRange;
    public bool playerInAttackRange;

    public GameObject enemyBullets;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightInRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //checka se è nel campo visivo 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //checka se è nel campo d'attacco
        
        if (!playerInSightInRange && !playerInAttackRange) 
        {
            Patroling();
        }

        if (playerInSightInRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInSightInRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void Patroling()
    {
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude > 1f)
        {
            walkPointSet = false;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() 
    { 
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(enemyBullets, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere (transform.position, sightRange);
    }



}
