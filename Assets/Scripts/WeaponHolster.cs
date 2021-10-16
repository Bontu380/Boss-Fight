using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    public static WeaponHolster instance;
    public Transform maceKnob;
    public Animator playerAnim;

    //Asagidaki ikisi private olacak tamamlandıktan sonra
    public int currentActiveWeaponIndex = 0;
    public List<Transform> weapons;
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
        if (maceKnob)
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

        //Animator mevzusu girecek buraya, şu anda oynamakta olan animasyonu kesmeli yoksa takılıyor. Mesela uzi ile ateş ederken switch atınca mace ile ateş animasyonu oluyor salakça
        AnimatorStateInfo currentStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (currentStateInfo.IsName("MeleeStateIdle") || currentStateInfo.IsName("PlayerIdleUzi"))
        {
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
            }
        }
    }
}
