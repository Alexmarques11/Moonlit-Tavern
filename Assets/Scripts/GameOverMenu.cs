using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public string GameScene;
    public string MainMenuScene;

    public void RestartStartGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
