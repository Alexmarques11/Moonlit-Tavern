using System.Collections;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    public Animator axeAnimator;
    public float delay = 0.3f;
    public bool attackBlocked;
    public float weaponDmg = 1f;
    public int weaponLevel;
    public bool attacking;

    public bool IsAttacking { get; private set; }

    private float attackingResetTime = 0f;
    private const float attackingResetDelay = 1f;

    private float attackBlockedResetTime = 0f;
    private const float attackBlockedResetDelay = 1.5f;

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (IsAttacking && Time.time > attackingResetTime)
        {
            IsAttacking = false;
        }

        if (attackBlocked && Time.time > attackBlockedResetTime)
        {
            attackBlocked = false;
        }

        if (IsAttacking)
        {
            return;
        }

        HandleAttacking();
        attacking = IsAttacking;
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

            attackingResetTime = Time.time + attackingResetDelay;
            attackBlockedResetTime = Time.time + attackBlockedResetDelay;

            StartCoroutine(DelayAttack());
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public void OnPlayerDeath()
    {
        attackBlocked = false;
        IsAttacking = false;
        axeAnimator.ResetTrigger("Attack");
    }

    public void UpgradeWeapon()
    {
        weaponDmg += 1;
        weaponLevel++;
        Debug.Log("Weapon Upgraded! Damage: " + weaponDmg);
    }

}
