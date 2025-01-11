using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public static bool IsDialogueActive = false;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool isTyping;

    public GameObject player;
    public GameObject aim;
    public TutorialManager tutorialManager;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }
            else if (textComponent.text == lines[index])
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        IsDialogueActive = true;

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.rb.velocity = Vector2.zero;
            playerMovement.ResetMovement();
            playerMovement.enabled = false;
        }

        if (aim != null)
        {
            aim.SetActive(false);
        }

        index = 0;
        StartCoroutine(TypeLine());
    }


    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            if (aim != null)
            {
                aim.SetActive(true);
            }

            IsDialogueActive = false;
            gameObject.SetActive(false);

            if (tutorialManager != null)
            {
                tutorialManager.ActivateTextBox();
            }
        }
    }

}
