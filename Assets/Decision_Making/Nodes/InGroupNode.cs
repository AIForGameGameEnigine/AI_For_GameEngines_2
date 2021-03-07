using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGroupNode : Node
{
    private GameObject[] minions;
    private int groupSize;

    public InGroupNode(GameObject[] minions, int groupSize)
    {
        this.minions = minions;
        this.groupSize = groupSize;
    }

    public override NodeState Evaluate()
    {
        if(minions.Length >= groupSize)
        {
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }else
        {
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }
    }
}
