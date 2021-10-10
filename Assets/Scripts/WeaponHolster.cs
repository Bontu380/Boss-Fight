using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    public static WeaponHolster instance;

    public Transform maceKnob;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void parentMace()
    {
        maceKnob.transform.parent = transform;
    }

    public void unParentMace()
    {
        maceKnob.transform.parent = null;
    }
}
