using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance;
    private MeleeWeapon activeMeleeWeapon;
    private Gun activeGun;
    void Awake()
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

    public void Start()
    {
        getActiveWeapon(); //Bu normalde melee weapon swap yapınca otomatik çağır
    }

    public void performMeleeAttack()
    {
        Debug.Log("Performing melee attack");
        //if (activeMeleeWeapon)
        //{
            activeMeleeWeapon.meleeAttack();
        //}
    }
    public void getActiveWeapon()
    {
        Transform holster = WeaponHolster.instance.transform;
        for (int i = 0; i < holster.childCount; i++)
        {
            Transform child = holster.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                Debug.Log(child.name);
                if (child.CompareTag("MeleeWeapon"))
                {
                    activeMeleeWeapon = child.GetComponent<MeleeWeapon>();         
                }
                else if (child.CompareTag("Gun"))
                {
                    activeGun = child.GetComponent<Gun>();
                }
                break;
            }
        }
    }
}
