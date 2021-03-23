using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class Avoidance : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //If no neighbors, return no adjustment
        if(context.Count == 0)
        {
            return Vector3.zero;
        }

        //Add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        float avoidanceMoveX = 0f;
        float avoidanceMoveZ = 0f;

        int nAvoid = 0;

        foreach(Transform item in context)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMoveX += (agent.transform.position.x - item.position.x);
                avoidanceMoveZ += (agent.transform.position.z - item.position.z);
            }
        }

        avoidanceMove = new Vector3(avoidanceMoveX, 0, avoidanceMoveZ);

        if(nAvoid > 0) {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
