using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    
    public Enemy boss;
    public GameObject firingExplosion;
    public int armHealth = 500;
    private Rigidbody armRb;
    public GameObject rocketPrefab;
    public Transform firePoint;
    public float rocketForce = 5f;


   /* private void Update()
    {
        Quaternion currentRotation = transform.rotation;
                
    }
    */

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
      GameObject explosionEffect = Instantiate(firingExplosion, firePoint.transform.position, firePoint.transform.rotation);
      Destroy(explosionEffect, 0.8f);
      Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
      Vector3 direction = target.position - rocket.transform.position;
      rocketRb.AddForce(direction * rocketForce,ForceMode.VelocityChange);  
    }
}
