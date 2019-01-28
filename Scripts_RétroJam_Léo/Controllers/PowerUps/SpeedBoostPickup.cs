using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : GeneralPickupClass
{
    [SerializeField]
    private float speedIncrease;

    public override void pickupEffect()
    {
        StartCoroutine(IncreaseSpeed());
    }

    IEnumerator IncreaseSpeed()
    {
        float initalPlayerSpeed = playerScript.speed;
        playerScript.speed += speedIncrease;

        yield return new WaitForSeconds(waitTime);

        playerScript.speed = initalPlayerSpeed;
    }
}
