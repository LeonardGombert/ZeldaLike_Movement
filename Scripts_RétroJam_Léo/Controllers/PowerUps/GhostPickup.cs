using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPickup : GeneralPickupClass
{
    //Start GhostTimer
    public override void pickupEffect()
    {
        StartCoroutine(GhostTimer());
    }
    //Shift Player from one layer to another and back after timer is = 0
    IEnumerator GhostTimer()
    {
        player.layer = 13;

        yield return new WaitForSeconds(waitTime);

        player.layer = 9;
        Destroy(gameObject);
    }

}
