using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class FakeHeight : MonoBehaviour
{
    public UnityEvent onGroundHitEvent;

    public Transform transformObject;
    public Transform transformBody;
    public Transform transformShadow;
    public GameObject objectPotion;
    public GameObject body;
    public GameObject shadow;
    public GameObject temporaryEfect;

    public VisualEffect lightningEffect;

    public float delay = 1f;

    public int damage = 30;

    public float gravity = -10f;
    public Vector2 groundVelocity;
    public float verticalVelocity;
    private float lastInitialVerticalVelocity;

    public bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        CheckGroundHit();

    }

    public void Initialize(Vector2 groundVelocity, float vertiacalVelocity)
    {
        isGrounded = false;
        this.groundVelocity = groundVelocity;
        this.verticalVelocity = vertiacalVelocity;
        lastInitialVerticalVelocity = vertiacalVelocity;
    }

    void UpdatePosition()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transformBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
            transformObject.position += (Vector3)groundVelocity * Time.deltaTime;
        }
        

    }

    void CheckGroundHit()
    {
        if(transformBody.position.y < transformShadow.position.y && !isGrounded)
        {

            isGrounded = true;
            GroundHit();
        }
    }

    void GroundHit()
    {
        onGroundHitEvent.Invoke();

        //StartCoroutine(nameof(delayDestroy));
        //{

        //}

        Destroy(gameObject, 1f);
    }

    //IEnumerator delayDestroy()
    //{
    //    yield return new WaitForSeconds (delay);
    //}

    public void Explosion()
    {

        body.GetComponent<SpriteRenderer>().enabled = false;
        shadow.SetActive(false);

        lightningEffect.Play();
        //temporaryEfect.SetActive(true);

        objectPotion.GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hitbox collided with an enemy: " + collision.gameObject.name);

            Vector2 knockbackDirection;
            if (FindObjectOfType<WeaponSwitching>().selectedWeapon == 0)
            {
                Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
                knockbackDirection = (collision.transform.position - playerTransform.position).normalized;
            }
            else
            {
                knockbackDirection = (collision.transform.position - transform.position).normalized;
            }

            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, knockbackDirection);
            
        }
    }
}
