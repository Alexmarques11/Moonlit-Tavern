using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUsePotion : MonoBehaviour
{
    Inventory playerInventory;

    void Awake()
    {
        playerInventory = gameObject.GetComponent<Inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(playerInventory.buffPotionsCount > 0)
            {
                playerInventory.UseBuffPotion();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playerInventory.ofensivePotionsCount > 0)
            {
                playerInventory.UseOfensivePotion();
            }
        }
    }
}
