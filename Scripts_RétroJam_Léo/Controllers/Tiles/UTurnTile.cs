using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTurnTile : aTile
{
    private int propulsionAmount = 2;

    public override void PlayerEffect(LPlayer player)
    {
        int adjusment;

        if (player.tilePlacedOnSelf) adjusment = 0;
        else adjusment = 1;

        player.Eject(propulsionAmount + adjusment + propulsionBoost, -player.direction);
    }

    public override void BombEffect(Bomb bomb)
    {
        Debug.Log(-bomb.direction.normalized);
        bomb.SetDestination(Grid.GetTilePosition(transform.position, -bomb.direction.normalized * (2 + propulsionAmount)));
    }
}