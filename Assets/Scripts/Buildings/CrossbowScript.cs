using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Video;

public class CrossbowScript : MonoBehaviour
{
    public float targetingRange = 20f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float fireRate = 1f;

    public float projectileAngleRotation = 0f;

    public float projectileSpeed = 15f;
    public float lifetime = 5f;

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
        if (target == null) return;

        GameObject projectileObj = Instantiate(projectile, firingPoint.position, Quaternion.identity);

        Vector3 direction = target.position - firingPoint.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(0, 0, angle + projectileAngleRotation);

        Rigidbody2D rb = projectileObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }



        Destroy(projectileObj, lifetime);

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
