using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : StateMachineBehaviour
{
    private Transform mace;
    private Transform maceKnob;
    private SpringJoint joint;
    private float timeAvailableToInitNextAttack;
    private float animationLength;
    private float counter = 0f;

    public int attackId = 1;
    public float maceSpeed;
    public float offsetX;
    public float zPositionRelativeToTheCamera;
    public float percentageRequiredToRegisterNextAttack = 0.5f;

   // OnStateMachineEnter denenebilir.

    private Vector3 maceKnobConnectedAnchor;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WeaponHolster.instance.parentMace();
        initTransforms();
        saveJoint();
        destroyJoint();

        animator.SetBool("GoingForNextAttack", false);

        animationLength = stateInfo.length;
        timeAvailableToInitNextAttack = animationLength * percentageRequiredToRegisterNextAttack;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("GoingForNextAttack", true);
        }


        Vector3 nextPos = Vector3.zero;
        Transform cameraPos = Camera.main.transform;
        if (counter <= animationLength / 2f)
        {
            nextPos = cameraPos.forward * zPositionRelativeToTheCamera + cameraPos.position + cameraPos.right * offsetX;
        }
        else if (counter > animationLength / 2f)
        {
            nextPos = cameraPos.position - cameraPos.right * offsetX;
        }
        maceKnob.position = Vector3.MoveTowards(maceKnob.position, nextPos, maceSpeed * Time.deltaTime);
        counter += Time.deltaTime;

     
        

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WeaponHolster.instance.unParentMace();
        addJoint();
        counter = 0f;
    }

    private void addJoint()
    {
        Rigidbody maceHandleRb = mace.GetComponentInChildren<Rigidbody>();

        SpringJoint addedJoint = maceKnob.gameObject.AddComponent<SpringJoint>();
        if (addedJoint)
        {
            addedJoint.autoConfigureConnectedAnchor = false;
            addedJoint.spring = 50f;
            addedJoint.damper = 0f;

            addedJoint.connectedAnchor = maceKnobConnectedAnchor;
            addedJoint.connectedBody = maceHandleRb;
        }


    }

    public void initTransforms()
    {
        foreach (Transform child in WeaponHolster.instance.transform)
        {
            if (child.name == "Mace")
            {
                mace = child;
            }
            else if (child.name == "JunctionMace")
            {
                maceKnob = child;
            }
        }
    }


    public void destroyJoint()
    {
        SpringJoint maceJoint = maceKnob.GetComponent<SpringJoint>();
        if (maceJoint)
        {
            Destroy(maceJoint);
        }
    }

    public void saveJoint()
    {
        SpringJoint maceJoint = maceKnob.GetComponent<SpringJoint>();
        if (maceJoint)
        {
            maceKnobConnectedAnchor = maceJoint.connectedAnchor;
        }
    }

}
