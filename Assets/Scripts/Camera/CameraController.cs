using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Transform bossPosition;
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    public float smoothTime = 1;

    public bool bossSpawned = false;
    private bool caroutineStarted = false;
    private bool caroutineFinished = false;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (bossSpawned == false && caroutineFinished == false)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, -46f, 46f), Mathf.Clamp(target.position.y, -55f, 50f), transform.position.z);
        }
        if (bossSpawned == true && caroutineFinished == false)
        {
            BossSpawn();
        }
        if (bossSpawned == false && caroutineFinished == true)
        {
            ReturnPlayer();
        }

    }


    public void BossSpawn()
    {
        Vector3 targetPosition = bossPosition.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        if (caroutineStarted == false)
        {
            caroutineStarted = true;
            StartCoroutine(BossIntroDelay());
        }
    }
    public void ReturnPlayer()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        if (caroutineStarted == false)
        {
            caroutineStarted = true;
            StartCoroutine(ReturnPlayerDelay());
        }
    }

    IEnumerator BossIntroDelay()
    {
        yield return new WaitForSeconds(4);
        bossSpawned = false;
        caroutineStarted = false;
        caroutineFinished = true;
    }

    IEnumerator ReturnPlayerDelay()
    {
        yield return new WaitForSeconds(4);
        caroutineStarted = false;
        caroutineFinished = false;
    }


}
