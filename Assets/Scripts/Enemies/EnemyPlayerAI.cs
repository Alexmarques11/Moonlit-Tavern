using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerAI : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public float moveSpeed;
    public SpriteRenderer enemyRenderer;
    public float playerDetectionRadius = 7f;
    public float buildingDetectionRadius = 5f;

    private Transform target;
    private Vector3 targetPositionOffset;
    private bool isFlipped = false;
    private Damage damageComponent;

    public Animator animator;

    private enum State
    {
        MoveToTavern,
        MoveToBuilding,
        MoveToPlayer,
        Idle
    }

    private State currentState;

    void Start()
    {
        damageComponent = GetComponent<Damage>();
        ChangeState(State.Idle);
    }

    void Update()
    {
        if (damageComponent != null && damageComponent.isAttacking)
        {
            theRigidbody.velocity = Vector2.zero;
            return;
        }

        UpdateState();
        ExecuteState();
        MoveToTarget(target);
    }

    private void UpdateState()
    {
        if (DetectPlayer())
        {
            ChangeState(State.MoveToPlayer);
            return;
        }

        if (DetectBuildings())
        {
            ChangeState(State.MoveToBuilding);
            return;
        }

        FindTavern();
        if (target == null)
        {
            ChangeState(State.Idle);
        }
        else
        {
            ChangeState(State.MoveToTavern);
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case State.MoveToTavern:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToBuilding:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToPlayer:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.Idle:
                animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private void MoveToTarget(Transform target)
    {
        if (target == null) return;

        Vector3 direction = (target.position + targetPositionOffset - transform.position).normalized;
        theRigidbody.velocity = direction * moveSpeed;

        if (direction.x > 0 && isFlipped) Flip();
        else if (direction.x < 0 && !isFlipped) Flip();
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 localScale = enemyRenderer.transform.localScale;
        localScale.x *= -1;
        enemyRenderer.transform.localScale = localScale;
    }

    private bool DetectPlayer()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius);
        foreach (Collider2D obj in playersInRange)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                if (Vector2.Distance(transform.position, obj.transform.position) <= playerDetectionRadius)
                {
                    target = obj.transform;
                    targetPositionOffset = Vector3.zero;
                    return true;
                }
            }
        }
        return false;
    }

    private bool DetectBuildings()
    {
        Collider2D[] buildingsInRange = Physics2D.OverlapCircleAll(transform.position, buildingDetectionRadius);
        foreach (Collider2D obj in buildingsInRange)
        {
            if (obj.gameObject.CompareTag("Building") && obj.gameObject.name != "Tavern")
            {
                Collider2D buildingCollider = obj.GetComponent<Collider2D>();
                if (buildingCollider != null)
                {
                    target = obj.transform;
                    targetPositionOffset = buildingCollider.bounds.center - obj.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    private void FindTavern()
    {
        if (target == null || (target.gameObject.name != "Tavern" && target.gameObject.name != "Tavern(Clone)"))
        {
            GameObject tavernObject = GameObject.Find("Tavern");
            if (tavernObject == null)
            {
                tavernObject = GameObject.Find("Tavern(Clone)");
            }

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
                Debug.LogWarning("Neither 'Tavern' nor 'Tavern(Clone)' found in the scene!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buildingDetectionRadius);
    }
}
