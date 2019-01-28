using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionBoostPickup : GeneralPickupClass
{
    public override void pickupEffect()
    {
        playerScript.propulsionBoost++;
    }
}
