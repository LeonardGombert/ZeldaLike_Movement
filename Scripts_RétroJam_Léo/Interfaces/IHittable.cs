using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    void onHit(Bomb bomb, Vector2 hitDirection);
}
