using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsTab;
    public string GameScene;
    private bool controls;
    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void Update()
    {
        if (controls == true && Input.GetMouseButtonDown(0))
        {
            controlsTab.SetActive(false);
            controls = false;
        }
    }

    public void ControlTab()
    {
        controlsTab.SetActive(true);
        controls = true;
    }

   public void QuitGame()
    {
        Application.Quit();
    }

}
