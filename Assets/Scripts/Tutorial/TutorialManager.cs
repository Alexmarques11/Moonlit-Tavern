using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject secondDialogue;
    public GameObject thirdDialogue;
    private bool dialogueTriggered = false;
    private bool thirdDialogueStarted = false;

    public GameObject buildSystem;
    public GameObject buildUI;
    public GameObject aim;

    public GameObject enterContructionMode;
    public GameObject exitConstructionMode;
    public GameObject enterNightMode;

    void Start()
    {
        if (secondDialogue != null)
        {
            secondDialogue.SetActive(false);
        }

        if (exitConstructionMode != null)
        {
            exitConstructionMode.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            enterContructionMode.SetActive(false);
            if (exitConstructionMode != null && exitConstructionMode.activeSelf)
            {
                exitConstructionMode.SetActive(false);
                enterNightMode.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            enterNightMode.SetActive(false);
        }

        if (!dialogueTriggered)
        {
            GameObject tavernaObject = GameObject.Find("Tavern(Clone)");
            if (tavernaObject != null)
            {
                enterContructionMode.SetActive(false);
                TriggerSecondDialogue();
                buildSystem.SetActive(false);
                buildUI.SetActive(false);
            }
        }

        if (thirdDialogueStarted && thirdDialogue != null && !thirdDialogue.activeSelf)
        {
            EndThirdDialogue();
        }
    }

    private void TriggerSecondDialogue()
    {
        dialogueTriggered = true;

        if (secondDialogue != null)
        {
            secondDialogue.SetActive(true);
        }
    }

    public void StartThirdDialogue()
    {
        if (thirdDialogue != null)
        {
            thirdDialogue.SetActive(true);
            thirdDialogueStarted = true;
        }

    }

    public void EndThirdDialogue()
    {
        if (thirdDialogue != null && !thirdDialogue.activeSelf)
        {
            SceneManager.LoadScene("SampleScene");
        }

    }

    public void ActivateTextBox()
    {
        if (exitConstructionMode != null)
        {
            exitConstructionMode.SetActive(true);
        }
    }
}
