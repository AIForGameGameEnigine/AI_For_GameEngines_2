using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private Transform[] target;
    private NavMeshAgent agent;

    public ChaseNode(Transform[] target, NavMeshAgent agent)
    {
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        foreach (var tar in target)
        {
            float distance = Vector3.Distance(tar.position, agent.transform.position);

            if (distance > 0.2)
            {
                agent.isStopped = false;
                agent.SetDestination(tar.position);
                _nodeState = NodeState.RUNNING;
                return _nodeState;
            }
            else
            {
                agent.isStopped = true;
                _nodeState = NodeState.SUCCESS;
                return _nodeState;
            }
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }

}
