using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints;

    public float maxHitPoints;
    [SerializeField] private GameObject drop;
    [SerializeField] public Transform position;
    [SerializeField] private int xpReward = 50;
    private WaveSpawner waveSpawner;

    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float knockbackDuration = 0.2f;
    private Rigidbody2D rb;

    private Vector2 knockbackVelocity;
    private float knockbackTime;

    private SpriteRenderer spriteRenderer;
    private bool isFading = false;

    private CameraController controller;
    [SerializeField]private bool isBoss;

    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxHitPoints = hitPoints;

        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer não encontrado em{gameObject.name}");
        }

        if (isBoss)
        {
            controller = FindObjectOfType<CameraController>();
            controller.bossPosition = position;
            controller.bossSpawned = true;
        }
    }

    void FixedUpdate()
    {
        if (knockbackTime > 0)
        {
            rb.velocity = knockbackVelocity;
            knockbackTime -= Time.fixedDeltaTime;

            spriteRenderer.color = Color.red;

            if (!isFading)
            {
                StartCoroutine(FadeToWhite());
            }

            knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, Time.fixedDeltaTime / knockbackDuration);
        }
    }

    public void TakeDamage(float dmg, Vector2 knockbackDir)
    {
        hitPoints -= dmg;

        if (knockbackDir != Vector2.zero)
        {
            knockbackVelocity = knockbackDir.normalized * knockbackForce;
            knockbackTime = knockbackDuration;
        }

        if (hitPoints <= 0)
        {
            GiveXPToPlayer();
            waveSpawner.RemoveEnemyFromList(gameObject);

            GameObject projectileObj = Instantiate(drop, position.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private IEnumerator FadeToWhite()
    {
        isFading = true;
        float fadeDuration = 0.2f;
        float fadeTimer = 0f;

        Color startColor = Color.red;
        Color endColor = Color.white;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, fadeTimer / fadeDuration);
            yield return null;
        }

        spriteRenderer.color = endColor;
        isFading = false;
    }

    private void GiveXPToPlayer()
    {
        PlayerXP playerXP = FindObjectOfType<PlayerXP>();
        if (playerXP != null)
        {
            playerXP.AddXP(xpReward);
        }
        else
        {
            Debug.LogWarning("PlayerXP não encontrado no jogo.");
        }
    }

    public float GetCurrentHealth()
    {
        return hitPoints;
    }

    public float GetMaxHealth()
    {
        return maxHitPoints;
    }
}
