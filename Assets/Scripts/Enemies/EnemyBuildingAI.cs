using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildingAI : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public float moveSpeed;
    public float detectionRadius = 5f;
    public SpriteRenderer enemyRenderer;

    private Transform target;
    private Vector3 targetPositionOffset;
    private bool isFlipped = false;
    private Damage damageComponent;

    void Start()
    {
        damageComponent = GetComponent<Damage>();
    }

    void Update()
    {
        if (IsAttacking())
            StopMovement();
        else
            HandleMovementBehavior();
    }

    private bool IsAttacking()
    {
        return damageComponent != null && damageComponent.isAttacking;
    }

    private void StopMovement()
    {
        theRigidbody.velocity = Vector2.zero;
    }

    private void HandleMovementBehavior()
    {
        if (!DetectBuildingsNearby())
        {
            FindTavern();
        }

        if (target != null)
        {
            MoveToTarget();
        }
        else
        {
            StopMovement();
        }
    }

    private bool DetectBuildingsNearby()
    {
        Collider2D[] buildings = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D building in buildings)
        {
            if (building.gameObject.CompareTag("Building") && building.gameObject.name != "Tavern")
            {
                SetTarget(building);
                return true;
            }
        }
        return false;
    }

    private void FindTavern()
    {
        if (target == null || target.gameObject.name != "Tavern")
        {
            GameObject tavernObject = GameObject.Find("Tavern");
            if (tavernObject != null)
            {
                SetTarget(tavernObject.GetComponent<Collider2D>());
            }
            else
            {
                target = null;
                Debug.LogWarning("Tavern object not found in the scene!");
            }
        }
    }

    private void MoveToTarget()
    {
        Vector3 direction = (target.position + targetPositionOffset - transform.position).normalized;
        theRigidbody.velocity = direction * moveSpeed;

        HandleSpriteFlip(direction);
    }

    private void HandleSpriteFlip(Vector3 direction)
    {
        if (direction.x > 0 && isFlipped)
        {
            Flip();
        }
        else if (direction.x < 0 && !isFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 localScale = enemyRenderer.transform.localScale;
        localScale.x *= -1;
        enemyRenderer.transform.localScale = localScale;
    }

    private void SetTarget(Collider2D collider)
    {
        target = collider.transform;
        targetPositionOffset = collider.bounds.center - collider.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
