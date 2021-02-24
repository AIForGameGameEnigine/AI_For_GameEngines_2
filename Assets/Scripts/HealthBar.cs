using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;
    public Slider healthFill;

    public float currentHealth;
    public float maxHealth;
    public float healthBarYOffset = 2;

    void Update()
    {
        PositionHealthBar();

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthFill.value = currentHealth / maxHealth;
    }

    private void PositionHealthBar()
    {
        Vector3 currentPos = transform.position;

        healthBar.position = new Vector3(currentPos.x, currentPos.y + healthBarYOffset, currentPos.z);

        healthBar.LookAt(Camera.main.transform);
    }
}
