using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    private Role role;

    public GameObject minion;
    public Transform spawnPos;
    [SerializeField] private int numMinion;
    [SerializeField] private float timeBetweenSpawns;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        role = GetComponent<Role>();
        elapsedTime = 0;

        InvokeRepeating("SpawnMinionsForLanes", 1, timeBetweenSpawns);
    }

    public void SpawnMinionsForLanes()
    {
        SpawnMinions(Role.LaneType.Top);
        SpawnMinions(Role.LaneType.Mid);
        SpawnMinions(Role.LaneType.Bot);
    }

    public void SpawnMinions(Role.LaneType lane)
    {
        for(int i = 0; i < numMinion; i++)
        {
            GameObject newMinion = Instantiate(minion, spawnPos.position, spawnPos.rotation);

            newMinion.GetComponent<Role>().teamType = role.teamType;
            newMinion.GetComponent<Role>().roleType = Role.RoleType.Minion;
            newMinion.GetComponent<Role>().lane = lane;
        }
    }
}
