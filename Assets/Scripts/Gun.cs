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

        if (isReloading || GameCoordinator.instance.isPaused || GameCoordinator.instance.playerMovingInputUnavailable )
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
                animator.SetBool("isFiring", false);
                StartCoroutine(reload());
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (nextBulletTimeCounter >= fireRateRatio)
            {
                animator.SetBool("isFiring", true);
                fire();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isFiring", false);
        }

        nextBulletTimeCounter += Time.deltaTime;

    }

    private void fire()
    {
        if (currentBullet <= 0)
        {
            animator.SetBool("isFiring", false);
            Debug.Log("Out of bullets");
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
        Debug.Log("In coroutine");
        isReloading = true;
        animator.SetBool("isFiring", false);
        animator.SetBool("Reloading", true);
        //Debug.Log(animator.GetBool("Reloading"));

        float animationLength = 0f;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if(clip.name == "WeaponReloading")
            {
                animationLength = clip.length;
                break;
            }
        }

        yield return new WaitForSeconds(animationLength);


        currentBullet = clipCapacity;
        animator.SetBool("Reloading", false);
       // animator.SetBool("isFiring", false);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
        {
            yield return null;
        }

        bulletInfo.text = currentBullet.ToString();
        isReloading = false;


    }
}


