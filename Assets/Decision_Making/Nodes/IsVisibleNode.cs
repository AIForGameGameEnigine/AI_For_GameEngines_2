using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IsVisibleNode : Node
{
    private int fieldOfView;
    private int viewDistance;

    private Transform[] target;
    private Transform origin;

    private Vector3 rayDirection;

    public IsVisibleNode(int fieldOfView, int viewDistance, 
                        Transform[] target, Transform origin)
    {
        this.fieldOfView = fieldOfView;
        this.viewDistance = viewDistance;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        foreach(var tar in target)
        {
            rayDirection = tar.position - origin.position;

            if (Vector3.Angle(rayDirection, origin.forward) < fieldOfView)
            {
                if (Physics.Raycast(origin.position, rayDirection, out hit, viewDistance))
                {
                    Debug.DrawLine(origin.position, rayDirection * viewDistance, Color.red);

                    if (hit.transform.CompareTag("Cover"))
                    {
                        _nodeState = NodeState.FAILURE;
                        return _nodeState;
                    }

                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                }
            }

            Debug.DrawLine(origin.position, rayDirection, Color.green);
        }

        _nodeState = NodeState.FAILURE;
        return _nodeState;

    }
}
