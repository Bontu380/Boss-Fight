using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Transform playerPos;
    public Vector3 targetPos;
    public int damage = 30;
    public float speed = 3f;
    public float rotateSpeed = 2f;
    public float explosionRadius = 1f;
    public float explosionBlastForce = 5f;
    public GameObject explosionEffect;
    private Rigidbody rocketRb;



    private void Start()
    {
        rocketRb = GetComponent<Rigidbody>();
       // targetPos = playerPos.position;
    }

    void Update()
    {
        if (GameCoordinator.instance.isPaused)
        {
            return;
        }
        /*
         Vector3 nextPos = Vector3.Lerp(rocketRb.position, playerPos.position, speed * Time.deltaTime);
         rocketRb.MovePosition(nextPos);
         Vector3 direction = playerPos.position - transform.position;
         Quaternion lookAtRotation = Quaternion.LookRotation(direction);
         transform.rotation = Quaternion.Lerp(transform.rotation,lookAtRotation, rotateSpeed * Time.deltaTime);
         */



        /*Vector3 nextPos = Vector3.Lerp(transform.position, playerPos.position, speed * Time.deltaTime);
        rocketRb.MovePosition(nextPos);
        Vector3 direction = playerPos.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, rotateSpeed * Time.deltaTime);
        */
       // Vector3 nextPos = Vector3.MoveTowards(transform.position,targetPos,speed*Time.deltaTime);
       // rocketRb.MovePosition(nextPos);

        //Vector3 direction = playerPos.position - transform.position;
        //Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, rotateSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        explode(collision.transform);
    }

    public void explode(Transform collisionPoint)
    {
       GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
       Destroy(effect, 3f);
       Collider[] effectedObjects = Physics.OverlapSphere(collisionPoint.position, explosionRadius);
       foreach (Collider effected in effectedObjects)
       {

         PlayerStats player = effected.GetComponent<PlayerStats>();
         if (player != null)
         {
             player.takeDamage(damage);
         }

         Rigidbody objectRb = effected.GetComponent<Rigidbody>();
         if (objectRb != null )
         {
              objectRb.AddExplosionForce(explosionBlastForce, transform.position, explosionRadius);
         }

        }

        Destroy(gameObject);

        
    }
  
}
