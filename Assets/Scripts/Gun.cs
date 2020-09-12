using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public Camera playerCamera;
    public ParticleSystem gunParticle;
    public GameObject impactEffect;
    public Text bulletInfo;
    Vector3 originalCamPos;
    public bool isReloading = false;
    public int damage = 50;
    public int clipCapacity = 30;
    public float fireRateRatio = 10f;
    public float reloadTime = 1f;


    public int currentBullet;
    private float nextBulletTimeCounter = 0f;


    private void Start()
    {
        nextBulletTimeCounter = fireRateRatio;
        currentBullet = clipCapacity;
        bulletInfo.text = currentBullet.ToString();
        originalCamPos = playerCamera.transform.position;
    }
    void Update()
    {

        if (isReloading || GameCoordinator.instance.isPaused )
        {
            return;
        }

      

        if (currentBullet <= 0)
        {
            StartCoroutine(reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentBullet != clipCapacity)
            {
                StartCoroutine(reload());
                return;
            }
        }

        if (nextBulletTimeCounter >= fireRateRatio)
        {
            if (Input.GetMouseButton(0))
            {
                fire();
            }
        }

        nextBulletTimeCounter += Time.deltaTime;

    }

    private void fire()
    {
        if (currentBullet == 0)
        {
            StartCoroutine(reload());

        }
        else
        {
            gunParticle.Play();
            nextBulletTimeCounter = 0f;
            decreaseBullet();


            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    Enemy enemyScript = hit.transform.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.takeDamage(damage);
                    }
                }

                if (hit.transform.CompareTag("Arm"))
                {
                    EnemyArm arm = hit.transform.GetComponent<EnemyArm>();
                    if (arm != null)
                    {
                        arm.takeDamage(damage);
                    }
                }

                if (hit.transform.CompareTag("Rocket"))
                {
                    Rocket rocket = hit.transform.GetComponent<Rocket>();
                    rocket.explode(hit.transform);
                }

                GameObject createdEffect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(createdEffect, 1f);

            }
        }

    }

    public void decreaseBullet()
    {
        currentBullet--;
        bulletInfo.text = currentBullet.ToString();
    }

    private IEnumerator reload()
    {

        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f); //Transition between animations is default by 0.25 as i learned.
        currentBullet = clipCapacity;
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        bulletInfo.text = currentBullet.ToString();
        isReloading = false;
    }
}


