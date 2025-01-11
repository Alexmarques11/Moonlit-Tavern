using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CustomCursorHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject customCursor;
    public TMP_Text buttonText;
    public Color hoverColor = Color.red;
    public Color normalColor = Color.white;

    void Start()
    {
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (customCursor != null)
        {
            customCursor.SetActive(true);
        }

        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (customCursor != null)
        {
            customCursor.SetActive(false);
        }

        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }
}
