using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spikeVisual;
    public float damage = 10f;
    public float activationDuration = 0.5f;
    public float cooldownTime = 2f;
    public float reactivationDelay = 1f;

    private bool isActive = false;
    private bool isTriggered = false;
    private bool canReactivate = true;
    private float cooldownTimer = 0f;
    private Animator spikeAnimator;

    private void Start()
    {
        spikeAnimator = spikeVisual.GetComponentInChildren<Animator>();
        if (spikeAnimator == null)
        {
            Debug.LogError("Animator não encontrado");
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (cooldownTimer <= 0 && other.CompareTag("Enemy") && !isTriggered && canReactivate)
        {
            ActivateSpikes();
            DealDamage(other.gameObject);
            isTriggered = true;
            cooldownTimer = cooldownTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isActive && other.CompareTag("Enemy") && !isTriggered && canReactivate)
        {
            ActivateSpikes();
            DealDamage(other.gameObject);
            isTriggered = true;
        }
    }

    private void ActivateSpikes()
    {
        isActive = true;
        spikeAnimator.SetBool("isActivated", true);
        Invoke("DeactivateSpikes", activationDuration);
    }

    private void DeactivateSpikes()
    {
        isActive = false;
        spikeAnimator.SetBool("isActivated", false);
        StartCoroutine(ReactivateAfterDelay());
    }

    private System.Collections.IEnumerator ReactivateAfterDelay()
    {
        yield return new WaitForSeconds(reactivationDelay);
        isTriggered = false;
        canReactivate = true;
    }

    private void DealDamage(GameObject enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            enemyHealth.TakeDamage(damage, knockbackDirection);
        }
    }
}
