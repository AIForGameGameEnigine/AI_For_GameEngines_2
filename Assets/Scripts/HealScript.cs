using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour
{
    public float secPerHeal;
    public float healAmount;

    private float nextTime = 0;
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        Role role = other.gameObject.GetComponent<Role>();

        if (other.gameObject.GetComponent<Role>() != null)
        {
            if(other.gameObject.GetComponent<Role>().teamType == transform.parent.gameObject.GetComponent<Role>().teamType 
                    && role.roleType == Role.RoleType.Champion)
            {
                GameObject champ = other.gameObject;

                if (Time.time >= nextTime)
                {
                    champ.GetComponent<Stats>().Health += healAmount;
                    nextTime = Time.time + secPerHeal;
                }
            }
        }
    }
}
