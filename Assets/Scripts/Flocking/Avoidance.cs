using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class Avoidance : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> agentContext, Flock flock)
    {
        Vector3 avoidanceMove = Vector3.zero;
        float avoidanceMoveX = 0f;
        float avoidanceMoveZ = 0f;
        int nAvoid = 0;

        //If no neighbors check if too close to navmesh
        if(agentContext.Count == 0)
        {
            avoidanceMove = avoidNavmesh(agent, avoidanceMove, avoidanceMoveX, avoidanceMoveZ);
            return avoidanceMove;
        }

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

        avoidanceMove = new Vector3(avoidanceMoveX, 0, avoidanceMoveZ);

        //Override the move if too close to edge of navmesh
        avoidanceMove = avoidNavmesh(agent, avoidanceMove, avoidanceMoveX, avoidanceMoveZ);

        //Calculate the average
        if(nAvoid > 0) {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }

    Vector3 avoidNavmesh(FlockAgent agent, Vector3 avoidanceMove, float avoidanceMoveX, float avoidanceMoveZ) 
    {
        NavMeshHit hit;

        if(NavMesh.FindClosestEdge(agent.transform.position, out hit, NavMesh.AllAreas))
        {
            float dist = Vector3.Distance(agent.transform.position, hit.position);
            Debug.DrawRay(hit.position, Vector3.up, Color.red);

            if(dist < 2.0f)
            {
                avoidanceMoveX = (agent.transform.position.x - hit.position.x);
                avoidanceMoveZ = (agent.transform.position.z - hit.position.z);
            }
        }

        avoidanceMove = new Vector3(avoidanceMoveX, 0, avoidanceMoveZ);

        return avoidanceMove;
    }
}
