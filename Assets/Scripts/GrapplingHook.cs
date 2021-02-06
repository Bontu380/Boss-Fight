using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    enum HookState {Idle,HookOnItsWay,PullingPlayer};

    private HookState hookState;
    private Vector3 targetHitPosition;
    
    public GameObject hook;
    public LineRenderer hookRenderer;
    public LayerMask grappleAvailableLayer;
    public Transform hookHolster;

    public float hookDistance = 100f;
    public float releaseHookOffset = 5f;
    public float hookGrabOffset = 1f;

    public float hookGoingSpeed = 20f;
    public float hookPullingSpeed = 10f;

    public Rigidbody playerRb;




    void Start()
    {
        hookState = HookState.Idle;
        hookRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(hookState == HookState.Idle)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position,transform.forward, out hit, hookDistance, grappleAvailableLayer))
                {
                    targetHitPosition = hit.point;                   
                    hook.SetActive(true);

                    setLineRenderer();
                    hookRenderer.enabled = true;

                    hookState = HookState.HookOnItsWay;
                }
               
            }
        }



        if(hookState == HookState.Idle)
        {
            return;
        }
        else if(hookState == HookState.HookOnItsWay)
        {
            Vector3 nextPos = Vector3.MoveTowards(hook.transform.position, targetHitPosition, Time.deltaTime * hookGoingSpeed);
            hook.transform.position = nextPos;

            float distanceBetweenHookAndTarget = Vector3.Distance(hook.transform.position, targetHitPosition);
            if (distanceBetweenHookAndTarget <= hookGrabOffset)
            {
                hookState = HookState.PullingPlayer;
            }
        }
        else if(hookState == HookState.PullingPlayer)
        {
    
            Vector3 nextPos = Vector3.MoveTowards(playerRb.position, targetHitPosition, Time.deltaTime * hookPullingSpeed);

            playerRb.MovePosition(nextPos);
          
            float distanceBetweenPlayerAndTarget = Vector3.Distance(playerRb.position, targetHitPosition);
            if (distanceBetweenPlayerAndTarget <= releaseHookOffset)
            {
                resetHook();             
                hookState = HookState.Idle;
            }
        }

           
    }

    private void LateUpdate()
    {
        if(hookState == HookState.Idle)
        {
            return;
        }
        else if(hookState == HookState.HookOnItsWay)
        {
            //Hook targete dogru gidecek
            hookRenderer.SetPosition(0, hookHolster.position);
            hookRenderer.SetPosition(1, hook.transform.position);
            
        }
        else if(hookState == HookState.PullingPlayer)
        {
            //Player targete dogru gidecek
            hookRenderer.SetPosition(0, hookHolster.position);
        }

    }

    public void setLineRenderer()
    {
        hookRenderer.positionCount = 2;
        hookRenderer.SetPosition(0, hook.transform.position);
        hookRenderer.SetPosition(1, hook.transform.position);
    }

    public void resetHook()
    {
        hookRenderer.enabled = false;
        hook.SetActive(false);
        hook.transform.position = hookHolster.position;
        hook.transform.rotation = Quaternion.Euler(hookHolster.rotation.eulerAngles.x, hookHolster.rotation.eulerAngles.y, hookHolster.rotation.eulerAngles.z);
    }

}
