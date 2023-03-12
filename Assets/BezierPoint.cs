using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BezierPoint : MonoBehaviour
{
    public Point center;
    public Point prevControlPoint; //global pos
    public Point nextControlPoint; //global pos

    //initialized in realivyty
    public BezierPoint(int2 pos, int2 prevControlPoint, int2 nextControlPoint)
    {
        this.center = new Point(pos);
        this.prevControlPoint = new Point(prevControlPoint + pos);
        this.nextControlPoint = new Point(nextControlPoint + pos);
    }

    public BezierPoint(Vector2 pos, Vector2 prevControlPoint, Vector2 nextControlPoint)
    {
        this.center = new Point(pos);
        this.prevControlPoint = new Point(prevControlPoint + pos);
        this.nextControlPoint = new Point(nextControlPoint + pos);
    }

    public override string ToString()
    {
        return prevControlPoint+ " " + center + " "+ nextControlPoint;
    }


}
