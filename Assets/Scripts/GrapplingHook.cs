using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    enum HookState { Idle, HookOnItsWay, PullingPlayer };

    public Animator animator;

    private HookState hookState;
    private Vector3 targetHitPosition;


    public GameObject hook;
    public Transform hookCollisionPoint;
    public Transform hookCablePoint;

    public LineRenderer hookRenderer;
    public LayerMask grappleAvailableLayer;
    public Transform hookHolster;

    public float hookDistance = 100f;
    public float releaseHookOffset = 5f;
    public float hookGrabOffset = 1f;

    public float hookGoingSpeed = 20f;
    public float hookPullingSpeed = 10f;

    public Rigidbody playerRb;

    public Transform hookLocalTransform;

    void Start()
    {
        hookState = HookState.Idle;
        hookRenderer.enabled = false;

        Debug.Log(hookLocalTransform.localPosition);
        Debug.Log(hookLocalTransform.localEulerAngles);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (hookState == HookState.Idle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, hookDistance))
                {
                    if (((1 << hit.transform.gameObject.layer) & grappleAvailableLayer) != 0)
                    {
                        targetHitPosition = hit.point;


                        hook.transform.LookAt(hit.point);

                        //hook.transform.parent = null;
                        hookRenderer.enabled = true;

                        animator.SetBool("GrapplingWithHook", true);

                        //hookState = HookState.HookOnItsWay;
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
            hookRenderer.SetPosition(0, hookHolster.position);
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
            hookRenderer.SetPosition(0, hookHolster.position);

            float distanceBetweenPlayerAndTarget = Vector3.Distance(playerRb.position, targetHitPosition);
            if (distanceBetweenPlayerAndTarget <= releaseHookOffset)
            {

                hookState = HookState.Idle;

                animator.SetBool("GrapplingWithHook", false);
       
                resetHook();
           

            }
        }


    }


    public void resetHook()
    {


        hookRenderer.enabled = false;
      

        hook.transform.parent = hookHolster.transform;
        hook.transform.localPosition = hookLocalTransform.localPosition;
        hook.transform.localRotation = Quaternion.Euler(hookLocalTransform.localEulerAngles.x, hookLocalTransform.localEulerAngles.y, hookLocalTransform.localEulerAngles.z);
     

    }
    public void setHookOnItsWay()
    {
        hook.transform.parent = null;
        //hookState = HookState.HookOnItsWay;
    }


}
