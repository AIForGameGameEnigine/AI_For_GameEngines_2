using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform targetMarker;

    // Update is called once per frame
    void Update()
    {
        int button = 0;

        if(Input.GetMouseButtonDown(button))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = 0.5f;
                targetMarker.position = targetPosition;
                //targetMarker.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
            }
        }
    }
}
