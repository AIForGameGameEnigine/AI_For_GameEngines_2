using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 50)]
    public int startingCount = 25;
    const float AgentDensity = 2f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    public List<FlockAgent> Agents { get => agents; set => agents = value; }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++) 
        {
           FlockAgent newAgent = Instantiate(
               agentPrefab,
               new Vector3(transform.position.x, transform.position.y, transform.position.z),
               Quaternion.Euler(0, Random.Range(0f, 360f), 0)
           );

           newAgent.name = "Agent" + i;
           Agents.Add(newAgent);
        }
    }

    void Update()
    {
        foreach(FlockAgent agent in Agents)
        {
            List<Transform> agentContext = GetNearbyAgents(agent);

            Vector3 move = behaviour.CalculateMove(agent, agentContext, this);
            move *= driveFactor;

            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);
        }
    }

    List<Transform> GetNearbyAgents(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);

        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider && c.gameObject.tag == "Minion")
            {
                context.Add(c.transform);
            }
        }

        return context;
    }
}
