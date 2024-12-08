using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlchemistMenu : MonoBehaviour
{
    private PlayerXP playerXP;
    private Inventory playerInventory;

    public int healthPotionPrice = 100;
    public int thunderPotionPrice = 200;

    [SerializeField] private Button closeMenuButton;
    [SerializeField] private Button buyHealthPotionButton;
    [SerializeField] private Button buyThunderPotionButton;
    [SerializeField] private TMP_Text healthPotionPriceText;
    [SerializeField] private TMP_Text thunderPotionPriceText;
    [SerializeField] private TMP_Text playerXPText;
    [SerializeField] private TMP_Text healthPotionInventoryText;
    [SerializeField] private TMP_Text thunderPotionInventoryText;
    [SerializeField] private TMP_Text npcDialogueText;

    private string[] greetings = {
        "Greetings, traveler! What elixirs do you seek today?",
        "Ah, a brave adventurer! Care to purchase some potions for your journey?",
        "Welcome! My potions can make the difference between life and death!"
    };

    private string purchaseSuccessMessage = "A wise choice! This potion will serve you well.";
    private string purchaseFailMessage = "You lack the gold or resources to make this purchase.";

    void Start()
    {
        playerXP = FindObjectOfType<PlayerXP>();
        playerInventory = FindObjectOfType<Inventory>();
        UpdateMenu();

        if (closeMenuButton != null)
            closeMenuButton.onClick.AddListener(CloseMenu);

        if (buyHealthPotionButton != null)
            buyHealthPotionButton.onClick.AddListener(BuyHealthPotion);

        if (buyThunderPotionButton != null)
            buyThunderPotionButton.onClick.AddListener(BuyThunderPotion);

        healthPotionPriceText.text = $"Price: {healthPotionPrice}";
        thunderPotionPriceText.text = $"Price: {thunderPotionPrice}";
    }

    public void OpenMenu()
    {
        if (playerXP == null || playerInventory == null)
            return;

        Time.timeScale = 0f;
        gameObject.SetActive(true);

        npcDialogueText.text = greetings[Random.Range(0, greetings.Length)];
        UpdateMenu();
    }

    private void CloseMenu()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void UpdateMenu()
    {
        if (playerXP != null)
            playerXPText.text = $"{playerXP.currentXP}";

        if (playerInventory != null)
        {
            healthPotionInventoryText.text = $"Potions: {playerInventory.buffPotionsCount}";
            thunderPotionInventoryText.text = $"Potions: {playerInventory.ofensivePotionsCount}";
        }
    }

    private void BuyHealthPotion()
    {
        if (playerXP != null && playerXP.currentXP >= healthPotionPrice)
        {
            playerXP.currentXP -= healthPotionPrice;
            playerInventory.buffPotionsCount++;
            npcDialogueText.text = purchaseSuccessMessage;
            UpdateMenu();
        }
        else
        {
            npcDialogueText.text = purchaseFailMessage;
        }
    }

    private void BuyThunderPotion()
    {
        if (playerXP != null && playerXP.currentXP >= thunderPotionPrice)
        {
            playerXP.currentXP -= thunderPotionPrice;
            playerInventory.ofensivePotionsCount++;
            npcDialogueText.text = purchaseSuccessMessage;
            UpdateMenu();
        }
        else
        {
            npcDialogueText.text = purchaseFailMessage;
        }
    }
}
