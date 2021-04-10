using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay In Radius")]
public class StayInRadius : FlockBehaviour
{
    // public Vector3 center;
    public float radius = 20f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> agentContext, Flock flock)
    {
        Vector3 centerOffset = flock.transform.position - agent.transform.position;
        float t = centerOffset.magnitude / radius;

        if(t < 0.95)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
    }
}
