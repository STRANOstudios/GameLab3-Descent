using UnityEngine;

public class PatrollingState : EnemyBaseState
{
    float distanceThreshold = 0.1f;


    private int currentWaypointIndex = 0;

    bool lastReachPointEnded;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("You're in the patrolling state!");
        lastReachPointEnded = false;
    }

    public override void UpdateState(StateManager enemy)
    {
        Vector3 direction = (enemy.waypoints[currentWaypointIndex].position - enemy.transform.position);

        enemy.transform.LookAt(enemy.waypoints[currentWaypointIndex]);

        enemy.rb.velocity = (direction.normalized * enemy.speed * Time.deltaTime);


        if (currentWaypointIndex == enemy.waypoints.Length - 1)
        {
            lastReachPointEnded = true;
        }
        if (currentWaypointIndex == 0)
        {
            lastReachPointEnded = false;
        }

        if (Vector3.Distance(enemy.transform.position, enemy.waypoints[currentWaypointIndex].position) <= distanceThreshold)
        {
            if (lastReachPointEnded)
            {
                currentWaypointIndex -= 1;
            }
            else
            {
                currentWaypointIndex += 1;
            }
        }

        if(Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position) <= enemy.ChaseDistance)
        {
            OnExit(enemy);
        }

        //RaycastHit hit;

        //if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        //{
        //    Debug.DrawRay(enemy.transform.position, enemy.transform.forward * hit.distance, Color.red);

        //    if (hit.collider.gameObject.TryGetComponent(out Door door))
        //    {
        //        enemy.ChangeState(new PatrollingState());
        //    }
        //}
    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new ChasingState());
    }
}

