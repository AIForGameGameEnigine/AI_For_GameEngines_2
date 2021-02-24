using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTartgetNode : Node
{
    private GameObject[] targets;
    private GameObject origin;

    public AttackTartgetNode(GameObject[] targets, GameObject origin)
    {
        this.targets = targets;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        foreach (var tar in targets)
        {
            float distance = Vector3.Distance(tar.transform.position, origin.transform.position);

            if (distance <= origin.GetComponent<Combat>().attackRange)
            {
                origin.GetComponent<Combat>().targetedEnemy = tar;
                _nodeState = NodeState.RUNNING;
                return _nodeState;
            }
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }

}
