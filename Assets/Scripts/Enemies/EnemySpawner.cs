using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;

    public Transform minSpawn, maxSpawn;

    private Transform target;

    private GameObject player;

    public TextMeshProUGUI waveCountText;
    int waveCount = 1;

    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;

    public int enemyAmountToSpawn;

    public int currentEnemyAmount;

    bool waveIsDone = true;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        waveCountText.text = "Wave: " + waveCount.ToString();

        if (waveIsDone == true && currentEnemyAmount <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(WaveSpawner());
        }

        transform.position = target.position;
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5f;

        if(spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f)> 0.5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y= minSpawn.position.y;
            }
        }

        return spawnPoint;
    }

    IEnumerator WaveSpawner()
    {
        waveIsDone = false;

        for (int i = 0; i < enemyAmountToSpawn; i++)
        {
            GameObject enemyClone = Instantiate(enemyToSpawn, SelectSpawnPoint(), transform.rotation);

            currentEnemyAmount++;

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;
        enemyAmountToSpawn += 3;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }

}
