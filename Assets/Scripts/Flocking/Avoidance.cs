using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class Avoidance : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> agentContext, Flock flock)
    {
        //If no neighbors, return no adjustment
        if(agentContext.Count == 0)
        {
            return Vector3.zero;
        }

        //Add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        float avoidanceMoveX = 0f;
        float avoidanceMoveZ = 0f;

        int nAvoid = 0;

        //Check distance between agents
        foreach(Transform item in agentContext)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMoveX += (agent.transform.position.x - item.position.x);
                avoidanceMoveZ += (agent.transform.position.z - item.position.z);
            }
        }

        NavMeshHit hit;
        if(NavMesh.FindClosestEdge(agent.transform.position, out hit, NavMesh.AllAreas))
        {
            float dist = Vector3.Distance(agent.transform.position, hit.position);
            Debug.DrawRay(hit.position, Vector3.up, Color.red);

            if(dist < 1.0f)
            {
                nAvoid++;
                avoidanceMoveX += (agent.transform.position.x - hit.position.x);
                avoidanceMoveZ += (agent.transform.position.z - hit.position.z);
            }
        }

        avoidanceMove = new Vector3(avoidanceMoveX, 0, avoidanceMoveZ);

        if(nAvoid > 0) {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
