using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GotoTower : Node
{
    private GameObject[] towers;
    private GameObject origin;

    public GotoTower(GameObject[] towers, GameObject origin)
    {
        this.towers = towers;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        var t = towers[0];
        foreach(var tower in towers)
        {
            float dist1 = Vector3.Distance(origin.transform.position, tower.transform.position);
            float dist2 = Vector3.Distance(origin.transform.position, t.transform.position);

            if(dist1 < dist2)
            {
                t = tower;
            }
        }

        float distance = Vector3.Distance(origin.transform.position, t.transform.position);

        if (distance > origin.GetComponent<Combat>().attackRange)
        {
            origin.GetComponent<Combat>().targetedEnemy = t;
            _nodeState = NodeState.RUNNING;
            return _nodeState;
        }else
        {
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

    }
}
