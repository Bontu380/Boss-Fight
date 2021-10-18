using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindGun : MonoBehaviour
{
    private bool isRewinding;
    private Rewindable hitRewindableTarget;


 
    void Update() //Line renderer will be added after functionality is done, it can be done in LateUpdate maybe.
    {
        if (Input.GetMouseButton(0) && !isRewinding)
        {
            Transform camPos = Camera.main.transform;

            RaycastHit hit;
            if(Physics.Raycast(camPos.position, camPos.forward, out hit))
            {
                Debug.Log(hit.transform.name);
                Rewindable candidate = hit.transform.GetComponent<Rewindable>();
                if (candidate)
                {
                    hitRewindableTarget = candidate;
                    hitRewindableTarget.startRewind();
                    isRewinding = true;
                }    
                
            }
        }
        else if (Input.GetMouseButtonUp(0) && isRewinding)
        {
            hitRewindableTarget.stopRewind();
            hitRewindableTarget = null;
            isRewinding = false;
        }
    }
}
