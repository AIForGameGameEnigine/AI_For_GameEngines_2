using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class Cohesion : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //If no neighbors, return no adjustment
        if(context.Count == 0)
        {
            return Vector3.zero;
        }

        //Add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        float cohesionMoveX = 0f;
        float cohesionMoveZ = 0f;

        foreach(Transform item in context)
        {
            cohesionMoveX += item.position.x;
            cohesionMoveZ += item.position.z;
        }

        cohesionMove = new Vector3(cohesionMoveX, 0, cohesionMoveZ);

        cohesionMove /= context.Count;

        //Create offset from agent position
        cohesionMove -= agent.transform.position;

        return cohesionMove;
    }
}
