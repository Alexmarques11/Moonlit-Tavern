using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TavernMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text tavernHealthText;
    [SerializeField] private TMP_Text tavernLevelText;
    [SerializeField] private Button upgradeTavernHealthButton;
    [SerializeField] private Button closeTavernMenuButton;
    [SerializeField] private Slider tavernHealthBar;
    [SerializeField] private TMP_Text golemStrenghtText;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text playerXPText;
    [SerializeField] private Button upgradePlayerHealthButton;
    [SerializeField] private TMP_Text playerHealthText;

    private Building tavernBuilding;
    private Inventory playerInventory;
    private PlayerHealth playerHealth;
    private PlayerXP playerXP;
    private int tavernLevel = 1;
    private int playerLevel = 1;
    private int requiredGolemStrength = 1;
    private int xpRequiredForPlayerHealthUpgrade = 50;

    private void Start()
    {
        GameObject tavernObject = GameObject.Find("Tavern");
        if (tavernObject != null)
        {
            tavernBuilding = tavernObject.GetComponent<Building>();
        }

        playerInventory = FindObjectOfType<Inventory>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerXP = FindObjectOfType<PlayerXP>();

        if (tavernBuilding != null)
        {
            UpdateMenu();
            upgradeTavernHealthButton.onClick.AddListener(UpgradeTavernHealth);
        }
        else
        {
            Debug.LogWarning("Tavern object or Building component not found!");
        }

        if (upgradePlayerHealthButton != null)
        {
            upgradePlayerHealthButton.onClick.AddListener(UpgradePlayerHealth);
        }

        if (closeTavernMenuButton != null)
        {
            closeTavernMenuButton.onClick.AddListener(CloseTavernMenu);
        }
    }

    private void Update()
    {
        UpdateMenu();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
           gameObject.SetActive(false);
        }

    }

    private void UpdateMenu()
    {
        if (tavernBuilding != null)
        {
            tavernHealthText.text = tavernBuilding.health + " / " + tavernBuilding.maxHealth;
            tavernLevelText.text = "Level: " + tavernLevel;

            if (tavernHealthBar != null)
            {
                tavernHealthBar.value = tavernBuilding.health / tavernBuilding.maxHealth;
            }

            int currentGolemStrength = playerInventory.GetGolemStrengthAmount();
            golemStrenghtText.text = currentGolemStrength + " / " + requiredGolemStrength;
        }

        if (playerXP != null)
        {
            playerXPText.text = playerXP.currentXP + " / " + xpRequiredForPlayerHealthUpgrade + " XP";
            playerHealthText.text = playerHealth.health + " / " + playerHealth.maxHealth;
            playerLevelText.text = "Level: " + playerLevel;
        }
    }

    private void UpgradeTavernHealth()
    {
        if (playerInventory.HasGolemStrenght() && tavernBuilding != null)
        {
            playerInventory.UseGolemStrenght();
            tavernBuilding.maxHealth += 500f;
            tavernBuilding.health = tavernBuilding.maxHealth;
            tavernLevel++;

            UpdateMenu();
        }
        else if (tavernBuilding == null)
        {
            Debug.LogWarning("Building component for Tavern is not assigned or not found.");
        }
        else
        {
            Debug.Log("Golem Strength insuficiente no inventário!");
        }
    }

    private void UpgradePlayerHealth()
    {
        if (playerXP != null && playerXP.currentXP >= xpRequiredForPlayerHealthUpgrade)
        {
            if (playerHealth != null)
            {
                playerHealth.maxHealth += 20f;
                playerHealth.health += 20f;
                playerXP.currentXP -= xpRequiredForPlayerHealthUpgrade;
                playerLevel++;

                Debug.Log("Player health upgraded! New max health: " + playerHealth.maxHealth);

                xpRequiredForPlayerHealthUpgrade = xpRequiredForPlayerHealthUpgrade + 50;
                UpdateMenu();
            }
        }
        else
        {
            Debug.Log("XP insuficiente para o upgrade de saúde!");
        }
    }

    private void CloseTavernMenu()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
