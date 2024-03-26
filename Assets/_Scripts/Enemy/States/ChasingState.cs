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
        RaycastHit hit;
        enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
        
        enemy.transform.LookAt(enemy.playerPrefab);
        enemy.rb.velocity = (enemy.dir.normalized * enemy.speed * Time.deltaTime);

        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget <= enemy.AttackDistance)
        {
            OnExit(enemy);
        }

        else if(distanceFromTarget > enemy.ChaseDistance)
        {
            enemy.ChangeState(new PatrollingState());
        }

        
        //if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        //{
        //    Debug.DrawRay(enemy.transform.position, enemy.transform.forward * hit.distance, Color.red);

        //    if (hit.collider.gameObject.TryGetComponent(out Door door))
        //        {
        //        enemy.ChangeState(new PatrollingState());
        //    }
        //}

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new AttackingState());
    }
}


//RaycastHit hit;
//if (Physics.Raycast(transform.position, transform.forward, out hit))
//{
//    // Se il raycast colpisce un oggetto, controlla se è un oggetto con tag "Target"
//    if (hit.collider.CompareTag("Target"))
//    {
//        // Se è un oggetto con tag "Target", stampa "Ciao" nella console
//        Debug.Log("Ciao");
//    }
//}