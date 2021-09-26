using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : StateMachineBehaviour
{

    private bool isGoingForNextAttack = false;
    private Transform mace;
    private Transform maceKnob;
    private SpringJoint joint;

    public float maceSpeed;
    public float offsetX;
    public float zPositionRelativeToTheCamera;

    private float animationLength;

    private float counter = 0f;

    private Vector3 maceKnobConnectedAnchor;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WeaponHolster.instance.parentMace();
        initTransforms();
        saveJoint();
        destroyJoint();

        animationLength = stateInfo.length;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(0))
        {
            isGoingForNextAttack = true;
        }


        //Vector3 direction = mace.position - Camera.main.transform.position;

        //Vector3 nextPos = Camera.main.transform.forward  + Camera.main.transform.position + direction;
        //maceKnob.position = Vector3.MoveTowards(maceKnob.position, nextPos, Time.deltaTime * maceSpeed);


        //y = cameraPos.position.y

        Vector3 nextPos = Vector3.zero;
        Transform cameraPos = Camera.main.transform;
        if (counter <= animationLength/2f)
        {
            nextPos = cameraPos.forward * zPositionRelativeToTheCamera + cameraPos.position + cameraPos.right * offsetX;
        }
        else if(counter > animationLength/2f)
        {
            nextPos =  cameraPos.position - cameraPos.right * offsetX;
        }
        maceKnob.position = Vector3.MoveTowards(maceKnob.position, nextPos, maceSpeed * Time.deltaTime);
        counter += Time.deltaTime;


    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        WeaponHolster.instance.unParentMace();
        addJoint();
        counter = 0f;
        if (isGoingForNextAttack)
        {
            animator.SetTrigger("MeleeAttack2"); //Buradaki 2 yi kodla falan çekip standart haline getirebilirsin.
        }
    }

    private void addJoint()
    {
        Rigidbody maceHandleRb = mace.GetComponentInChildren<Rigidbody>();

        SpringJoint addedJoint = maceKnob.gameObject.AddComponent<SpringJoint>();
        addedJoint.autoConfigureConnectedAnchor = false;
        addedJoint.spring = 50f;
        addedJoint.damper = 0f;

        addedJoint.connectedAnchor = maceKnobConnectedAnchor;
        addedJoint.connectedBody = maceHandleRb;



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
        Destroy(maceJoint);
    }

    public void saveJoint()
    {
        SpringJoint maceJoint = maceKnob.GetComponent<SpringJoint>();
        maceKnobConnectedAnchor = maceJoint.connectedAnchor;
        Debug.Log(maceKnobConnectedAnchor);
    }

}
