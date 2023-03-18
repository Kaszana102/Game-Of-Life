using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SimParamSaver : MonoBehaviour
{
    public RenderScript render;
    public BezierCurve bezierCurve;

    public Slider indexSlider;

    public Slider simRangeSlider;

    public struct allSimParams
    {
        public int simRange;
        public List<BezierPoint> points;
        public bool saved;
    }

    allSimParams[] paramsList = new allSimParams[5];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < paramsList.Length; i++)
        {
            paramsList[i].saved = false;
        }
    }


    public void SaveActParams()
    {
        int index = ((int)indexSlider.value) - 1;

        paramsList[index].simRange = render.GetSimRange();
        paramsList[index].points = bezierCurve.CopyBezierPoints();
        paramsList[index].saved = true;
    }

    public void RestoreParams()
    {
        int index = ((int)indexSlider.value) - 1;
        if (paramsList[index].saved)
        {
            render.SimRange(paramsList[index].simRange);
            simRangeSlider.value = paramsList[index].simRange;
            bezierCurve.SetBezierPoints(Extensions.Clone(paramsList[index].points));
            render.RefreshCoefficients();
        }
    }


    public void RestoreSpecialParams(int index)
    {
        int specialSimRange=1;
        List<BezierPoint> specialPoints = new List<BezierPoint> ();
        switch (index)
        {
            case 0:
                //bizmut
                specialSimRange = 15;
                specialPoints.Add(new BezierPoint(new int2(64,89), new int2(52,269), new int2(55,0),true));
                specialPoints.Add(new BezierPoint(new int2(95,338), new int2(145,449), new int2(195,338), true));
                specialPoints.Add(new BezierPoint(new int2(188,110), new int2(264,350), new int2(312,144), true));                
                specialPoints.Add(new BezierPoint(new int2(306,392), new int2(357,489), new int2(406,392), true));                
                specialPoints.Add(new BezierPoint(new int2(400,200), new int2(436,57), new int2(600,200), true));



                /*
                specialSimRange = 15;
                specialPoints.Add(new BezierPoint(new int2(64,89), new int2(52,269), new int2(55,0),true));
                specialPoints.Add(new BezierPoint(new int2(77,329), new int2(145,449), new int2(178,334), true));
                specialPoints.Add(new BezierPoint(new int2(188,110), new int2(267,364), new int2(312,144), true));                
                specialPoints.Add(new BezierPoint(new int2(306,392), new int2(357,489), new int2(406,392), true));                
                specialPoints.Add(new BezierPoint(new int2(400,200), new int2(436,57), new int2(600,200), true)); 
                  */

                break;
            case 1:
                //komórki?
                specialSimRange = 15;
                specialPoints.Add(new BezierPoint(new int2(64,89), new int2(37,176), new int2(55,0),true));
                specialPoints.Add(new BezierPoint(new int2(95,338), new int2(145,449), new int2(195,338),true));
                specialPoints.Add(new BezierPoint(new int2(188,110), new int2(260,306), new int2(312,144),true));
                specialPoints.Add(new BezierPoint(new int2(306,392), new int2(365,403), new int2(406,392),true));
                specialPoints.Add(new BezierPoint(new int2(392,161), new int2(460,57), new int2(600,200),true));
                break;
            case 2:
                //fajny podzia³ komórek
                specialSimRange = 6;
                specialPoints.Add(new BezierPoint(new int2(27, 45), new int2(0, 5), new int2(61, 269), true));
                specialPoints.Add(new BezierPoint(new int2(97, 336), new int2(147, 343), new int2(197, 336), true));
                specialPoints.Add(new BezierPoint(new int2(379, 220), new int2(427, 193), new int2(600, 200), true));


                break;


            
        }

        render.SimRange(specialSimRange);
        simRangeSlider.value = specialSimRange;
        bezierCurve.SetBezierPoints(Extensions.Clone(specialPoints));
        render.RefreshCoefficients();
    }

}

static class Extensions
{
    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}