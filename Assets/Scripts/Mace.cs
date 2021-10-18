using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MeleeWeapon
{

    public Transform junctionMaceRigidbody;
    public Transform junctionHandleRigidbody;

    public LineRenderer lineRenderer;

    private Vector3 maceKnobDefaultPosition;

    private bool maceKnobInitiated = false;
    private void OnEnable()
    {
        //Burada animasyon da oynatılabilir. Silahı çekme animasyonu
        if (!maceKnobInitiated)
        {
            float distance = Vector3.Distance(junctionMaceRigidbody.position, junctionHandleRigidbody.position);
            Vector3 direction = junctionMaceRigidbody.position - junctionHandleRigidbody.position;
            maceKnobDefaultPosition = junctionHandleRigidbody.position + (distance * direction);

         

            junctionMaceRigidbody.gameObject.SetActive(false);
            gameObject.SetActive(false);
            maceKnobInitiated = true;
            return;

        }


        if(WeaponHolster.instance.maceKnob)
        {

            WeaponHolster.instance.unParentMace();
            WeaponHolster.instance.maceKnob.gameObject.SetActive(true);
            junctionMaceRigidbody.position = maceKnobDefaultPosition;

        }
    }

    private void OnDisable()
    {
        if (WeaponHolster.instance.maceKnob)
        {
            WeaponHolster.instance.maceKnob.gameObject.SetActive(false);
            WeaponHolster.instance.parentMace();
        }       
    }
    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, junctionHandleRigidbody.position);
        lineRenderer.SetPosition(1, junctionMaceRigidbody.position);    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
