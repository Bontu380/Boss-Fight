using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrapplingHook : MonoBehaviour
{
    enum HookState { Idle, HookOnItsWay, PullingPlayer };

    public Animator animator;

    [SerializeField]
    private HookState hookState;
    private Vector3 targetHitPosition;


    public GameObject hook;
    public Transform hookCollisionPoint;
    public Transform hookCablePoint;

    public LineRenderer hookRenderer;
    public LayerMask grappleAvailableLayer;

    private Vector3 localIdlePosition;
    public Transform parent;

    public float hookCooldown = 3f;

    private float hookTimeCounter;

    public float hookDistance = 100f;
    public float releaseHookOffset = 5f;
    public float hookGrabOffset = 1f;

    public float hookGoingSpeed = 20f;
    public float hookPullingSpeed = 10f;

    public Rigidbody playerRb;

    public Transform hookHoldingPointOnArm;

    private Vector3 hookOriginalRotation, hookOriginalScale;

    public Image hookCooldownImage;

    void Start()
    {
        hookState = HookState.Idle;
        hookRenderer.enabled = false;

        hookTimeCounter = hookCooldown;

        hookCooldownImage.fillAmount = hookTimeCounter / hookCooldown;

        hookOriginalRotation = hook.transform.rotation.eulerAngles;
        hookOriginalScale = hook.transform.localScale;
    }

    void Update()
    {
        hookTimeCounter += Time.deltaTime;
        hookCooldownImage.fillAmount = hookTimeCounter / hookCooldown;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (hookState == HookState.Idle && hookTimeCounter >= hookCooldown)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, hookDistance,grappleAvailableLayer))
                {                   
                    {
                        targetHitPosition = hit.point;
                        hook.SetActive(true);
                        hookRenderer.SetPosition(0, parent.position);
                        hookRenderer.SetPosition(1, hookCablePoint.position);
                        hookRenderer.enabled = true;
                        GameCoordinator.instance.playerMovingInputUnavailable = true;
                        GameCoordinator.instance.playerShootingInputUnavailable = true;
                        playerRb.useGravity = false;
                        playerRb.velocity = Vector3.zero;
                        animator.SetBool("GrapplingWithHook", true);

                    }
                }

            }
        }



        if (hookState == HookState.Idle)
        {
            return;
        }
        else if (hookState == HookState.HookOnItsWay)
        {
            Vector3 nextPos = Vector3.MoveTowards(hook.transform.position, targetHitPosition, Time.deltaTime * hookGoingSpeed);

            hook.transform.position = nextPos;
            hookRenderer.SetPosition(0, parent.position);
            hookRenderer.SetPosition(1, hookCablePoint.position);

            //float distanceBetweenHookAndTarget = Vector3.Distance(hookCollisionPoint.position, targetHitPosition);

            float distanceBetweenHookAndTarget = Vector3.Distance(hook.transform.position, targetHitPosition);
            if (distanceBetweenHookAndTarget <= hookGrabOffset)
            {
                hookState = HookState.PullingPlayer;

            }
        }
        else if (hookState == HookState.PullingPlayer)
        {

            Vector3 nextPos = Vector3.MoveTowards(playerRb.position, targetHitPosition, Time.deltaTime * hookPullingSpeed);
            playerRb.MovePosition(nextPos);
            hookRenderer.SetPosition(0, parent.position);

            float distanceBetweenPlayerAndTarget = Vector3.Distance(playerRb.position, targetHitPosition);
            if (distanceBetweenPlayerAndTarget <= releaseHookOffset)
            {

      
               // playerRb.useGravity = true;
               // animator.SetBool("GrapplingWithHook", false);
       
                resetHook();
           

            }
        }


    }


    public void resetHook()
    {
        GameCoordinator.instance.playerMovingInputUnavailable = false;
        GameCoordinator.instance.playerShootingInputUnavailable = false;
        playerRb.useGravity = true;
        animator.SetBool("GrapplingWithHook", false);

        hookRenderer.enabled = false;
        hook.SetActive(false);

        hook.transform.position = hookHoldingPointOnArm.position ;
        hook.transform.parent = parent;

        //hook.transform = hookOriginalTransform;

        hookRenderer.enabled = false;
        hook.transform.localScale = hookOriginalScale;
        hook.transform.localEulerAngles = hookOriginalRotation;
        hookState = HookState.Idle;
        hookTimeCounter = 0f;
        //hook.transform.localRotation = Quaternion.Euler(hookOriginalRotation.x, hookOriginalRotation.y, hookOriginalRotation.z);


        // hook.transform.localRotation = Quaternion.Euler(hookLocalTransform.localEulerAngles.x, hookLocalTransform.localEulerAngles.y, hookLocalTransform.localEulerAngles.z);


    }
    public void setHookOnItsWay()
    {
        hook.transform.LookAt(targetHitPosition);
        hook.transform.parent = null;
        hookState = HookState.HookOnItsWay;
    }


}
