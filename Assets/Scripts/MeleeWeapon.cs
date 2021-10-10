using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    public int attackDamage;
    public float attackRadius;
    public float knockback;
    public Transform attackPoint;
    //public LayerMask damageableEnemies;


    public virtual void meleeAttack()
    {
        Collider[] hitObjects = Physics.OverlapSphere(attackPoint.position, attackRadius);
        Debug.Log("Called meleeAttack");

        foreach (Collider hitObject in hitObjects)
        {
            Debug.Log(hitObject.name);
            if (hitObject.transform.CompareTag("Enemy"))
            {
                Enemy enemyScript = hitObject.GetComponent<Enemy>();
                if (enemyScript)
                {
                    enemyScript.takeDamage(attackDamage);
                }
            }

            Rigidbody hitObjectRb = hitObject.GetComponent<Rigidbody>();
            if (hitObjectRb)
            {
                hitObjectRb.AddExplosionForce(knockback,attackPoint.position,attackRadius);
            }
        }


    }
}