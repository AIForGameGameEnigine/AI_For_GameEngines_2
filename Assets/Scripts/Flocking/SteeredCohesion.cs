using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesion : FlockBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> agentContext, Flock flock)
    {
        //If no neighbors, return no adjustment
        if(agentContext.Count == 0)
        {
            return Vector3.zero;
        }

        //Add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        float cohesionMoveX = 0f;
        float cohesionMoveZ = 0f;

        foreach(Transform item in agentContext)
        {
            cohesionMoveX += item.position.x;
            cohesionMoveZ += item.position.z;
        }

        cohesionMove = new Vector3(cohesionMoveX, 0, cohesionMoveZ);

        cohesionMove /= agentContext.Count;

        //Create offset from agent position
        cohesionMove.x -= agent.transform.position.x;
        cohesionMove.z -= agent.transform.position.z;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}
