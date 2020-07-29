using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public bool isDead = false;
    public int maxPlayerHealth = 100;
    public int currentHealth;


    public Healthbar healthbar;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentHealth = maxPlayerHealth;
        healthbar.setMaxHealth(maxPlayerHealth);
  
     
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {

   
        isDead = true;
        GameCoordinator.instance.playerDied();
   

    }


}
