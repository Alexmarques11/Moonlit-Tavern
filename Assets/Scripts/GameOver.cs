using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string GameOverScene;
    Building building;


    void Awake()
    {
        building = this.GetComponent<Building>();
    }

    void Update()
    {
        if(building.health <= 0)
        {
            EndGame();
        }
    }


    private void EndGame()
    {
        SceneManager.LoadScene(GameOverScene);
    }
}
