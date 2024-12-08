using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    private WaveSpawner waveSpawner;

    void Start()
    {
        maxHealth = health;
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (waveSpawner.waveComplete)
        {
            health = maxHealth;
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
