using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class Alignment : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> agentContext, Flock flock)
    {
        //If no neighbors, maintain current allignment
        if(agentContext.Count == 0)
        {
            return agent.transform.forward;
        }

        //Add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        float alignmentMoveX = 0f;
        float alignmentMoveZ = 0f;

        List<Transform> filteredContext = (filter == null) ? agentContext : filter.Filter(agent, agentContext); 

        foreach(Transform item in filteredContext)
        {
            alignmentMoveX += item.transform.transform.forward.x;
            alignmentMoveZ += item.transform.transform.forward.z;
        }

        alignmentMove = new Vector3(alignmentMoveX, 0, alignmentMoveZ);

        alignmentMove /= agentContext.Count;

        return alignmentMove;
    }
}
