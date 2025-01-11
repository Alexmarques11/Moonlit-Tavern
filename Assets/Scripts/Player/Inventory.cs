using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int glue;
    public int wood;
    public int stone;
    public int golemStrenght;

    public GameObject buffPotions;
    public int buffPotionsCount;

    public GameObject ofensivePotions;
    public int ofensivePotionsCount;

    //Constantes das poções
    public Vector2 groundDispenseVelocity;
    public Vector2 verticalDispenseVelocity;

    [SerializeField]
    private GameObject healingPotion;

    [SerializeField]
    private GameObject lightningPotion;

    [SerializeField]
    private ParticleSystem healingParticleEffect;

    PlayerHealth playerHealth;


    public GameObject arrow;
    public Transform FirePoint;


    void Awake()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
    }

    public void GetResources(string dropName, int value)
    {
        switch (dropName)
        {
            case "glue":
                {
                    glue = glue + value;
                    break;
                }
            case "wood":
                {
                    wood = wood + value;
                    break;
                }
            case "stone":
                {
                    stone = stone + value;
                    break;
                }
            case "golemStrenght":
                {
                    golemStrenght = golemStrenght + value;
                    break;
                }
        }
    }

    public void SpendResources(int spentGlue, int spentWood, int spentStone)
    {
        glue = glue - spentGlue;

        wood = wood - spentWood;

        stone = stone - spentStone;
    }

    public void UseBuffPotion()
    {
        if (buffPotions == healingPotion && buffPotionsCount > 0)
        {
            if (healingParticleEffect != null)
            {
                healingParticleEffect.Play();
            }

            buffPotionsCount = buffPotionsCount - 1;
            playerHealth.RestoreHealth(playerHealth.maxHealth / 2);
        }
    }

    public void UseOfensivePotion()
    {
        if (ofensivePotions == lightningPotion)
        {
            ofensivePotionsCount = ofensivePotionsCount - 1;
            GameObject instantiatedLightningPotion = Instantiate(lightningPotion, FirePoint.position, Quaternion.identity);
            instantiatedLightningPotion.GetComponent<FakeHeight>().Initialize(FirePoint.right * Random.Range(groundDispenseVelocity.x, groundDispenseVelocity.y), Random.Range(verticalDispenseVelocity.x, verticalDispenseVelocity.y));

        }
    }

    public bool HasGolemStrenght()
    {
        return golemStrenght > 0;
    }

    public void UseGolemStrenght()
    {
        if (golemStrenght > 0)
        {
            golemStrenght--;
        }
        else
        {
            Debug.LogWarning("Golem Strength is already at zero!");
        }
    }
    public int GetGolemStrengthAmount()
    {
        return golemStrenght;
    }

}
