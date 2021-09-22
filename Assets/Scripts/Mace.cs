using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{

    public Transform junctionMaceRigidbody;
    public Transform junctionHandleRigidbody;

    public LineRenderer lineRenderer;


    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, junctionHandleRigidbody.position);
        lineRenderer.SetPosition(1, junctionMaceRigidbody.position);    
    }
}
