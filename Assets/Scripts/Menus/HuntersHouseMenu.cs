using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HuntersHouseMenu : MonoBehaviour
{
    private PlayerXP playerXP;
    private CrossbowWeapon crossbowWeapon;
    private WeaponSwitching weaponSwitching;

    public int currentweapon;
    public int weaponLevel = 1;
    public float weaponDamage = 1;
    public int xpRequiredForWeaponUpgrade = 50;

    [SerializeField] private Button closeMenuButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text WeaponLevelText;
    [SerializeField] private TMP_Text WeaponDamageText;
    [SerializeField] private TMP_Text playerXPText;
    [SerializeField] private TMP_Text NpcDialogueText;

    //Textos do NPC
    private string[] greetings = {"Welcome back, adventurer! Ready to strengthen your arsenal?",
                                  "Ah, you return! Let's make that crossbow mightier!",
                                  "Hello again! Shall we prepare for your next hunt?"};
    private string[] successMessages = {"Your weapon has been upgraded! Use it wisely.",
                                        "The crossbow grows stronger! You're ready for tougher battles.",
                                        "Excellent choice! Your enemies won't stand a chance now."};
    private string[] failureMessages = {"You don't have enough XP for an upgrade. Keep hunting!",
                                        "Come back with more XP, and we'll talk upgrades.",
                                        "Not enough experience... perhaps try hunting more monsters."};


    void Start()
    {
        GameObject huntersHouseObject = GameObject.Find("HuntersHouse");

        playerXP = FindObjectOfType<PlayerXP>();
        weaponSwitching = FindObjectOfType<WeaponSwitching>();
        if (weaponSwitching == null)
        {
            Debug.LogError("WeaponSwitching not found!");
        }

        weaponSwitching.crossbowUnlocked = true;
        weaponSwitching.selectedWeapon = 1;

        if (closeMenuButton != null)
        {
            closeMenuButton.onClick.AddListener(CloseMenu);
        }

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(AttemptWeaponUpgrade);
        }
    }

    public void OpenMenu()
    {
        if (weaponSwitching == null)
        {
            return;
        }
        Time.timeScale = 0f;
        currentweapon = weaponSwitching.selectedWeapon;
        weaponSwitching.selectedWeapon = 1;
        weaponSwitching.SelectedWeapon();
        gameObject.SetActive(true);

        NpcDialogueText.text = greetings[Random.Range(0, greetings.Length)];
    }

    private void CloseMenu()
    {
        Time.timeScale = 1f;
        weaponSwitching.selectedWeapon = currentweapon;
        weaponSwitching.SelectedWeapon();
        gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateSelectedWeapon();
        UpdateMenu();
    }

    void UpdateSelectedWeapon()
    {
        if (weaponSwitching != null && weaponSwitching.selectedWeapon == 1 && weaponSwitching.IsCrossbowUnlocked())
        {
            crossbowWeapon = weaponSwitching.transform.GetChild(weaponSwitching.selectedWeapon).GetComponent<CrossbowWeapon>();
        }
        else
        {
            crossbowWeapon = null;
        }
    }

    void UpdateMenu()
    {
        if (playerXP != null)
        {
            string currentXPColor = playerXP.currentXP >= xpRequiredForWeaponUpgrade ? "#00FF00" : "#FF0000";
            playerXPText.text = $"<color={currentXPColor}>{playerXP.currentXP}</color> / {xpRequiredForWeaponUpgrade} XP";
        }
    }


    private void AttemptWeaponUpgrade()
    {
        if (playerXP != null && crossbowWeapon != null)
        {
            if (playerXP.currentXP >= xpRequiredForWeaponUpgrade)
            {
                crossbowWeapon.UpgradeWeapon();
                playerXP.currentXP -= xpRequiredForWeaponUpgrade;
                xpRequiredForWeaponUpgrade += 50;
                NpcDialogueText.text = "Weapon upgraded successfully!";
                weaponLevel = crossbowWeapon.weaponLevel;
                weaponDamage = crossbowWeapon.weaponDmg;
                WeaponLevelText.text = "Level: " + weaponLevel;
                WeaponDamageText.text = "Damage: " + weaponDamage;
                playerXPText.text = playerXP.currentXP + " / " + xpRequiredForWeaponUpgrade;

                NpcDialogueText.text = successMessages[Random.Range(0, successMessages.Length)];
            }
            else
            {
                NpcDialogueText.text = failureMessages[Random.Range(0, failureMessages.Length)];
            }
        }
    }
}
