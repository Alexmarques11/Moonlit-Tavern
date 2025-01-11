using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explode : MonoBehaviour
{
    public GameObject damageHitbox;
    public VisualEffect explosionParticles;
    public GameObject position;
    public int playerDamage = 50;
    public int structureDamage = 100;

    void Start()
    {
        /*if (damageHitbox != null)
        {
            damageHitbox.SetActive(false);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Building") || other.gameObject.CompareTag("Tavern"))
        {
            Blow();
        }
    }

    void Blow()
    {
        explosionParticles.Play();
        CircleCollider2D circleHitbox = damageHitbox.GetComponent<CircleCollider2D>();
        circleHitbox.enabled = true;

        SpriteRenderer spriterenderer = gameObject.GetComponent<SpriteRenderer>();
        spriterenderer.enabled = false;

        Unit unit = gameObject.GetComponent<Unit>();
        unit.speed = 0f;

        gameObject.transform.position = position.transform.position;

        Destroy(damageHitbox, 0.1f);
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damageHitbox != null)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.health -= playerDamage;
                }
            }
            else if ((other.CompareTag("Building") || other.CompareTag("Tavern")) && other.tag != "InactiveBuilding")
            {
                Building building = other.GetComponent<Building>();
                if (building != null)
                {
                    building.TakeDamage(structureDamage);
                }
            }
        }
    }
}
