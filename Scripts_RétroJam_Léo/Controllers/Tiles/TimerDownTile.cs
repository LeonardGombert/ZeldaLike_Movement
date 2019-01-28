using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TimerDownTile : aTile
{
    [BoxGroup("Values")]
    public float fuseDownAmount = 0.5f;

    public override void BombEffect(Bomb bomb)
    {
        float defuseAmount = (fuseDownAmount + (tileBoost / bomb.fuseTime));

        if ((bomb.fuseTimeLeft -= defuseAmount) <= 0) bomb.fuseTimeLeft = 0.01f;
        else bomb.fuseTimeLeft -= (fuseDownAmount + (tileBoost / bomb.fuseTime));
    }
}