using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    public Collider collider;
    public Transform target = null;
    private bool foundEnemy = false;

    public List<Transform> targets = new List<Transform>();
    public float reloadTime = 0.5f;
    //public float damage = 10.0f;
    private float nextTime;
    public GameObject laser;
    public float Range;

    private void Start()
    {
        Physics.IgnoreLayerCollision(0, 7);
    }

    void Update()
    {
        GetAllInRange();

        if(target != null)
        {
            foundEnemy = true;
        }
        else
        {
            foundEnemy = false;
        }

        if(foundEnemy)
        {
            if (target.GetComponent<Stats>().Health <= 0)
            {
                targets.Remove(target);
            }

            FaceTarget();

            if(Time.time >= nextTime)
            {
                fire();
                nextTime = Time.time + reloadTime;
            }
        }

        if(targets.Count > 0)
        {
            target = targets[0];

            if (target == null)
                targets.Remove(target);
        }

        
    }

    public void fire()
    {
        if(target != null)
        {
            GetComponent<Combat>().targetedEnemy = target.gameObject;
        }else
        {
            targets.Remove(target);
        }
    }

    void GetAllInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Range, 1 << LayerMask.NameToLayer("Targetable"));

        targets.Clear();

        foreach (var hitCollider in hitColliders)
        {
            Role role = hitCollider.gameObject.GetComponent<Role>();
            if (role != null)
            {
                if (role.teamType != GetComponent<Role>().teamType)
                {
                    EnemyInRange(hitCollider);
                }
            }
        }

        if (targets.Count <= 0)
        {
            foundEnemy = false;
            target = null;
        }
    }


    private void EnemyInRange(Collider other)
    {
        if (targets.Count == 0)
        {
            targets.Add(other.transform);
        }
        else
        {
            int idx = 0;

            foreach (Transform target in targets)
            {
                if(GetComponent<Role>().roleType == Role.RoleType.Champion && idx == 0)
                {
                    targets.Add(other.transform);
                    return;
                }
                else if (GetComponent<Role>().roleType == Role.RoleType.Champion && idx > 0)
                {
                    targets.Insert(idx, other.transform);
                    return;
                }

                idx++;
            }

            targets.Add(other.transform);
        }
        
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Enemy") || other.CompareTag("Player"))
    //    {
    //        targets.Remove(other.transform);

    //        if(targets.Count <= 0)
    //        {
    //            foundEnemy = false;
    //            target = null;
    //        }
    //    }
    //}

    void FaceTarget()
    {
        Vector3 direction = (target.position - laser.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, lookRotation, Time.deltaTime * 100f);
    }
}
