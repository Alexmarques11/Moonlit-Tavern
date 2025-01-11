using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;
    public float attackInterval = 1f;
    private float lastAttackTime = 0f;

    public Animator animator;
    public GameObject damageHitbox;
    public bool isAttacking { get; private set; }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (damageHitbox != null)
        {
            damageHitbox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damageHitbox != null && damageHitbox.activeSelf)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.health -= damage;
                }
            }
            else if ((other.CompareTag("Building") || other.CompareTag("Tavern")) && other.tag != "InactiveBuilding")
            {
                Building building = other.GetComponent<Building>();
                if (building != null)
                {
                    building.TakeDamage(damage);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (Time.time >= lastAttackTime + attackInterval && !isAttacking)
        {
            if ((other.gameObject.CompareTag("Player")) ||
                ((other.gameObject.CompareTag("Building") || other.gameObject.CompareTag("Tavern")) && other.gameObject.tag != "InactiveBuilding"))
            {
                isAttacking = true;
                lastAttackTime = Time.time;
                animator.SetBool("isAttacking", true);
            }
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
