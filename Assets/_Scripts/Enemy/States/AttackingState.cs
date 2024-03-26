
using UnityEngine;

public class AttackingState : EnemyBaseState
{
    float distanceFromTarget;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");
        enemy.rb.velocity = Vector3.zero;

    }

    public override void UpdateState(StateManager enemy)
    {
        RaycastHit hit;
        enemy.transform.LookAt(enemy.playerPrefab);
        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.ChaseDistance)
        {
            enemy.ChangeState(new PatrollingState());
            return;
        }

        enemy.spawnTime += Time.deltaTime;
        if (enemy.spawnTime >= enemy.fireRate)
        {
            MonoBehaviour.Instantiate(enemy.bulletPrefab, enemy.firePointSX.position, enemy.transform.rotation);
            MonoBehaviour.Instantiate(enemy.bulletPrefab, enemy.firePointDX.position, enemy.transform.rotation);
            enemy.spawnTime = 0;
        }

        if (distanceFromTarget > enemy.AttackDistance)
        {
            OnExit(enemy);
        }

        //if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        //{
        //    Debug.DrawRay(enemy.transform.position, enemy.transform.forward * hit.distance, Color.red);
        //    if (hit.collider.gameObject.layer == 16)
        //    {
        //        Debug.Log("colpito porta");
        //        enemy.ChangeState(new PatrollingState());
        //    }
        //}

    }

    public override void OnExit(StateManager enemy)
    { 
        enemy.ChangeState(new ChasingState());    
    }
}
