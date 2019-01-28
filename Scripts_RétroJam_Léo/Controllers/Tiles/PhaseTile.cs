using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTile : aTile
{
    public LayerMask layerMask;

    public override void PlayerEffect(LPlayer player)
    {
        Vector2 gridPosition = Grid.GetTilePosition(transform.position, Vector2.zero);
        Vector2 iPosition = direction;
        Vector2 emptyPos = Vector2.zero;
        int nbrOfTiles = 0;
        bool hit = true;

        int adjusment;

        if (player.tilePlacedOnSelf) adjusment = 0;
        else adjusment = 1;

        while (hit == true)
        {
            RaycastHit2D hitTile = Physics2D.Raycast(gridPosition + iPosition, direction, 0.45f, layerMask);
            nbrOfTiles++;
            Debug.Log("Nbr of pass (" + nbrOfTiles + ")");
            if (hitTile.collider != null)
            {
                iPosition += direction;
            }
            else
            {
                emptyPos = gridPosition + iPosition;
                hit = false;
                break;
            }
        }

        player.Eject(nbrOfTiles + adjusment, direction, true);
    }

    public override void BombEffect(Bomb bomb)
    {
        Vector2 gridPosition = Grid.GetTilePosition(transform.position, Vector2.zero);
        Vector2 iPosition = direction;
        Vector2 emptyPos = Vector2.zero;
        int nbrOfTiles = 0;
        bool hit = true;
        bomb.opacity = true;

        while (hit == true)
        {
            RaycastHit2D hitTile = Physics2D.Raycast(gridPosition + iPosition, direction, 0.45f, layerMask);
            nbrOfTiles++;
            Debug.Log("Nbr of pass (" + nbrOfTiles + ")");
            if (hitTile.collider != null)
            {
                iPosition += direction;
            }
            else
            {
                emptyPos = gridPosition + iPosition;
                hit = false;
                break;
            }
        }

        bomb.SetDestination(emptyPos);
    }
}