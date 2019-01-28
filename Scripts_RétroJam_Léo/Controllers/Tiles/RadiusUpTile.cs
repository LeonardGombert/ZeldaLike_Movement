using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RadiusUpTile : aTile
{
    [BoxGroup("Values")]
    public int radiusUpAmount;

    public override void BombEffect(Bomb bomb)
    {
        bomb.radius += (radiusUpAmount + tileBoost);
    }
}