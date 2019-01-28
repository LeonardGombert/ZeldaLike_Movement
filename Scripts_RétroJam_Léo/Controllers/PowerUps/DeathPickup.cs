using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPickup : GeneralPickupClass
{
    // Start is called before the first frame update
    public override void pickupEffect()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(waitTime);

        playerScript.Respawn();
    }
}
