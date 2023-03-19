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
            simRangeSlider.value = paramsList[index].simRange;
            //render.SimRange(paramsList[index].simRange);
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
                //dots
                specialSimRange = 15;
                specialPoints.Add(new BezierPoint(new int2(10,42), new int2(0,326), new int2(67,293),true));
                specialPoints.Add(new BezierPoint(new int2(156,282), new int2(232,366), new int2(280,243), true));
                specialPoints.Add(new BezierPoint(new int2(283,196), new int2(288,65), new int2(343,164), true));                
                specialPoints.Add(new BezierPoint(new int2(956,212), new int2(449,148), new int2(403,238), true));                
                



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
                specialSimRange = 9;
                specialPoints.Add(new BezierPoint(new int2(10,42), new int2(0,214), new int2(67,293),true));
                specialPoints.Add(new BezierPoint(new int2(88,240), new int2(125,280), new int2(190,279), true));
                specialPoints.Add(new BezierPoint(new int2(182,258), new int2(232,235), new int2(269,262), true));
                specialPoints.Add(new BezierPoint(new int2(282,253), new int2(350,282), new int2(389,246), true));
                specialPoints.Add(new BezierPoint(new int2(410,217), new int2(478,225), new int2(403,238), true));
                break;
            case 2:
                //fajny podzia³ komórek
                specialSimRange = 6;
                specialPoints.Add(new BezierPoint(new int2(27, 45), new int2(0, 5), new int2(61, 269), true));
                specialPoints.Add(new BezierPoint(new int2(97, 336), new int2(147, 343), new int2(197, 336), true));
                specialPoints.Add(new BezierPoint(new int2(379, 220), new int2(427, 193), new int2(600, 200), true));


                break;





                //specialPoints.Add(new BezierPoint(new int2(,), new int2(,), new int2(,),true));
                //specialPoints.Add(new BezierPoint(new int2(,), new int2(,), new int2(,), true));
        }

        //render.SimRange(specialSimRange);
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