using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    private PlayerHealth playerHealth;

    public TextMeshProUGUI countdownText;
    public float respawnTime = 10f;
    private bool isDead = false;
    private float currentTime;

    public Camera mainCamera;
    public float maxZoomOut = 60f;
    public float zoomSpeed = 10f;
    private float originalZoom;
    public Image healthBar;

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        countdownText.gameObject.SetActive(false);
        currentTime = respawnTime;

        if (mainCamera.orthographic)
        {
            originalZoom = mainCamera.orthographicSize;
        }
        else
        {
            originalZoom = mainCamera.fieldOfView;
        }
    }

    void Update()
    {
        if (isDead)
        {
            CountdownToRespawn();
            ZoomOut();
        }
        else
        {
            CheckPlayerDeath();
        }
    }

    private void CheckPlayerDeath()
    {
        if (playerHealth != null && playerHealth.health <= 0 && !isDead)
        {
            isDead = true;
            currentTime = respawnTime;
            countdownText.gameObject.SetActive(true);

            player.SetActive(false);
            mainCamera.transform.position = new Vector3(0, 0, -10);
            playerHealth.healthBar.fillAmount = 0;

            ResetWeaponStates();
        }
    }

    private void CountdownToRespawn()
    {
        if (!isDead) return;

        currentTime -= Time.deltaTime;

        if (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0");
        }
        else if (currentTime <= 0 && isDead)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        player.SetActive(true);
        player.transform.position = respawnPoint.position;
        playerHealth.health = playerHealth.maxHealth;

        mainCamera.orthographicSize = originalZoom;
        countdownText.gameObject.SetActive(false);

        isDead = false;
        currentTime = respawnTime;

        ResetWeaponStates();
    }

    private void ZoomOut()
    {
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, maxZoomOut, Time.deltaTime * zoomSpeed);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, maxZoomOut, Time.deltaTime * zoomSpeed);
        }
    }

    private void ResetWeaponStates()
    {
        if (player == null) return;

        AxeWeapon axeWeapon = player.GetComponentInChildren<AxeWeapon>();
        if (axeWeapon != null)
        {
            axeWeapon.ResetIsAttacking();
            axeWeapon.attackBlocked = false;
        }
    }
}
