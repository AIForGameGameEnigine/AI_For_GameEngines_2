using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if(targets.Length > 0)
        {
            GameObject finalTarget = targets.Select(tar => tar).OrderByDescending(tar => tar.GetComponent<Stats>().Health).ToArray()[0];

            origin.GetComponent<Combat>().targetedEnemy = finalTarget;
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }

}
