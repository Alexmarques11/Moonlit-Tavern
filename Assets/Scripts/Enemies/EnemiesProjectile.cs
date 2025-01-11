using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesProjectile : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.health -= damage;
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Building") || other.CompareTag("Tavern") && other.tag != "InactiveBuilding")
        {
            Building building = other.GetComponent<Building>();
            if (building != null)
            {
                building.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
