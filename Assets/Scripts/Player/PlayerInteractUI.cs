using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject axeIcon;
    [SerializeField] private GameObject crossbowIcon;
    [SerializeField] private GameObject staffIcon;


    private void Update()
    {
        WeaponSwitching weaponSelected = aim.gameObject.GetComponent<WeaponSwitching>();

        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }

        switch(weaponSelected.selectedWeapon)
        {
            case 0 :
            {
                axeIcon.SetActive(true);
                crossbowIcon.SetActive(false);
                staffIcon.SetActive(false);
                break;
            }
            case 1 :
            {
                axeIcon.SetActive(false);
                crossbowIcon.SetActive(true);
                staffIcon.SetActive(false);
                break;
            }
            case 2 :
            {
                axeIcon.SetActive(false);
                crossbowIcon.SetActive(false);
                staffIcon.SetActive(true);
                break;
            }
        }
        
    }

    private void Show(BuildInteractable buildInteractable)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = ("Interact with " + buildInteractable.GetInteractText());
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
