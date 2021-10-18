using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInTime
{
    private Vector3 position;
    private Quaternion rotation;

    public PointInTime(Vector3 position,Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
