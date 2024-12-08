using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public bool crossbowUnlocked = false;
    public bool staffUnlocked = false;

    void Start()
    {
        SelectedWeapon();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeapon = GetNextWeaponIndex(selectedWeapon + 1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeapon = GetPreviousWeaponIndex(selectedWeapon - 1);
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
    }

    int GetNextWeaponIndex(int index)
    {
        int totalWeapons = transform.childCount;

        for (int i = 0; i < totalWeapons; i++)
        {
            index = index % totalWeapons;
            if (IsWeaponUnlocked(index))
            {
                return index;
            }
            index++;
        }
        return selectedWeapon;
    }

    int GetPreviousWeaponIndex(int index)
    {
        int totalWeapons = transform.childCount;


        for (int i = 0; i < totalWeapons; i++)
        {
            index = (index + totalWeapons) % totalWeapons;
            if (IsWeaponUnlocked(index))
            {
                return index;
            }
            index--;
        }
        return selectedWeapon;
    }

    bool IsWeaponUnlocked(int index)
    {
        if (index == 1 && !crossbowUnlocked) return false;
        if (index == 2 && !staffUnlocked) return false;
        return true;
    }

    public void SelectedWeapon()
    {
        int i = 0;

        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }

    public void UnlockCrossbow()
    {
        crossbowUnlocked = true;
    }

    public bool IsCrossbowUnlocked()
    {
        return crossbowUnlocked;
    }

    public void UnlockStaff()
    {
        staffUnlocked = true;
    }

    public bool IsStaffUnlocked()
    {
        return staffUnlocked;
    }
}
