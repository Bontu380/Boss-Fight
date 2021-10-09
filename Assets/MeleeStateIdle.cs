using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeStateIdle : StateMachineBehaviour
{
    //public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.SetBool("GoingForNextAttack", false);
    //}
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("GoingForNextAttack",true);
        }
    }


}
