using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float enemySightAngle = 75f;
    public float enemySightRange = 20f;
    public float enemyReturningNormalTime = 1f;
    public float rotateSpeed = 4.5f;
    public float rocketCooldown = 9f;
    public float rocketTimeCounter;
    public float timeBetweenRockets = 1f;
    public float minRocketLaunchRange = 3f;


    public float meleeKnockbackForce = 5f;

    public float meleeRange = 8f;
    public int smashDamage = 50;
    public float meleeAttackTimeCounter;
    public float meleeAttackCooldown = 4f;

    public Transform attackPoint;
    public LayerMask layers;
    public float meleeAttackColliderRadius = 3f;


    [SerializeField]
    private float playerLosingTimeCounter = 0f;
    public Rigidbody playerRb;
    public bool playerOnSight = false;
    public bool bothArmsDestroyed = false;
    public bool movingToPlayer = false;
    public Color angryEyeColor;
    public Color normalEyeColor;
    public GameObject[] eyes;
    public EnemyArm[] arms;
    private Rigidbody enemyRb;
    private NavMeshAgent agent;
    private Animator bossAnimator;
    private Vector3 playerLastSeenPos;

    private bool inAnimation = false;



    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        bossAnimator = GetComponent<Animator>();
        normalEyeColor = eyes[0].GetComponent<Renderer>().material.color;
        rocketTimeCounter = rocketCooldown - 1f;

    }

    //void Update()
    //{
    //    lookAtPlayer();
    //    //transform.LookAt(playerRb.position);
    //}

    void Update()
    {

        if (GameCoordinator.instance.isPaused)
        {
            return;
        }


        rocketTimeCounter += Time.deltaTime;
        meleeAttackTimeCounter += Time.deltaTime;

        checkIsPlayerOnSight();

        //Incrementing that but when i see player, i reset that counter.
        playerLosingTimeCounter += Time.deltaTime;

        if (!bothArmsDestroyed)
        {
            if (playerOnSight)
            {
                lookAtPlayer();
                float distanceBetweenPlayer = Vector3.Distance(transform.position, playerRb.position);
                if (distanceBetweenPlayer >= minRocketLaunchRange)
                {
                    enemyRb.velocity = Vector3.zero;
                    if (rocketTimeCounter >= rocketCooldown)
                    {
                        agent.isStopped = true; ;
                        lookAtPlayer();
                        StartCoroutine(fireRockets());
                        rocketTimeCounter = 0f;
                        agent.isStopped = false;
                    }
                }
                checkIsPlayerLost();
            }

        }
        else
        {
            float distanceBetweenPlayer = Vector3.Distance(transform.position, playerRb.position);
            if (distanceBetweenPlayer > meleeRange && !movingToPlayer)
            {
                agent.isStopped = false; //Bunu 
                agent.SetDestination(playerRb.position);
                movingToPlayer = true;
            }
            else
            {
                if (meleeAttackTimeCounter >= meleeAttackCooldown) //VE ARALARINDAKI ACI BELIRLI BR DERECEDEN KUCUKSE
                {
                    movingToPlayer = false;
                    agent.isStopped = true;
                    lookAtPlayer();
                    inAnimation = true;
                    bossAnimator.SetTrigger("SmashAttack");
                    //meleeAttack();

                }

            }
        }
    }

    public void lookAtPlayer()
    {
        //EĞER SMASH ATTACK VS ATIYORSA BU METOD DURACAK
        if (inAnimation) return;

        Vector3 direction = playerRb.position - transform.position;
        Quaternion lookToRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookToRotation, rotateSpeed * Time.deltaTime);

    }
    public void smashAttack()
    {
        Debug.Log("Called smashAttack");

        Collider[] hit = Physics.OverlapSphere(attackPoint.position, meleeAttackColliderRadius, layers);

        foreach (Collider player in hit)
        {
            PlayerStats script = player.GetComponent<PlayerStats>();
            if (script != null)
            {

                script.takeDamage(smashDamage);
            }
            /*Rigidbody objectRb = player.GetComponent<Rigidbody>();
            if (objectRb != null)
            {
                objectRb.AddExplosionForce(meleeKnockbackForce, attackPoint.transform.position, meleeAttackColliderRadius,100f,ForceMode.VelocityChange);
            }*/

        }
     
        bossAnimator.ResetTrigger("SmashAttack");
        meleeAttackTimeCounter = 0f;
        inAnimation = false;
    }


    public void checkIsPlayerOnSight()
    {
        //If player is in the angle of the sight of the enemy.
        float currentAngle = Vector3.Angle(playerRb.position - transform.position, transform.forward);
        if (currentAngle <= enemySightAngle)
        {
            //If there is anything between enemy and player like a wall or something so i cast a ray and check if that hits player except than a wall or something.
            //Also check if player is in the enemy sight range.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerRb.transform.position - transform.position, out hit, enemySightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerSeen();
                }
            }
        }
    }
    public void checkIsPlayerLost()
    {
        //Player can be lost in two ways. In both ways, time must be passed for enemy to return normal.
        //First : Player could escape from sight angle of the enemy.
        //Second :  Player could go far away and gets out of enemy range.

        RaycastHit hit;



        if (playerLosingTimeCounter >= enemyReturningNormalTime)
        {
            //Calculating angle between enemy and player.
            float currentAngle = Vector3.Angle(playerRb.position, transform.forward);

            if (currentAngle > enemySightAngle)
            {
                //Check the distance, also if player is hid behind some walls or something.
                playerLost();
            }
            if (Physics.Raycast(transform.position, playerRb.position - transform.position, out hit, enemySightRange))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    playerLost();
                }
            }
        }

    }
    public void playerSeen()
    {
        playerOnSight = true;
        playerLastSeenPos = playerRb.position;




        playerLosingTimeCounter = 0f; //Saw the player so cooldown counter resets.

        //Turning the eye color to red.
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].GetComponent<Renderer>().material.color = angryEyeColor;
        }

        // agent.SetDestination(playerPos.position); //Chasing the player
        GameCoordinator.instance.playerIsBeingChased();





        //Harekete geçmesi için yarım saniye falan delay koy yoksa çok op.
    }
    public void playerLost()
    {
        playerOnSight = false;
        GameCoordinator.instance.playerIsSafe();
        //Turning the eye color to normal.
        for (int i = 0; i < eyes.Length; i++)
        {
            eyes[i].GetComponent<Renderer>().material.color = normalEyeColor;
        }
        agent.isStopped = false;
        agent.SetDestination(playerLastSeenPos);
    }
    public IEnumerator fireRockets()
    {

        if (bothArmsDestroyed)
        {
            yield break;
        }

        if (!bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("RocketLauncherSetUp"))
        {
            bossAnimator.SetBool("deployLaunchers", true);
            yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length + bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.25f);
        }


        int i = 0;
        foreach (EnemyArm arm in arms)
        {
            if (arm != null)
            {
                arm.fireRocket(playerRb.position);
                yield return new WaitForSeconds(timeBetweenRockets);
            }
            else
            {
                i++;
                if (i == arms.Length)
                {
                    bothArmsDestroyed = true;
                    bossAnimator.SetBool("deployLaunchers", false);
                    //bossAnimator.SetBool("pullSword", true);
                    bossAnimator.SetTrigger("SwordTrigger");
                }
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackColliderRadius);
    }


}
