using UnityEngine;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;
    [SerializeField] private TMP_Text xpText;

    private void Update()
    {
        if (xpText != null)
        {
            xpText.text = currentXP.ToString();
        }
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP Added: " + amount + ". Current XP: " + currentXP);
    }

    public void SubtractXP(int amount)
    {
        currentXP -= amount;
        if (currentXP < 0) currentXP = 0;
        Debug.Log("XP Subtracted: " + amount + ". Current XP: " + currentXP);
    }
}
