using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwing : MonoBehaviour
{

    public Transform AxeAttackRange;
    public Animator anim;

    public int CurrentWeaponNumber;

    public bool CanSwing = true;

    void changeWeapon()
    {
        if (CurrentWeaponNumber == 0)
        {
            CurrentWeaponNumber += 1;
            anim.SetLayerWeight(CurrentWeaponNumber - 1, 0);
            anim.SetLayerWeight(CurrentWeaponNumber, 1);
        }
        else
        {
            CurrentWeaponNumber -= 1;
            anim.SetLayerWeight(CurrentWeaponNumber + 1, 0);
            anim.SetLayerWeight(CurrentWeaponNumber, 1);
        }
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused == true)
        {
            CanSwing = false;
        }
        else
        {
            CanSwing = true;
        }
        
        if (Input.GetKeyDown(KeyCode.C) && CanSwing == true)
        {
            
            anim.SetTrigger("SwitchToGun");
            changeWeapon();
        }

        if (Input.GetMouseButtonDown(0) && CurrentWeaponNumber == 1 && CanSwing == true)
        {
            anim.SetTrigger("Attack");
            FindObjectOfType<AudioManager>().Play("Axe Swing");
        }

        if (CurrentWeaponNumber == 1 && Input.GetKeyDown(KeyCode.C))
        {
            FindObjectOfType<AudioManager>().Play("Switch");
        }
    }
}
