using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    public Collider collider;
    public Transform target = null;
    private bool foundEnemy = false;

    public List<Transform> targets = new List<Transform>();
    public float reloadTime = 2.0f;
    public float damage = 10.0f;
    private float nextTime;
    public GameObject laser;

    void Update()
    {
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
            if (target.GetComponent<Collider>().GetComponent<HealthBar>() != null && target.GetComponent<Collider>().GetComponent<HealthBar>().currentHealth <= 0)
            {
                targets.Remove(target);
            }

            FaceTarget();

            if(Time.time >= nextTime)
            {
                Invoke("fire", 0.2f);
                nextTime = Time.time + reloadTime;
            }
        }

        if(targets.Count > 0)
        {
            target = targets[0];
        }
    }

    public void fire()
    {
        if(target != null && target.GetComponent<Collider>().GetComponent<HealthBar>() != null)
        {
            target.GetComponent<Collider>().GetComponent<HealthBar>().ChangeHealth(-damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Player"))
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
                    if(target.tag == "Player" && idx == 0)
                    {
                        targets.Add(other.transform);
                        return;
                    }
                    else if (target.tag == "Player" && idx > 0)
                    {
                        targets.Insert(idx, other.transform);
                        return;
                    }

                    idx++;
                }

                targets.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            targets.Remove(other.transform);

            if(targets.Count <= 0)
            {
                foundEnemy = false;
                target = null;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - laser.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, lookRotation, Time.deltaTime * 100f);
    }
}
