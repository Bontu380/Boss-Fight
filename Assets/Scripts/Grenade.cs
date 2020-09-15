using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionWait = 3f;
    public float countdown = 0f;
    public float explosionRadius = 0.5f;
    public float explosionForce = 100f;
    public int damage = 200;
    public GameObject explosionEffect;
    void Start()
    {
        countdown = explosionWait;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCoordinator.instance.isPaused)
        {
            return;
        }
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            explode();
        }
    }

    public void explode()
    {
       GameObject createdExplosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
       Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius);
       
        foreach(Collider nearby in nearbyObjects)
        {
            Enemy enemyScript = nearby.GetComponent<Enemy>();
            if(enemyScript != null)
            {
                enemyScript.takeDamage(damage);
            }
            
            EnemyArm arm = nearby.GetComponent<EnemyArm>();
            if (arm != null)
            {
               arm.takeDamage(damage);
            }
            

            Rigidbody objectRb = nearby.GetComponent<Rigidbody>();
            if(objectRb != null)
            {
                objectRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(createdExplosion, 1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /* if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Arm"))
         {
             explode();
         }
         */
        explode();
    }
}
