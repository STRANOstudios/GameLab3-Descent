using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : HP
{
    public EnemyBaseState currentState; //reference dello stato attivo nella state machine 
    public PatrollingState patrollingState = new PatrollingState();
    public ChasingState chasingState = new ChasingState();
    public AttackingState attackingState = new AttackingState();

    [Header("Movement Settings")]
    [SerializeField, Range(1, 20)] public float Speed;

    [SerializeField, Range(1, 20)] public float ChaseDistance;
    [SerializeField, Range(1, 20)] public float AttackDistance;

    [Header("Attack Settings")]
    [SerializeField, Range(1, 20)] public float AttackRate;
    [SerializeField] public Transform firePointSX;
    [SerializeField] public Transform firePointDX;


    [Header("Waypoints")]
    [SerializeField] public Transform[] waypoints;

    [Header("Player Prefab")]
    [SerializeField] public Transform playerPrefab;

    [Header("Bullet Prefab")]
    [SerializeField] public Transform bulletPrefab;

    [SerializeField] Drop drop;

    [HideInInspector] public Vector3 dir;

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    void Start()
    {
        currentState = patrollingState; 
        currentState.EnterState(this);
    }

    void Update()
    {
      currentState.UpdateState(this);  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            myHP -= 1;
            
            if (myHP <= 0)
            {
                Death();
            }
        }
    }

    public override void Death()
    {
        drop.CheckDropChance();
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Gizmos.color = Color.blue;
        //foreach (var direction in AvailableDirections)
        //{
        //    Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction.normalized * ChaseDistance);
        //}
    }
}
