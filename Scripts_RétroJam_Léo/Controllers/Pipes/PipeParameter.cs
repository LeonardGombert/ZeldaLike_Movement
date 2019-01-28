using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeParameter : MonoBehaviour
{
    [HideInInspector]
    public float resetTime;
    [HideInInspector]
    public float speed;

    void Start()
    {
        SetPipesDifficulty();
    }

    public void SetPipesDifficulty()
    {
        Pipe[] pipeScripts = GetComponentsInChildren<Pipe>();

        foreach (Pipe pipe in pipeScripts)
        {
            pipe.SetPrimaryValues(speed, resetTime);
        }
    }

    public enum ShootDirection { Up, Right, Left, Down}

    public static System.Tuple<int,int, Vector2> SetShootValues(ShootDirection shootDirection)
    {
        int instantDestroyDistance;
        int fireLength;
        Vector2 direction = Vector2.zero;

        switch (shootDirection)
        {
            case ShootDirection.Up: direction = Vector2.up;
                break;
            case ShootDirection.Right:
                direction = Vector2.right;
                break;
            case ShootDirection.Left:
                direction = Vector2.left;
                break;
            case ShootDirection.Down:
                direction = Vector2.down;
                break;
        }

        if (shootDirection == ShootDirection.Right || shootDirection == ShootDirection.Left)
        {
            instantDestroyDistance = 1;
            fireLength = 16;
        }
        else
        {
            instantDestroyDistance = 0;
            fireLength = 9;
        }

        return System.Tuple.Create( instantDestroyDistance, fireLength, direction);
    }

    public enum MoveAxis { X, Y}

    public static System.Tuple<Vector2, Vector2> SetMoveValues(MoveAxis moveAxis)
    {
        Vector2 positiveDir;
        Vector2 negativeDir;

        if (moveAxis == MoveAxis.X)
        {
            positiveDir = Vector2.right;
            negativeDir= Vector2.left;

        }
        else
        {
            positiveDir = Vector2.up;
            negativeDir = Vector2.down;
        }

        return System.Tuple.Create(positiveDir, negativeDir);
    }
}
