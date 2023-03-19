using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

/// <summary>
/// custom bezier curve,
/// holds max to 4 bezier points
/// </summary>
public class BezierCurve : MonoBehaviour
{
    
    protected List<BezierPoint> bezierPoints = new List<BezierPoint>();  
    const int DEGREE = 2;
    const int thickness = 5;


    Renderer renderer;
    [SerializeField] RenderTexture text;

    int height = 500, width = 500;


    //shader
    public ComputeShader shader;
    public ComputeShader shaderReset;

    const int MAX_BEZIER_POINTS = 8;

    int2[] pointsArray = new int2[MAX_BEZIER_POINTS*3-2];
    int[] pointsNumber = new int[1];

    ComputeBuffer pointsBuffer;
    ComputeBuffer pointsNumberBuffer;



    //changing circles variables
    RawImage image;
    bool holdingCircle = false;
    Point heldPoint;
    
    public RenderScript render;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        text = new RenderTexture(width, height, 24);
        text.enableRandomWrite = true;
        text.Create();

        //renderer.material.mainTexture = text;

        shader.SetTexture(0, "Result", text);
        shaderReset.SetTexture(0, "Result", text);
        

        pointsBuffer = new ComputeBuffer(MAX_BEZIER_POINTS*3-2, 8);
        pointsNumberBuffer = new ComputeBuffer(1, 4);
        
        bezierPoints.Add(new BezierPoint(new int2(0,000), new int2(0, 0), new int2(100, 100)));
        bezierPoints.Add(new BezierPoint(new int2(500,100), new int2(-100, 100), new int2(100, 100)));




        image = gameObject.GetComponent<RawImage>();
        image.texture = text;

        render.RefreshCoefficients();
    }


    // Update is called once per frame
    void Update()
    {
        CheckMouse();

        ConvertBezierToPoints();
        DrawCurve();        
    }

    Vector2 MousePosToTextCoord()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, Input.mousePosition, null, out Vector2 mousePos);

        //convert to texture pos
        mousePos.x += image.rectTransform.rect.size.x / 2;
        mousePos.y += image.rectTransform.rect.size.y / 2;


        mousePos.x *= text.width / image.rectTransform.rect.size.x;
        mousePos.y *= text.height / image.rectTransform.rect.size.y;
        return mousePos;
    }

    bool ClickedPoint(Point point)
    {
        Vector2 mousePos = MousePosToTextCoord();

        float dist = Vector3.Distance(mousePos, point.posV2());
        if (dist < 20)
        {            
            return true;            
        }

        
        return false;
    }

    void CreateNewBezierPoint()
    {
        if (bezierPoints.Count < MAX_BEZIER_POINTS)
        {
            Vector2 mousePos = MousePosToTextCoord();
            int indexToAdd = 0;
            bool added = false;
            foreach (BezierPoint point in bezierPoints)
            {
                if (mousePos.x < point.center.posV2().x)
                {
                    bezierPoints.Insert(indexToAdd, new BezierPoint(mousePos, new Vector2(-50, 0), new Vector2(50, 0)));
                    added = true;
                    break;
                }
                indexToAdd++;
            }
            if (!added)
            {
                bezierPoints.Add(new BezierPoint(mousePos, new Vector2(-50, 0), new Vector2(50, 0)));
            }
            ConvertBezierToPoints();
            render.RefreshCoefficients();
        }
    }

    void SortBezierPoints()
    {
        for(int i= 0; i < bezierPoints.Count-1; i++)
        {
            if(bezierPoints[i].center.pos.x > bezierPoints[i + 1].center.pos.x)
            {
                BezierPoint temp = bezierPoints[i];
                bezierPoints[i] = bezierPoints[i + 1];
                bezierPoints[i+1] = temp;
            }
        }
    }

    int2 ClampMousePos(int2 pos)
    {
        if(pos.x< 0)pos.x = 0;
        if(pos.y< 0)pos.y = 0;
        if(pos.x> text.width)pos.x = text.width;
        if(pos.y> text.height)pos.y = text.height;

        return pos;
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (BezierPoint bezierPoint in bezierPoints)
            {
                //check all 3 points
                if (ClickedPoint(bezierPoint.prevControlPoint))
                {
                    holdingCircle = true;
                    heldPoint = bezierPoint.prevControlPoint;
                    break;
                }

                if (ClickedPoint(bezierPoint.center))
                {
                    holdingCircle = true;
                    heldPoint = bezierPoint.center;
                    break;
                }

                if (ClickedPoint(bezierPoint.nextControlPoint))
                {
                    holdingCircle = true;
                    heldPoint = bezierPoint.nextControlPoint;
                    break;
                }
            }
        }

        if (holdingCircle)
        {
            Vector2 mousePos = MousePosToTextCoord();
            int2 posInt2 = new int2((int)mousePos.x, (int)mousePos.y);

            //convert mousePos to image coords rect;
            
            heldPoint.pos = ClampMousePos(posInt2);

            SortBezierPoints();
            render.RefreshCoefficients();
        }

        if (Input.GetMouseButtonUp(0))
        {
            holdingCircle = false;            
        }

        if (Input.GetMouseButtonDown(1))
        {
            bool deleted = false;
            for(int i=0;i<bezierPoints.Count;i++)
            {                

                if (ClickedPoint(bezierPoints[i].center))
                {
                    deleted = true;
                    bezierPoints.RemoveAt(i);
                    break;
                }
            }
            if (!deleted)
            {
                CreateNewBezierPoint();
            }
            
        }
    }


    void ConvertBezierToPoints()
    {
        int number = 0; 
        for(int i = 0; i < bezierPoints.Count; i++)
        {
            //if first omit prev control point
            if (i == 0)
            {
                pointsArray[number++] = bezierPoints[i].center.pos;                
                pointsArray[number++] = bezierPoints[i].nextControlPoint.pos;
            }
            //if last omit next control point
            else if (i == bezierPoints.Count-1)
            {
                pointsArray[number++] = bezierPoints[i].prevControlPoint.pos;
                pointsArray[number++] = bezierPoints[i].center.pos;                
                
            }
            //if standard include all of them
            else
            {
                pointsArray[number++] = bezierPoints[i].prevControlPoint.pos;
                pointsArray[number++] = bezierPoints[i].center.pos;
                pointsArray[number++] = bezierPoints[i].nextControlPoint.pos;
            }
        }


        pointsNumber[0] = number;


    }
    /// <summary>
    /// if modified update curve!
    /// updates texture for curve
    /// </summary>
    void DrawCurve()
    {
        
        pointsBuffer.SetData(pointsArray);
        pointsNumberBuffer.SetData(pointsNumber);


        shader.SetBuffer(0, "points", pointsBuffer);
        shader.SetBuffer(0, "pointsNumber", pointsNumberBuffer);


        shaderReset.Dispatch(0, width / 8, height / 8, 1); 
        shader.Dispatch(0, width / 8, height / 8, 1); 
    }


    public List<BezierPoint> CopyBezierPoints()
    {

        return Extensions.Clone(bezierPoints);
    }

    public void SetBezierPoints(List<BezierPoint> newPoints)
    {
        bezierPoints = newPoints;
        ConvertBezierToPoints();
    }


    /// <summary>
    /// return value of bezier curve.
    /// t € <0,1>
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public float Evaluate(float t)
    {
        
        int x = (int)(t * text.width);

        bool found = false;
        bool beforeFirst = false;
        int prevPointIndex = 0;
        for(int i =0; i<bezierPoints.Count; i++)
        {
            if (x <= bezierPoints[i].center.pos.x)
            {                
                found = true;
                if (i == 0)
                {
                    beforeFirst = true;
                }
                else
                {
                    prevPointIndex = i-1;
                }
                break;
            }
        }

        float result=0;
        if (!found)
        {
            //interpolate from the end
            
            result = bezierPoints[bezierPoints.Count - 1].center.pos.y;
        }
        else if (beforeFirst)
        {
            //interpolate from the beg
            result = bezierPoints[0].center.pos.y;
        }
        else
        {

            prevPointIndex *= 3;//convert bezierpoint index do pointsarray index

            float sum = 0;
            float targetF
                =0;
            //standard calc estimation
            for(float f = 0; f <= 1; f += 0.01f)
            {
                sum = 0;
                for (int i = 0; i < 4; i++)
                {
                    sum += ((i==2 || i == 1)? 3 : 1) * math.pow(f, i) * math.pow(1 - f, 3 - i) * pointsArray[prevPointIndex + i].x;                    
                }
                if (sum >= x)
                {
                    //found f (aproximately) xd
                    targetF = f;
                    break;
                }
            }

            //caly y
            for (int i = 0; i < 4; i++)
            {
                result += ((i == 2 || i == 1) ? 3 : 1) * math.pow(targetF, i) * math.pow(1 - targetF, 3 - i) * pointsArray[prevPointIndex + i].y;                
            }
        }

        //convert result to <-1,1>
        result  = Mathf.Lerp(-1,1,result/text.height);

        return result;
    }


}
