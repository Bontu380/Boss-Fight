using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    
    public Enemy boss;
    public int armHealth = 500;
    private Rigidbody armRb;
    public GameObject rocketPrefab;
    public Transform firePoint;
  

   
    private void Start()
    {
        armRb = GetComponent<Rigidbody>();

    }



    public void takeDamage(int damage)
    {
        boss.takeDamage(damage/2);
        armHealth -= damage;
        if(armHealth <= 0)
        {
            transform.parent = null;
            armRb.isKinematic = false;
            Destroy(this);
        }

    }

    public void fireRocket(Transform target)
    {
      GameObject rocket =  Instantiate(rocketPrefab, firePoint.transform.position, firePoint.transform.rotation);
      Rocket rocketScript = rocket.GetComponent<Rocket>();
      rocketScript.playerPos = target;
    }
}
