using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Video;

public class CrossbowScript : MonoBehaviour
{
    private float targetingRange = 20f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float fireRate = 1f;

    private Transform target;
    private float timeUntilFire;

    // Start is called before the first frame update
   /* private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(firingPoint.position, transform.forward, targetingRange);
    }
   */
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "InactiveBuilding")
        {
            return;
        }

        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / fireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }

    }

    private void Shoot()
    {
        GameObject projectileObj = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        Debug.Log(target);
        Bullet projectileScript = projectileObj.GetComponent<Bullet>();
        projectileScript.SetTarget(target);

    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }

    }
}
