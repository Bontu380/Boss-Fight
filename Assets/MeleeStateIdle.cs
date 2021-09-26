using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeStateIdle : StateMachineBehaviour
{

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("MeleeAttack1");
        }
    }


}
