using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlackSmithMenu : MonoBehaviour
{
    private PlayerXP playerXP;
    private AxeWeapon axeWeapon;
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
    private string[] greetings = {"Ah, a familiar face! Shall we forge greatness today?",
                                  "Welcome to my forge! Let’s turn your axe into a legend.",
                                  "Back for another upgrade? Let's make it unforgettable!"};
    private string[] successMessages = {"The upgrade is complete! May your strikes be swift and deadly.",
                                        "Your weapon gleams with newfound power. Use it wisely!",
                                        "The forge has worked its magic. Go forth and conquer!"};
    private string[] failureMessages = {"It seems you lack the XP for this upgrade. Patience is key, adventurer.",
                                        "Not enough experience? Perhaps the monsters outside can help with that.",
                                        "I can’t forge power from nothing. Return when you’re more seasoned."};


    void Start()
    {
        GameObject blackSmithObject = GameObject.Find("BlackSmith");

        playerXP = FindObjectOfType<PlayerXP>();
        weaponSwitching = FindObjectOfType<WeaponSwitching>();
        if (weaponSwitching == null)
        {
            Debug.LogError("WeaponSwitching not found!");
        }

        weaponSwitching.selectedWeapon = 0;

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
        weaponSwitching.selectedWeapon = 0;
        weaponSwitching.SelectedWeapon();
        gameObject.SetActive(true);
        UpdateMenu();

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
        if (weaponSwitching != null && weaponSwitching.selectedWeapon == 0)
        {
            axeWeapon = weaponSwitching.transform.GetChild(weaponSwitching.selectedWeapon).GetComponent<AxeWeapon>();
        }
        else
        {
            axeWeapon = null;
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
        if (playerXP != null && axeWeapon != null)
        {
            if (playerXP.currentXP >= xpRequiredForWeaponUpgrade)
            {
                axeWeapon.UpgradeWeapon();
                playerXP.currentXP -= xpRequiredForWeaponUpgrade;
                xpRequiredForWeaponUpgrade += 50;
                NpcDialogueText.text = "Weapon upgraded successfully!";
                weaponLevel = axeWeapon.weaponLevel;
                weaponDamage = axeWeapon.weaponDmg;
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
