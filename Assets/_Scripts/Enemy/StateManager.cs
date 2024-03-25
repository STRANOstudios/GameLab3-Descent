using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : HP
{
    public EnemyBaseState currentState; //reference dello stato attivo nella state machine 

    [Header("Movement Settings")]
    [SerializeField, Range(1, 500)] public float speed;

    [SerializeField, Range(1, 200)] public float ChaseDistance;
    [SerializeField, Range(1, 200)] public float AttackDistance;

    [Header("Attack Settings")]
    //[SerializeField, Range(1, 20)] public float AttackRate;
    [SerializeField] public Transform firePointSX;
    [SerializeField] public Transform firePointDX;
    [SerializeField] public float fireRate;
    [SerializeField] public float spawnTime;



    [Header("Waypoints")]
    [SerializeField] public Transform[] waypoints;

    [Header("Player Prefab")]
    [SerializeField] public Transform playerPrefab;

    [Header("Bullet Prefab")]
    [SerializeField] public Transform bulletPrefab;

    [SerializeField] Drop drop;

    [HideInInspector] public Vector3 dir;

    [HideInInspector] public Projectile damageDealtToEnemy;

    [HideInInspector] public Rigidbody rb;


    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    void Start()
    {
        currentState = new PatrollingState(); 
        currentState.EnterState(this);
        damageDealtToEnemy = FindAnyObjectByType<Projectile>();
        rb = GetComponent<Rigidbody>();
        
       
    }

    void Update()
    {
      currentState.UpdateState(this);  
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi");
        if (collision.gameObject.layer == 13)
        {
            Debug.Log("si");
            damageDealtToEnemy = collision.transform.GetComponent<Projectile>();

            myHP -= damageDealtToEnemy.GetDamage;
            
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Gizmos.color = Color.blue;
    }

#endif
}
