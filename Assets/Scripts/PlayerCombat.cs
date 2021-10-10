using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat instance;
    private MeleeWeapon activeMeleeWeapon;
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
        Debug.Log(activeMeleeWeapon.name);
        activeMeleeWeapon.meleeAttack();
    }
    public void getActiveWeapon()
    {
        Transform holster = WeaponHolster.instance.transform;
        for (int i = 0; i < holster.childCount; i++)
        {
            Transform child = holster.GetChild(i);
            {
                activeMeleeWeapon = child.GetComponent<MeleeWeapon>();
                break;
            }
        }
    }
}
