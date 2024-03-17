using UnityEngine;

public class ChasingState : EnemyBaseState
{
    float distanceFromTarget;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering chasing mode");
    }

    public override void UpdateState(StateManager enemy)
    {
        enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
        
        enemy.transform.LookAt(enemy.playerPrefab);
        enemy.transform.position += (enemy.dir.normalized * enemy.Speed * Time.deltaTime);

        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget <= enemy.AttackDistance)
        {
            OnExit(enemy);
        }

        else if(distanceFromTarget > enemy.ChaseDistance)
        {
            enemy.ChangeState(new PatrollingState());
        }

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(enemy.attackingState);
    }
}
