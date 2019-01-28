using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionTile : aTile
{
    private int propulsionAmount = 3;
    [SerializeField]
    private LayerMask layerMask;

    public override void PlayerEffect(LPlayer player)
    {
        int adjusment;

        if (player.tilePlacedOnSelf) adjusment = 0;
        else adjusment = 1;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 2, layerMask);

        if (hit)
        {
            Debug.Log("Correct");
            propulsionAmount = 1;
            adjusment--;
        }

        player.Eject(propulsionAmount + adjusment + propulsionBoost, direction);
    }

    public override void BombEffect(Bomb bomb)
    {
        bomb.SetDestination(Grid.GetTilePosition(transform.position, direction.normalized * propulsionAmount));
    }
}
