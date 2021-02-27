using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject Tower;
    private Transform target;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        target = null;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        //Check for targets
        if (Tower.GetComponent<TargetDetection>().target != null)
        {
            target = Tower.GetComponent<TargetDetection>().target;

            lineRenderer.SetPosition(1, target.position);
        }
        else
        {
            target = null;
        }

        if(target != null)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

       
        //RaycastHit hit;

        //if(Physics.Raycast(transform.position, transform.forward, out hit) 
        //    /*&& (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Floor"))*/)
        //{
        //    if(hit.collider)
        //    {
        //        lineRenderer.SetPosition(1, hit.point);
        //    }
        //}
        //else
        //{
        //    lineRenderer.SetPosition(1, transform.forward * 5000);
        //}


    }
}
