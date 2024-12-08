using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private float projectileSpeed = 10f;
    private float projectileDmg = 5f;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(projectileDmg, knockbackDirection);

            Destroy(gameObject);
        }
    }
}
