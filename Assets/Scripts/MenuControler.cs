using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControler : MonoBehaviour
{
    // variaveis do main menu
    public string MainMenuScene;

    // variaveis do menu de pausa
    public GameObject PauseScreen;
    public GameObject Ui;
    public bool IsPaused = false;

    // variaveis do sistema de construcao
    public GameObject BuildingGridSystem;

    public WaveSpawner WaveSpawner;

    public GameObject Aim;

    public GameObject Axe;
    public GameObject Crossbow;
    public GameObject Staff;

    private AxeWeapon axeWeaponComponent;
    private CrossbowWeapon crossbowWeaponComponent;
    private StaffWeapon staffWeaponComponent;

    public bool IsBuilding = false;

    public GameObject buildCostUI;

    // variaveis do inventario
    public GameObject Player;

    public GameObject InventoryUI;

    public Text GlueDisplay;
    public Text WoodDisplay;
    public Text StoneDisplay;
    public Image DisplayBPotion;
    public Image DisplayOPotion;
    public Text BuffPotionCount;
    public Text OfensivePotionCount;

    public Text axeDamageCount;
    public Text crossbowDamageCount;
    public Text staffDamageCount;

    public GameObject HealingPotion;
    public GameObject LightningPotion;

    public Sprite HealingPotionImage;
    public Sprite LightningPotionImage;

    public GameObject CrossbowInventoryUi;
    public GameObject StaffInventoryUi;

    public bool IsInventoryOpen;

    // Start is called before the first frame update
    void Start()
    {
        if (Axe != null)
        {
            axeWeaponComponent = Axe.GetComponent<AxeWeapon>();
            crossbowWeaponComponent = Crossbow.GetComponent<CrossbowWeapon>();
            staffWeaponComponent = Staff.GetComponent<StaffWeapon>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialogue.IsDialogueActive)
        {
            return;
        }

        //Inventory UI
        Inventory playerInventory = Player.gameObject.GetComponent<Inventory>();

        GlueDisplay.text = playerInventory.glue.ToString();
        WoodDisplay.text = playerInventory.wood.ToString();
        StoneDisplay.text = playerInventory.stone.ToString();

        BuffPotionCount.text = playerInventory.buffPotionsCount.ToString();
        OfensivePotionCount.text = playerInventory.ofensivePotionsCount.ToString();

        axeDamageCount.text = axeWeaponComponent.weaponDmg.ToString();
        crossbowDamageCount.text = crossbowWeaponComponent.weaponDmg.ToString();
        staffDamageCount.text = staffWeaponComponent.weaponDmg.ToString();

        //Potion Inventory UI
        if (playerInventory.buffPotions = HealingPotion)
        {
            DisplayBPotion.sprite = HealingPotionImage;

        }
        if (playerInventory.ofensivePotions = LightningPotion)
        {
            DisplayOPotion.sprite = LightningPotionImage;

        }

        //Wepon Inventory UI
        WeaponSwitching weaponSwitching = Aim.GetComponent<WeaponSwitching>();

        if (weaponSwitching.crossbowUnlocked)
        {
            CrossbowInventoryUi.SetActive(true);
            crossbowDamageCount.gameObject.SetActive(true);
        }
        if (weaponSwitching.staffUnlocked)
        {
            StaffInventoryUi.SetActive(true);
            staffDamageCount.gameObject.SetActive(true);
        }


        if (axeWeaponComponent != null && axeWeaponComponent.IsAttacking)
        {
            return;
        }

        //Open Build System UI
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (IsBuilding == false && WaveSpawner.waveComplete)
            {
                IsBuilding = true;
                BuildingGridSystem.SetActive(true);
                Aim.SetActive(false);
                Ui.SetActive(false);
                buildCostUI.SetActive(true);
            }
            else
            {
                IsBuilding = false;
                BuildingGridSystem.SetActive(false);
                Aim.SetActive(true);
                Ui.SetActive(true);
                buildCostUI.SetActive(false);

                ResetWeaponStates();
            }
        }


        //Open Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused == false)
            {
                IsPaused = true;
                Aim.SetActive(false);
                PauseScreen.SetActive(true);
                Ui.SetActive(false);
                Time.timeScale = 0f;
            }

            if (IsInventoryOpen == true)
            {
                IsInventoryOpen = false;
                IsPaused = false;
                InventoryUI.SetActive(false);
                Ui.SetActive(true);
                if (!IsBuilding)
                {
                    Aim.SetActive(true);
                    Ui.SetActive(true);
                }
                else
                {
                    BuildingGridSystem.SetActive(true);
                }


                Time.timeScale = 1f;
            }


            if (IsBuilding == true)
            {
                IsBuilding = false;
                BuildingGridSystem.SetActive(false);
                Aim.SetActive(true);
                Ui.SetActive(true);
                buildCostUI.SetActive(false);

                ResetWeaponStates();
            }
        }

        //Open Inventory UI
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (IsInventoryOpen == false)
            {
                IsInventoryOpen = true;
                IsPaused = true;
                InventoryUI.SetActive(true);
                Ui.SetActive(false);
                Aim.SetActive(false);
                BuildingGridSystem.SetActive(false);

                Time.timeScale = 0f;
            }
            else
            {
                IsInventoryOpen = false;
                IsPaused = false;
                InventoryUI.SetActive(false);
                Ui.SetActive(true);
                if (!IsBuilding)
                {
                    Aim.SetActive(true);
                    Ui.SetActive(true);
                }
                else
                {
                    BuildingGridSystem.SetActive(true);
                }


                Time.timeScale = 1f;
            }
        }
    }

    public void Resume()
    {
        if (IsPaused == true)
        {
            IsPaused = false;
            PauseScreen.SetActive(false);
            Ui.SetActive(true);
            Aim.SetActive(true);
            Time.timeScale = 1f;

        }
    }

    public void ReturnMenu()
    {
        IsPaused = false;
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene(MainMenuScene);
    }

    private void ResetWeaponStates()
    {
        // Reseta o machado
        if (axeWeaponComponent != null)
        {
            axeWeaponComponent.ResetIsAttacking();
            axeWeaponComponent.attackBlocked = false;
        }
    }
}
