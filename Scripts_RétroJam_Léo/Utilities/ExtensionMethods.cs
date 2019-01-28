using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionMethods
{
    public static int GetRandomSpecificInt(int minRange, int maxRange, bool isOdd)
    {
        int evenOrOdd;

        if (isOdd) evenOrOdd = 1;
        else evenOrOdd = 0;

        int value = minRange - 1;

        while(value == minRange - 1 || value % 2 == evenOrOdd)
        {
            value = Random.Range(minRange, maxRange);
        }

        return value;
    }

    public static Vector2 FloorVect2(Vector2 vector)
    {
        vector.x = Mathf.Floor(vector.x);
        vector.y = Mathf.Floor(vector.y);

        return vector;
    }

    public static Vector2 RoundVect2(Vector2 vector)
    {
        vector.x = Mathf.Round(vector.x * 10) / 10;
        vector.y = Mathf.Round(vector.y * 10) / 10;

        return vector;
    }
}
