using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class Point : MonoBehaviour, ICloneable
{
    public int2 pos;

    public Point(int2 pos)
    {
        this.pos = pos;
    }
    public Point(Vector2 pos)
    {
        this.pos.x = (int)pos.x;
        this.pos.y = (int)pos.y;
    }

    public object Clone()
    {
        return new Point(pos);
    }

    public Vector2 posV2()
    {
        return new Vector2(pos.x, pos.y);
    }

    public override string ToString()
    {
        return "Points: " + pos;
    }
}
