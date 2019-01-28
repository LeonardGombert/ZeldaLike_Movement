using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoostPickup : GeneralPickupClass
{
    public override void pickupEffect()
    {
        playerScript.tileBoost++;
    }
}
