using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaffWeapon : MonoBehaviour
{
    public GameObject spell;
    public Transform FirePoint;
    public bool canFire;
    private float timer;
    public float timeBetweenShots;
    public float weaponDmg = 1f;
    public int weaponLevel = 1;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenShots)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Instantiate(spell, FirePoint.position, Quaternion.identity);
        }
    }

    public void UpgradeWeapon()
    {
        weaponDmg += 1;
        weaponLevel++;
        Debug.Log("Weapon Upgraded! Damage: " + weaponDmg);
    }
}
