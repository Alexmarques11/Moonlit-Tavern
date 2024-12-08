using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private bool isFlipped = false;

    public SpriteRenderer weaponRenderer, characterRenderer;

    public Animator axeAnimator;
    public float delay = 0.3f;
    public bool attackBlocked;

    public bool IsAttacking { get; private set; }

    public GameObject hitbox;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    private void Update()
    {
        if (IsAttacking)
        {
            return;
        }
        HandleAiming();
        HandleAttacking();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

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

    private void HandleAttacking()
    {
        if (attackBlocked)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            axeAnimator.SetTrigger("Attack");
            IsAttacking = true;
            attackBlocked = true;
            StartCoroutine(DelayAttack());
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
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