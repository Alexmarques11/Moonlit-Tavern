using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichKingAI : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnCooldown = 10f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float spawnOffset = 1.5f;

    private float lastWaveTime;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        lastWaveTime = Time.time - spawnCooldown;
    }

    void Update()
    {
        if (Time.time >= lastWaveTime + spawnCooldown && !isSpawning)
        {
            StartCoroutine(SpawnWave());
            lastWaveTime = Time.time;
        }

        CleanupDeadEnemies();
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        for (int i = 0; i < enemiesPerWave; i++)
        {

            Vector3 spawnPosition = transform.position + transform.up * spawnOffset;
            spawnPosition += (Vector3)Random.insideUnitCircle * 0.5f;

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);

            Debug.Log("Inimigo spawnado pelo Lich King!");

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void CleanupDeadEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }
}
