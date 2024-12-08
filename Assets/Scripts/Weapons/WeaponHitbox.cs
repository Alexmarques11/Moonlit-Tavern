using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hitbox collided with an enemy: " + collision.gameObject.name);

            Vector2 knockbackDirection;
            if (FindObjectOfType<WeaponSwitching>().selectedWeapon == 0)
            {
                Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
                knockbackDirection = (collision.transform.position - playerTransform.position).normalized;
            }
            else
            {
                knockbackDirection = (collision.transform.position - transform.position).normalized;
            }

            // Aplica o dano e o knockback
            if (FindObjectOfType<WeaponSwitching>().selectedWeapon == 0)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(FindObjectOfType<AxeWeapon>().weaponDmg, knockbackDirection);
            }
            else if (FindObjectOfType<WeaponSwitching>().selectedWeapon == 1)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(FindObjectOfType<CrossbowWeapon>().weaponDmg, knockbackDirection);
            }
            else if (FindObjectOfType<WeaponSwitching>().selectedWeapon == 2)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(FindObjectOfType<StaffWeapon>().weaponDmg, knockbackDirection);
            }
        }
    }
}
