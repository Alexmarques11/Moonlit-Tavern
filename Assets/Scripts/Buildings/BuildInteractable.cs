using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildInteractable : MonoBehaviour
{
    [SerializeField] private string interactText;

    [SerializeField] private GameObject gateVisual;
    private Animator gateAnimator;
    private WeaponSwitching weaponSwitching;
    public int selectedWeapon;

    private bool isGateOpen = false;

    [SerializeField] private GameObject tavernMenuPrefab;
    [SerializeField] private GameObject blacksmithMenuPrefab;
    [SerializeField] private GameObject alchemistMenuPrefab;
    [SerializeField] private GameObject magetowerMenuPrefab;
    [SerializeField] private GameObject huntershouseMenuPrefab;
    private GameObject tavernMenuInstance;
    private GameObject blacksmithMenuInstance;
    private GameObject alchemistMenuInstance;
    private GameObject magetowerMenuInstance;
    private GameObject huntershouseMenuInstance;

    public GameObject textObject;
    public TextMeshProUGUI textComponent;


    void Start()
    {
        if (gateVisual != null)
        {
            gateAnimator = gateVisual.GetComponent<Animator>();

            if (gateAnimator != null)
            {
                gateAnimator.SetBool("Open", false);
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interact!");

        if (interactText == "Tavern")
        {
            PauseGame();

            if (tavernMenuInstance == null)
            {
                tavernMenuInstance = Instantiate(tavernMenuPrefab);
                tavernMenuInstance.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

                Debug.Log("Tavern menu instantiated!");
            }
            else
            {
                tavernMenuInstance.SetActive(true);
                Debug.Log("Tavern menu activated!");
            }
        }
        else if (interactText == "Hunter's House")
        {
            PauseGame();

            if (huntershouseMenuInstance == null)
            {
                huntershouseMenuInstance = Instantiate(huntershouseMenuPrefab);
                huntershouseMenuInstance.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                Debug.Log("Hunter's House menu instantiated!");
            }

            huntershouseMenuInstance.SetActive(true);

            HuntersHouseMenu huntersMenu = huntershouseMenuInstance.GetComponent<HuntersHouseMenu>();
            if (huntersMenu != null)
            {
                huntersMenu.OpenMenu();
            }
            else
            {
                Debug.LogError("HuntersHouseMenu component not found on Hunter's House menu instance!");
            }

        }
        else if (interactText == "Mage Tower")
        {
            PauseGame();

            if (magetowerMenuInstance == null)
            {
                magetowerMenuInstance = Instantiate(magetowerMenuPrefab);
                magetowerMenuInstance.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                Debug.Log("Mage Tower menu instantiated!");
            }

            magetowerMenuInstance.SetActive(true);

            MageTowerMenu mageMenu = magetowerMenuInstance.GetComponent<MageTowerMenu>();
            if (mageMenu != null)
            {
                mageMenu.OpenMenu();
            }
            else
            {
                Debug.LogError("HuntersHouseMenu component not found on Hunter's House menu instance!");
            }

        }
        else if (interactText == "Blacksmith")
        {
            PauseGame();

            if (blacksmithMenuInstance == null)
            {
                blacksmithMenuInstance = Instantiate(blacksmithMenuPrefab);
                blacksmithMenuInstance.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                Debug.Log("BlackSmith menu instantiated!");
            }

            blacksmithMenuInstance.SetActive(true);

            BlackSmithMenu blackSmithMenu = blacksmithMenuInstance.GetComponent<BlackSmithMenu>();
            if (blackSmithMenu != null)
            {
                blackSmithMenu.OpenMenu();
            }
            else
            {
                Debug.LogError("BlackSmithMenu component not found on BlackSmith menu instance!");
            }

        }
        else if (interactText == "Alchemist")
        {
            PauseGame();

            if (alchemistMenuInstance == null)
            {
                alchemistMenuInstance = Instantiate(alchemistMenuPrefab);
                alchemistMenuInstance.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                Debug.Log("Alchemist menu instantiated!");
            }

            alchemistMenuInstance.SetActive(true);

            AlchemistMenu alchemistMenu = alchemistMenuInstance.GetComponent<AlchemistMenu>();
            alchemistMenu.OpenMenu();
            if (alchemistMenu != null)
            {
                alchemistMenu.OpenMenu();
            }
            else
            {
                Debug.LogError("AlchemistMenu component not found on BlackSmith menu instance!");
            }
        }

        else if (interactText == "Gate")
        {
            Debug.Log("Interacted with Gate");

            if (gateAnimator != null)
            {
                isGateOpen = !isGateOpen;
                gateAnimator.SetBool("Open", isGateOpen);
            }
            else
            {
                Debug.LogWarning("Gate Animator not found on GateVisual object!");
            }
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Disable the GameObject after the delay
        if (textObject != null)
        {
            textComponent.text = "";

        }
    }
}
