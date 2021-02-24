using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange;

    public GameObject[] towers;
    public List<GameObject> champs;
    public List<GameObject> minions;

    private NavMeshAgent agent;
    private Selector topNode;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        champs = new List<GameObject>();
        minions = new List<GameObject>();
    }

    private void LateUpdate()
    {
        GetAllInRange();
        ConstructBehaviourTree();
        topNode.Evaluate();
    }

    void GetAllInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            Role role = hitCollider.gameObject.GetComponent<Role>();
            if(role != null)
            {
                if (role.teamType != GetComponent<Role>().teamType)
                {
                    switch (role.roleType)
                    {
                        case Role.RoleType.Champion:
                            if(champs.IndexOf(hitCollider.gameObject) < 0)
                                champs.Add(hitCollider.gameObject);
                            break;
                        case Role.RoleType.Minion:
                            if (minions.IndexOf(hitCollider.gameObject) < 0)
                                minions.Add(hitCollider.gameObject);
                            break;
                    }
                }
            }
           
        }
    }

    private void ConstructBehaviourTree()
    {
        InRangeNode minionsInRange = new InRangeNode(detectionRange, minions.ToArray(), transform);
        InRangeNode champsInRange = new InRangeNode(detectionRange, champs.ToArray(), transform);
        InRangeNode towersInRange = new InRangeNode(detectionRange, towers, transform);
        AttackTartgetNode attackMinions = new AttackTartgetNode(minions.ToArray(), gameObject);
        AttackTartgetNode attackChamps = new AttackTartgetNode(champs.ToArray(), gameObject);
        Inverter towerInRangeInverter = new Inverter(towersInRange);
        GotoTower gotoTower = new GotoTower(towers, gameObject);

        Sequence attackMinionSeg = new Sequence(new List<Node> { minionsInRange, towerInRangeInverter, attackMinions});
        Sequence attackChampSeg = new Sequence(new List<Node> { champsInRange, attackChamps});
        Sequence goToTowerSeg = new Sequence(new List<Node> {gotoTower});

        topNode = new Selector(new List<Node> {attackMinionSeg, attackChampSeg, goToTowerSeg});
    }
}
