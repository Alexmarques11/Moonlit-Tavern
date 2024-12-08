using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Image healthBarImage;

    void Start()
    {
        if (enemyHealth == null)
        {
            enemyHealth = FindObjectOfType<EnemyHealth>();
        }

        if (healthBarImage == null)
        {
            Debug.LogError("Health bar image não atribuída no BossHealthBar.");
        }
    }

    void Update()
    {
        if (enemyHealth != null && healthBarImage != null)
        {
            float healthPercentage = enemyHealth.GetCurrentHealth() / enemyHealth.GetMaxHealth();
            healthBarImage.fillAmount = healthPercentage;
        }
    }
}
