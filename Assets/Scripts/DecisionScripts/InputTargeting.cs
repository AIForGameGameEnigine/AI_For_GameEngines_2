using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargeting : MonoBehaviour
{

    private GameObject selectedHero;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        selectedHero = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                Role role = hit.collider.GetComponent<Role>();

                if ( role != null)
                {
                    Role targetRole = hit.collider.gameObject.GetComponent<Role>();
                    Role HeroRole = selectedHero.GetComponent<Role>();

                    if (targetRole.teamType != HeroRole.teamType)
                    {
                        selectedHero.GetComponent<Combat>().targetedEnemy = hit.collider.gameObject;
                    }
                }
                else
                {
                    selectedHero.GetComponent<Combat>().targetedEnemy = null;
                }
            }
        }
    }
}
