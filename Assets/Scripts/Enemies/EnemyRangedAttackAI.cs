using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAI : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public float moveSpeed;
    private Transform target;
    public Vector3 targetPositionOffset;
    public float detectionRadius = 5f;
    public float playerDetectionRadius = 20f;
    public float stoppingDistance = 0.5f;
    private bool isFlipped = false;
    public SpriteRenderer enemyRenderer;
    public Animator animator;


    private EnemyRangedAttack rangedAttack;

    void Start()
    {
        FindTavern();
        rangedAttack = GetComponent<EnemyRangedAttack>();
    }

    void Update()
    {
        DetectPlayerNearby();

        if (target == null || target.CompareTag("Player") == false)
        {
            if (target != null && target.CompareTag("InactiveBuilding"))
            {
                target = null;
            }

            DetectBuildingsNearby();
        }


        if (target == null) FindTavern();

        MoveTowardsTarget();

        if (rangedAttack != null && target != null)
        {
            rangedAttack.SetTarget(target, targetPositionOffset);
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + targetPositionOffset;
            Vector3 direction = targetPosition - transform.position;

            if (direction.magnitude <= stoppingDistance)
            {
                theRigidbody.velocity = Vector2.zero;
                animator.SetBool("isMoving", false);
            }
            else
            {
                theRigidbody.velocity = direction.normalized * moveSpeed;
                animator.SetBool("isMoving", true);
            }

            if (direction.x > 0 && isFlipped)
            {
                Flip();
            }
            else if (direction.x < 0 && !isFlipped)
            {
                Flip();
            }
        }
        else
        {
            theRigidbody.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
    }


    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 localScale = enemyRenderer.transform.localScale;
        localScale.x *= -1;
        enemyRenderer.transform.localScale = localScale;
    }

    private void DetectPlayerNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius);
        bool playerInRange = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                target = hit.transform;
                targetPositionOffset = Vector3.zero;
                playerInRange = true;
                return;
            }
        }

        if (!playerInRange && target != null && target.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void DetectBuildingsNearby()
    {
        Collider2D[] buildings = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D building in buildings)
        {
            if (building.gameObject.CompareTag("Building") && building.gameObject.tag != "InactiveBuilding")
            {
                target = building.transform;

                Collider2D buildingCollider = building.GetComponent<Collider2D>();
                if (buildingCollider != null)
                {
                    targetPositionOffset = buildingCollider.bounds.center - building.transform.position;
                    return;
                }
            }
        }
    }


    private void FindTavern()
    {
        if (target == null || target.gameObject.name != "Tavern")
        {
            GameObject tavernObject = GameObject.Find("Tavern");
            if (tavernObject != null)
            {
                target = tavernObject.transform;

                Collider2D tavernCollider = tavernObject.GetComponent<Collider2D>();
                if (tavernCollider != null)
                {
                    targetPositionOffset = tavernCollider.bounds.center - tavernObject.transform.position;
                }
                else
                {
                    targetPositionOffset = Vector3.zero;
                }
            }
            else
            {
                target = null;
                Debug.LogWarning("Tavern object not found in the scene!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
