using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rewindable : MonoBehaviour
{

    private Rigidbody objectRb;
    private NavMeshAgent agent;
    private bool isRewinding = false;
    private PointInTime[] pointsInTime;
    private float counter = 0f;

    public float timeRewindLimit = 5f; //In seconds.


    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
          

        int arrayLength = (int)(timeRewindLimit / Time.fixedDeltaTime );
       // Debug.Log(arrayLength);
        pointsInTime = new PointInTime[arrayLength];
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewinding)
        {

        }
 
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            return;
        }

        if (counter >= timeRewindLimit)
        {
            counter = 0f;
        }
        else
        {
            //Debug.Log((int)(counter / Time.fixedDeltaTime));
            PointInTime point = new PointInTime(transform.position, transform.rotation);
            pointsInTime[(int)counter] = point;
            counter += Time.fixedDeltaTime;
        }
    }

    public void startRewind()
    {
        if (objectRb)
        {
            objectRb.isKinematic = true;
        }
        if (agent)
        {
            agent.enabled = true;
        }
        isRewinding = true;
    }

    public void stopRewind()
    {
        if (objectRb)
        {
            objectRb.isKinematic = false;
        }
        if (agent)
        {
            agent.enabled = false;
        }
        isRewinding = false;
    }
}
