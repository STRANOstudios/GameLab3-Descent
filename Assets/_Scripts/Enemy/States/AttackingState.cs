
using UnityEngine;

public class AttackingState : EnemyBaseState
{
    float distanceFromTarget;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");

    }

    public override void UpdateState(StateManager enemy)
    {
        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.ChaseDistance)
        {
            enemy.ChangeState(new PatrollingState());
        }

        //andre qui!! :DD

        if (distanceFromTarget > enemy.AttackDistance)
        {
            OnExit(enemy);
        }
    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(enemy.chasingState);
    }
}
