using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeNode : Node
{
    private float range;
    private GameObject[] targets;
    private Transform origin;

    public InRangeNode(float range, GameObject[] targets, Transform origin)
    {
        this.range = range;
        this.targets = targets;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        foreach(var tar in targets)
        {
            float dist = Vector3.Distance(tar.transform.position, origin.position);
            _nodeState = dist < range ? NodeState.SUCCESS : NodeState.FAILURE;

            if(_nodeState == NodeState.SUCCESS)
                return _nodeState;
        }
       
        return _nodeState;
    }
}
