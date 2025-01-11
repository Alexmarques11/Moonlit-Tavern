using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MageTowerMenu : MonoBehaviour
{
    private PlayerXP playerXP;
    private StaffWeapon staffWeapon;
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
    private string[] greetings = {"Greetings, seeker of knowledge. Ready to harness arcane power?",
                                  "Ah, another visit from the chosen one! Shall we enhance your staff?",
                                  "Welcome back, adventurer. The winds of magic favor you today!"};
    private string[] successMessages = {"Your staff is now imbued with greater power. Use it wisely.",
                                        "The arcane energy flows stronger through your weapon. Well done!",
                                        "A wise choice! Your enemies will tremble before your might."};
    private string[] failureMessages = {"The arcane arts demand a higher price. Gather more experience.",
                                        "A true mage is patient. Come back with more experience.",
                                        "The upgrade must wait. Seek out more battles and grow your strength."};


    void Start()
    {
        GameObject mageTowerObject = GameObject.Find("MageTower");

        playerXP = FindObjectOfType<PlayerXP>();
        weaponSwitching = FindObjectOfType<WeaponSwitching>();
        if (weaponSwitching == null)
        {
            Debug.LogError("WeaponSwitching not found!");
        }

        weaponSwitching.staffUnlocked = true;
        weaponSwitching.selectedWeapon = 2;

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
        weaponSwitching.selectedWeapon = 2;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

    }

    void UpdateSelectedWeapon()
    {
        if (weaponSwitching != null && weaponSwitching.selectedWeapon == 2 && weaponSwitching.IsStaffUnlocked())
        {
            staffWeapon = weaponSwitching.transform.GetChild(weaponSwitching.selectedWeapon).GetComponent<StaffWeapon>();
        }
        else
        {
            staffWeapon = null;
        }
    }

    void UpdateMenu()
    {
        if (playerXP != null)
        {
            string currentXPColor = playerXP.currentXP >= xpRequiredForWeaponUpgrade ? "#7BDF4D" : "#E11616";
            playerXPText.text = $"<color={currentXPColor}>{playerXP.currentXP}</color> / {xpRequiredForWeaponUpgrade} XP";
        }
    }


    private void AttemptWeaponUpgrade()
    {
        if (playerXP != null && staffWeapon != null)
        {
            if (playerXP.currentXP >= xpRequiredForWeaponUpgrade)
            {
                staffWeapon.UpgradeWeapon();
                playerXP.currentXP -= xpRequiredForWeaponUpgrade;
                xpRequiredForWeaponUpgrade += 50;
                NpcDialogueText.text = "Weapon upgraded successfully!";
                weaponLevel = staffWeapon.weaponLevel;
                weaponDamage = staffWeapon.weaponDmg;
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
