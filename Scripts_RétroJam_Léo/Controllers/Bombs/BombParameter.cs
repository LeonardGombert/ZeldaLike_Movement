using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombParameter : MonoBehaviour
{
    [HideInInspector]
    public Vector2 fuseTimeRange;
    [HideInInspector]
    public Vector2Int radiusRange;

    [SerializeField]
    private GameObject basicExplosionEffect;
    [SerializeField]
    private GameObject lineExplosionLane;
    [SerializeField]
    private GameObject lineExplosionExtremity;
    [SerializeField]
    private GameObject lineExplosionCenter;

    public static GameObject[] explosionEffects;

    public static Dictionary<List<Vector2>, int> FillDicoAnglesForExplosionDir(List<Vector2>[] _explosionLines)
    {
        Dictionary<List<Vector2>, int> dictionary = new Dictionary<List<Vector2>, int>()
        {
            {_explosionLines[0], -90 },
            {_explosionLines[1], 180 },
            {_explosionLines[2], 90 },
            {_explosionLines[3], 0 },
        };

        return dictionary;
    }

    public static Vector2[] directions = new Vector2[4]
    {
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.up
    };

    void Awake()
    {
        explosionEffects = new GameObject[4]
        {
            basicExplosionEffect,
            lineExplosionLane,
            lineExplosionExtremity,
            lineExplosionCenter,
        };
    }
}
