using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public Animator anim;
 
    public GameObject grenadePrefab;
    public float throwCooldown = 3f;
    public float counter;
    public float grenadeThrowForce = 30f;
    public float grenadeThrowForceUp = 0.5f;
    public GrenadeBar grenadeBar;
    public float grenadeSpawnRange = 2f;

    private void Start()
    {
        counter = throwCooldown;
        grenadeBar.setMaxSeconds(throwCooldown);
        
    }

    void Update()
    {
        if (GameCoordinator.instance.isPaused)
        {
            return;
        }

        counter += Time.deltaTime;
        grenadeBar.setSeconds(counter);

        if (Input.GetMouseButtonDown(1))
        {
            if (counter >= throwCooldown)
            {
                StartCoroutine(throwGrenade());
                counter = 0f;
                grenadeBar.setSeconds(counter);
            }
            
        }
    }


    public IEnumerator throwGrenade()
    {
        /*
        GameObject grenade = Instantiate(grenadePrefab, grenadeHoldingP.position, Quaternion.identity);
        Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
        SphereCollider grenadeCol = grenade.GetComponent<SphereCollider>();
        grenade.transform.parent = grenadeHoldingP;




        

         anim.SetBool("isThrowing", true);
         yield return new WaitForSeconds(0.25f);        
         grenade.transform.parent = null;
         Vector3 throwDirection = transform.forward.normalized * grenadeThrowForce + Vector3.up * grenadeThrowForceUp;
         grenadeRb.AddForce(throwDirection , ForceMode.VelocityChange);
         grenadeCol.enabled = true;
         grenadeBar.setSeconds(0f);
         yield return new WaitForSeconds(1f);
         anim.SetBool("isThrowing", false);
     */
      
    
        anim.SetTrigger("ThrowGrenade");
        yield return new WaitForSeconds(0.15f);


        Vector3 grenadeHoldingPoint = transform.position + transform.forward * grenadeSpawnRange;
        grenadeHoldingPoint = transform.position + transform.forward * grenadeSpawnRange;
        GameObject grenade = Instantiate(grenadePrefab, grenadeHoldingPoint, Quaternion.identity);
        Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
        SphereCollider grenadeCol = grenade.GetComponent<SphereCollider>();
        Vector3 throwDirection = (transform.forward * grenadeThrowForce) + (Vector3.up * grenadeThrowForceUp);
        grenadeRb.AddForce(throwDirection, ForceMode.VelocityChange);
        grenadeCol.enabled = true;
        grenadeBar.setSeconds(0f);
       

    }

}
