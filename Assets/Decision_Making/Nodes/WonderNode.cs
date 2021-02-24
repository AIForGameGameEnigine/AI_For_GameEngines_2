using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WonderNode : Node
{
    private Transform[] targets;
    private NavMeshAgent agent;

    private int currentTarget;

    public WonderNode(Transform[] targets, NavMeshAgent agent)
    {
        this.targets = targets;
        this.agent = agent;

        currentTarget = 0;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(targets[currentTarget].position, agent.transform.position);

        if(distance > 0.2)
        {
            agent.isStopped = false;
            agent.SetDestination(targets[currentTarget].position);
            _nodeState = NodeState.RUNNING;
            return _nodeState;

        }else
        {
            agent.isStopped = true;
            currentTarget = Random.Range(0, targets.Length);
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

    }

}
