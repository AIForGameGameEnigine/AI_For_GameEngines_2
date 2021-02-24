using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform targetTransform;
    private float movementSpeed, rotSpeed;

    void Start()
    {
        movementSpeed = 10.0f;
        rotSpeed = 2.0f;
    }


    void Update()
    {
        

        Vector3 tarPos = targetTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);

        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetTransform.position) < 2.0f)
        {
            return;
        }

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));

    }
}
