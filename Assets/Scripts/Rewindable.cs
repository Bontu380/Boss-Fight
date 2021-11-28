using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rewindable : MonoBehaviour
{

    private Rigidbody objectRb;
    private NavMeshAgent agent;
    private bool isRewinding = false;
    private List<PointInTime> pointsInTime;

    public float timeRewindLimit = 5f; //In seconds.


    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        pointsInTime = new List<PointInTime>();
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            rewind();
        }
        else
        {
            record();
        }
    }

    public void rewind()
    {

            PointInTime currentPoint = pointsInTime[0];
            if (currentPoint == null)
            {
                stopRewind();
                return;

            }
            pointsInTime.RemoveAt(0);

            transform.position = currentPoint.position;
            transform.rotation = currentPoint.rotation;
    
        
    }

    public void record()
    {
        if (pointsInTime.Count > Mathf.Round(timeRewindLimit / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void startRewind()
    {
        if (agent)
        {
            agent.enabled = false;
        }
        if (objectRb)
        {
            objectRb.isKinematic = true;
            objectRb.velocity = Vector3.zero;
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
            agent.enabled = true;
        }
        isRewinding = false;
    }
}
