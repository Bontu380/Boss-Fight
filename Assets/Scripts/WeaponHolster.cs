﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    public static WeaponHolster instance;
    public Transform maceKnob;
    public Animator playerAnim;

    //Asagidaki ikisi private olacak tamamlandıktan sonra
    private int currentActiveWeaponIndex = 0;
    private List<Transform> weapons;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        initWeapons();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // > 0 means scrolled up.
        {
            switchWeapon((currentActiveWeaponIndex + 1) % weapons.Count);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) // < 0 scrolled down.
        {
            if (currentActiveWeaponIndex == 0)
            {
                switchWeapon(weapons.Count - 1);
            }
            else
            {
                switchWeapon(currentActiveWeaponIndex - 1);
            }
   
        }
    }
    public void parentMace()
    {
        if (maceKnob.gameObject.activeSelf)
        {
            maceKnob.transform.parent = transform;
        }
    }

    public void unParentMace()
    {
        if (maceKnob)
        {
            maceKnob.transform.parent = null;
        }
    }

    public void switchWeapon(int index)
    {

        
        AnimatorStateInfo currentStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (currentStateInfo.IsName("MeleeStateIdle") || currentStateInfo.IsName("PlayerIdleUzi"))
        {
            //Debug.Log(currentActiveWeaponIndex);
            weapons[currentActiveWeaponIndex].gameObject.SetActive(false);
            weapons[index].gameObject.SetActive(true);
            currentActiveWeaponIndex = index;

            if (weapons[currentActiveWeaponIndex].CompareTag("Gun"))
            {
                playerAnim.SetBool("inMeleeState", false);
            }
            else
            {
                playerAnim.SetBool("inMeleeState", true);
            }
            PlayerCombat.instance.getActiveWeapon();
        }
    }

    public void initWeapons()
    {
        weapons = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Gun") || child.CompareTag("MeleeWeapon"))
            {
                weapons.Add(child);
                if (child.gameObject.activeSelf)
                {
                    currentActiveWeaponIndex = i;
                }
            }
        }
    }
}
