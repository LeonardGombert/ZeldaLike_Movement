using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : GeneralPickupClass
{
    [SerializeField]
    private int healAmount;

    public override void pickupEffect()
    {
        if(Mathf.Sign(playerScript.dmgMeter - healAmount) == -1)
        {
            healAmount = playerScript.dmgMeter;
        }

        playerScript.dmgMeter -= healAmount;
    }
}
