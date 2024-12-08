using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float projectileLifetime = 3f;

    private Transform target;
    private Vector3 targetPositionOffset;
    private float lastAttackTime;
    public Animator animator;

    public void SetTarget(Transform newTarget, Vector3 newTargetPositionOffset)
    {
        target = newTarget;
        targetPositionOffset = newTargetPositionOffset;
    }

    void Update()
    {
        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                if (GetComponent<Rigidbody2D>().velocity.magnitude == 0)
                {
                    animator.SetBool("isAttacking", true);
                    TryAttack();
                }
            }
        }
    }

    private void TryAttack()
    {
        ShootProjectile();
        lastAttackTime = Time.time;
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = (target.position + targetPositionOffset - transform.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        Destroy(projectile, projectileLifetime);
    }

    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
