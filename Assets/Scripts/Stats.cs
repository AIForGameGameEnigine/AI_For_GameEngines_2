using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private int lvl;
    public float expToNextLvl;
    public float exp;
    public float expOnDeath;
    public float maxHealth;
    private float health;
    public float atkDmg;
    public float atkSpd;
    private float atkTime = 1.4f;

    Combat combatScript;

    public float Health { get => health; set => health = value; }
    public float AtkTime { get => atkTime; }
    public float Exp { get => exp; set => exp = value; }

    // Start is called before the first frame update
    void Start()
    {
        combatScript = GetComponent<Combat>();
        health = maxHealth;
        lvl = 1;
        expToNextLvl = 100;
        Exp = 0;
        expOnDeath = 20 * lvl;
    }

    // Update is called once per frame
    void Update()
    {
        if (Exp >= expToNextLvl)
            LevelUp();

        if(health <= 0)
        {
            Destroy(gameObject);
            combatScript.targetedEnemy = null;
            combatScript.performMeleeAttack = false;
            combatScript.IsAlive = false;
        }
    }

    private void LevelUp()
    {
        lvl++;
        maxHealth += maxHealth * 0.3f;
        atkDmg += atkDmg * 0.25f;
        atkSpd += atkSpd * 0.1f;
        exp = 0;
        expToNextLvl += expToNextLvl * 0.2f;
        expOnDeath = 20 * lvl;

        Debug.Log("Level Up!");
    }
}
