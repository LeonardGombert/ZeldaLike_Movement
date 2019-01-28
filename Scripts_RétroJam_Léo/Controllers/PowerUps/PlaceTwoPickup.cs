using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTwoPickup : GeneralPickupClass
{
    public override void pickupEffect()
    {
        StartCoroutine(IncreaseTileCapacity());
    }

    IEnumerator IncreaseTileCapacity()
    {
        playerScript.tileCapacity = 1;

        yield return new WaitForSeconds(waitTime);

        playerScript.tileCapacity = 0;
    }
}
