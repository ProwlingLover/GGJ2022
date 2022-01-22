using UnityEngine;
using Pathfinding;

public class ECEnemy : ECNPC {

    private NPCState state = NPCState.PatrolIdle;
    private IAstarAI agent;

    void Start() 
    {
        agent = gameObject.GetComponent<IAstarAI>();
    }
    public void RemoveTarget()
    {
        var aiDesSetter = gameObject.GetComponent<AIDestinationSetter>();
        if (aiDesSetter != null)
        {
            aiDesSetter.target = null;
        }
    }

    public void SwitchToTargetState(NPCState state)
    {
        this.state = state;
    }

    public override void UpdateState()
    {
        if (agent != null)
        {
            if (agent.reachedEndOfPath && !agent.pathPending)
            {
                SwitchToTargetState(NPCState.PatrolIdle);
            }
            else
            {
                SwitchToTargetState(NPCState.PatrolWalk);
            }
        }
    }   
}