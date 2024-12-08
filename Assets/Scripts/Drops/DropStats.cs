using UnityEngine;


public class DropType : MonoBehaviour
{
    public string dropName;
    public int value;
    private WaveSpawner waveSpawner;
    private Inventory playerInventory;

    void Awake()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        playerInventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (waveSpawner.waveComplete)
        {
            if (dropName == "golemStrngth")
            {
                value = 1;
            }
            else
            {
                value = Random.Range(1, 3);
            }

            if (playerInventory != null)
            {
                playerInventory.GetResources(dropName, value);
                //Debug.Log(playerInventory.glue);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (dropName == "golemStrngth")
            {
                value = 1;
            }
            else
            {
                value = Random.Range(1, 3);
            }

            playerInventory = other.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.GetResources(dropName, value);
            }
            Debug.Log(playerInventory.glue);

            Destroy(gameObject);
        }
    }
}
