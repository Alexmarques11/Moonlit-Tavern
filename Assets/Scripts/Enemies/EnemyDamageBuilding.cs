using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBuilding : MonoBehaviour
{
    public float damage;
    public float attackInterval = 1f;
    private float lastAttackTime = 0f;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (Time.time >= lastAttackTime + attackInterval)
        {
            if (other.gameObject.CompareTag("Building") && other.gameObject.tag != "InactiveBuilding")
            {
                Building building = other.gameObject.GetComponent<Building>();
                if (building != null)
                {
                    building.TakeDamage(damage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}
