using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Building : MonoBehaviour
{
    [SerializeField] private string buildingName;
    private string tavernName = "tavern";

    [SerializeField] public float health, maxHealth = 100f;

    [SerializeField] private BuildingsHealthBar healthBar;
    [SerializeField] private SpriteRenderer prefabVisualRenderer;

    private WaveSpawner waveSpawner;

    private Rigidbody2D rb2D;
    private Collider2D col2D;

    private string originalTag;
    private int originalLayer;

    public List<Light2D> buildingLights;
    private LightsManager lightManager;

    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        healthBar.gameObject.SetActive(false);

        waveSpawner = FindObjectOfType<WaveSpawner>();

        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();

        originalTag = gameObject.tag;
        originalLayer = gameObject.layer;

        if (prefabVisualRenderer == null)
        {
            prefabVisualRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (buildingLights != null && buildingLights.Count > 0)
        {
            foreach (Light2D light in buildingLights)
            {
                light.enabled = false;
            }

            lightManager = FindObjectOfType<LightsManager>();
            if (lightManager != null)
            {
                foreach (Light2D light in buildingLights)
                {
                    lightManager.RegisterBuildingLight(light);
                }
            }
        }
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<BuildingsHealthBar>();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health < maxHealth)
        {
            ShowHealthBar();
        }

        if (health <= 0 && buildingName != tavernName)
        {
            if (col2D != null) col2D.enabled = false;

            HideHealthBar();

            gameObject.tag = "InactiveBuilding";

            gameObject.layer = LayerMask.NameToLayer("InactiveBuilding");

            SetOpacity(0.5f);
        }

    }

    private void ShowHealthBar()
    {
        healthBar.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (waveSpawner.waveComplete)
        {
            if (health <= 0)
            {
                health = maxHealth;

                if (col2D != null) col2D.enabled = true;

                gameObject.tag = originalTag;

                gameObject.layer = originalLayer;

                SetOpacity(1f);
            }
            else
            {
                health = maxHealth;
            }
        }


        if (health == maxHealth)
        {
            HideHealthBar();
        }
    }

    private void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
    private void SetOpacity(float alpha)
    {
        if (prefabVisualRenderer != null)
        {
            Color color = prefabVisualRenderer.color;
            color.a = alpha;
            prefabVisualRenderer.color = color;
        }
        else
        {
            Debug.LogWarning("PrefabVisual SpriteRenderer not assigned or found!");
        }
    }

    void OnDestroy()
    {
        if (lightManager != null && buildingLights != null)
        {
            foreach (Light2D light in buildingLights)
            {
                lightManager.DeregisterBuildingLight(light);
            }
        }
    }

}
