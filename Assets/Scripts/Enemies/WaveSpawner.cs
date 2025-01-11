using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private float spawnInterval;
    private float spawnTimer;

    public Transform SpawnMin;
    public Transform SpawnMax;

    public GameObject bossPrefab;
    public GameObject finalBossPrefab;
    private bool bossSpawned = false;
    private bool finalBossSpawned = false;

    public TextMeshProUGUI waveCountText;

    public bool waveComplete = true;

    //public GameObject NightEffect;
    public Image PlacarDia;
    public Image PlacarNoite;

    //Tutorial

    public TutorialManager tutorialManager;
    private bool thirdDialogueStarted = false;

    void Start()
    {
        waveComplete = true;
        GenerateWave();
    }

    void Update()
    {
        if (tutorialManager != null && !tutorialManager.isSecondDialogueFinished)
        {
            return;
        }
        if (currWave == 10 && !bossSpawned)
        {
            SpawnBoss();
        }
        else if (currWave == 20 && !finalBossSpawned)
        {
            SpawnBoss();
        }
        else if (spawnTimer <= 0)
        {
            // Spawn de inimigos normais
            if (enemiesToSpawn.Count > 0)
            {
                Vector3 spawnPosition = GetPosition();
                GameObject enemy = Instantiate(enemiesToSpawn[0], spawnPosition, Quaternion.identity);

                enemiesToSpawn.RemoveAt(0);
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;
            }

        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
        }

        if (enemiesToSpawn.Count == 0 && spawnedEnemies.Count == 0)
        {
            int currDay;
            currDay = currWave + 1;
            waveComplete = true;
            waveCountText.text = "Day: " + currDay.ToString();
            //NightEffect.SetActive(false);
            PlacarNoite.enabled = false;
            PlacarDia.enabled = true;
        }

        if (waveComplete)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (Time.timeScale == 0)
                {
                    return;
                }

                currWave++;
                bossSpawned = false;
                finalBossSpawned = false;
                waveComplete = false;
                GenerateWave();
                //NightEffect.SetActive(true);
                PlacarNoite.enabled = true;
                PlacarDia.enabled = false;
            }
        }

        if (tutorialManager != null)
        {
            if (waveComplete)
            {
                if (currWave == 1 && !thirdDialogueStarted)
                {
                    tutorialManager.StartThirdDialogue();
                    thirdDialogueStarted = true;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currWave++;
                    bossSpawned = false;
                    finalBossSpawned = false;
                    waveComplete = false;
                    GenerateWave();
                    waveCountText.text = "Night: " + currWave.ToString();
                    //NightEffect.SetActive(true);
                }
            }
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = 2;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
        }
    }

    private Vector3 GetPosition()
    {
        // Escolhe uma das quatro bordas da �rea para spawnar o inimigo
        int side = Random.Range(0, 4);

        float x = 0f;
        float y = 0f;

        switch (side)
        {
            case 0: // Borda superior
                x = Random.Range(SpawnMin.position.x, SpawnMax.position.x);
                y = SpawnMax.position.y;
                break;
            case 1: // Borda inferior
                x = Random.Range(SpawnMin.position.x, SpawnMax.position.x);
                y = SpawnMin.position.y;
                break;
            case 2: // Borda esquerda
                x = SpawnMin.position.x;
                y = Random.Range(SpawnMin.position.y, SpawnMax.position.y);
                break;
            case 3: // Borda direita
                x = SpawnMax.position.x;
                y = Random.Range(SpawnMin.position.y, SpawnMax.position.y);
                break;
        }

        // Retorna a posi��o aleat�ria na borda da �rea
        return new Vector3(x, y, 0);
    }

    private void SpawnBoss()
    {
        Vector3 spawnPosition = GetPosition();

        if (currWave == 20)
        {
            GameObject finalBoss = Instantiate(finalBossPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(finalBoss);
            finalBossSpawned = true;
        }
        else if (currWave == 10)
        {
            GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(boss);
            bossSpawned = true;
        }
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
