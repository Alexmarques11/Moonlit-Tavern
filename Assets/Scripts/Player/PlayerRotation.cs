using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Transform aimTransform;
    private bool isFlipped = false;

    public SpriteRenderer characterRenderer;

    public AxeWeapon axeweapon;

    private MenuControler menuControler;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");

        menuControler = FindObjectOfType<MenuControler>();
    }

    void Start()
    {

    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (axeweapon.IsAttacking || axeweapon == null)
        {
            return;
        }
        HandleAiming();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (menuControler != null && !menuControler.IsBuilding)
        {
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (mousePosition.x < transform.position.x && !isFlipped)
        {
            aimTransform.localScale = new Vector3(aimTransform.localScale.x, -aimTransform.localScale.y, aimTransform.localScale.z);
            isFlipped = true;
            characterRenderer.flipX = true;
        }
        else if (mousePosition.x >= transform.position.x && isFlipped)
        {
            aimTransform.localScale = new Vector3(aimTransform.localScale.x, -aimTransform.localScale.y, aimTransform.localScale.z);
            isFlipped = false;
            characterRenderer.flipX = false;
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
