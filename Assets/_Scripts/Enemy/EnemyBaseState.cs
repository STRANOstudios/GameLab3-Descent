using UnityEngine;

public abstract class EnemyBaseState 
{
    public abstract void EnterState(StateManager enemy);

    public abstract void UpdateState(StateManager enemy);
    
    public abstract void OnExit(StateManager enemy);
}
