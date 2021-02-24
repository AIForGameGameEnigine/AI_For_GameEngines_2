using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum AttackType
    {
        Melee, Ranged
    };
    public AttackType attackType;

    public GameObject targetedEnemy;
    public float attackRange;

    private Movement moveScript;
    private Stats statsScript;
    private Animator anim;

    public bool basicAttackIdle;
    private bool isAlive;
    public bool performMeleeAttack = true;

    public bool IsAlive { get => isAlive; set => isAlive = value; }

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<Movement>();
        statsScript = GetComponent<Stats>();
        anim = GetComponent<Animator>();
        isAlive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && targetedEnemy != null)
            GoToEnemy();
    }

    void GoToEnemy()
    {
        if (Vector3.Distance(transform.position, targetedEnemy.transform.position) > attackRange)
            moveScript.MoveTo(targetedEnemy.transform.position, attackRange);
        else
        {
            if (attackType == AttackType.Melee)
            {
                if (performMeleeAttack)
                {
                    StartCoroutine(MeleeAttackInterval());
                }
            }
        }
    }

    IEnumerator MeleeAttackInterval()
    {
        if(performMeleeAttack)
            anim.speed = 1 + (statsScript.atkSpd * 0.02f);
        performMeleeAttack = false;
        anim.SetBool("Basic Attack", true);

        yield return new WaitForSeconds(statsScript.AtkTime / ((100 + statsScript.AtkTime) * 0.01f));

        if(targetedEnemy == null)
        {
            anim.SetBool("Basic Attack", false);
            performMeleeAttack = true;
            anim.speed = 1;
        }
    }

    public void MeleeAttack()
    {
        if(targetedEnemy != null)
        {
            Stats targetStats = targetedEnemy.GetComponent<Stats>();
            Role targetRole = targetedEnemy.GetComponent<Role>();
            Role HeroRole = gameObject.GetComponent<Role>();

            if (targetRole.teamType != HeroRole.teamType)
            {
                targetStats.Health -= statsScript.atkDmg;

                if (targetStats.Health <= 0)
                    statsScript.Exp += targetStats.expOnDeath;
            }
        }

        performMeleeAttack = true;
    }
}
