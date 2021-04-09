using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange;

    public List<GameObject> towers;
    public List<GameObject> allyTowers;
    public List<GameObject> champs;
    public List<GameObject> minions;
    public List<GameObject> allys;
    public int groupSize = 3;

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
        towers = new List<GameObject>();
    }

    private void Update()
    {
        GetTowersToTarget();
        GetAllInRange();
        ConstructBehaviourTree();
        topNode.Evaluate();

        champs.Clear();
        minions.Clear();
        towers.Clear();
        allyTowers.Clear();
        allys.Clear();
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
                }else
                {
                    switch (role.roleType)
                    {
                        case Role.RoleType.Champion:
                        case Role.RoleType.Minion:
                            if (allys.IndexOf(hitCollider.gameObject) < 0)
                                allys.Add(hitCollider.gameObject);
                            break;
                    }
                }
            }
           
        }
    }

    void GetTowersToTarget()
    {
        GameObject[] t = GameObject.FindGameObjectsWithTag("Tower");

        foreach(var target in t)
        {
            if (target.GetComponent<Role>().teamType != GetComponent<Role>().teamType &&
                target.GetComponent<Role>().lane == GetComponent<Role>().lane)
            {
                if (towers.IndexOf(target) < 0)
                    towers.Add(target);
            }

            if (target.GetComponent<Role>().teamType == GetComponent<Role>().teamType &&
               target.GetComponent<Role>().lane == GetComponent<Role>().lane)
            {
                if (allyTowers.IndexOf(target) < 0)
                    allyTowers.Add(target);
            }
        }

    }

    private void ConstructBehaviourTree()
    {


        //target players
        InRangeNode champsInRange = new InRangeNode(detectionRange, champs.ToArray(), transform);
        AttackTartgetNode attackChamps = new AttackTartgetNode(champs.ToArray(), gameObject);

        Sequence targetChampSeg = new Sequence(new List<Node> { champsInRange, attackChamps });

        //target minions
        InRangeNode minionsInRange = new InRangeNode(detectionRange, minions.ToArray(), transform);
        AttackTartgetNode attackMinions = new AttackTartgetNode(minions.ToArray(), gameObject);

        Sequence targetkMinionSeg = new Sequence(new List<Node> { minionsInRange, attackMinions });

        //target buildings
        InGroupNode isInGroup = new InGroupNode(allys.ToArray(), groupSize);
        GotoTower gotoEnemyTower = new GotoTower(towers.ToArray(), gameObject);

        Sequence targetTowerSeg = new Sequence(new List<Node> { isInGroup, gotoEnemyTower });

        //regroup
        Inverter inGroupInverter = new Inverter(isInGroup);
        GotoTower gotoOwnTower = new GotoTower(allyTowers.ToArray(), gameObject);

        Sequence regroupSeg = new Sequence(new List<Node> { inGroupInverter, gotoOwnTower });


        topNode = new Selector(new List<Node> {targetChampSeg, targetkMinionSeg, targetTowerSeg, regroupSeg});
    }
}
