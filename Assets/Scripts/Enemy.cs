using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 500;
    public int currentHealth;
    public float movementSpeed = 5f;
    public float turningSpeed = 3f;
    public Healthbar healthbar;
    public EnemyBehaviour enemyBehaviour;

    private void Start()
    {
        healthbar.setMaxHealth(maxHealth);
        currentHealth = maxHealth;

    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            die();
        }

        enemyBehaviour.playerSeen();
    }

    public void die()
    {
        //Animasyon ses falan varsa triggerla
        GameCoordinator.instance.playerIsSafe();
        Destroy(gameObject);
    }
}
