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

    public Image loadPresetImage;

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

    private void Update()
    {
        int index = ((int)indexSlider.value) - 1;
        if (paramsList[index].saved)
        {
            loadPresetImage.color = Color.green;
        }
        else {
            loadPresetImage.color = Color.gray;
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
            case 3:
                specialSimRange = 15;
                specialPoints.Add(new BezierPoint(new int2(15, 322), new int2(0, 216), new int2(62, 160), true));
                specialPoints.Add(new BezierPoint(new int2(80, 206), new int2(107, 198), new int2(118, 253), true));
                specialPoints.Add(new BezierPoint(new int2(143, 245), new int2(123, 322), new int2(165, 193), true));
                specialPoints.Add(new BezierPoint(new int2(165, 275), new int2(178, 358), new int2(216, 314), true));
                specialPoints.Add(new BezierPoint(new int2(262, 305), new int2(283, 222), new int2(345, 206), true));
                specialPoints.Add(new BezierPoint(new int2(360, 224), new int2(321, 21), new int2(413, 271), true));
                specialPoints.Add(new BezierPoint(new int2(419, 173), new int2(500, 248), new int2(600, 200), true));
                break;
            case 4:
                
                specialSimRange = 11;
                specialPoints.Add(new BezierPoint(new int2(10, 42), new int2(0, 418), new int2(37, 238), true));
                specialPoints.Add(new BezierPoint(new int2(28, 144), new int2(47, 89), new int2(279, 245), true));
                specialPoints.Add(new BezierPoint(new int2(156, 235), new int2(216, 295), new int2(249, 172), true));
                specialPoints.Add(new BezierPoint(new int2(296, 387), new int2(350, 381), new int2(428, 283), true));
                specialPoints.Add(new BezierPoint(new int2(250, 50), new int2(484, 15), new int2(343, 164), true));
                break;
            case 5:
                
                specialSimRange = 10;
                specialPoints.Add(new BezierPoint(new int2(27, 45), new int2(0, 251), new int2(67, 500), true));
                specialPoints.Add(new BezierPoint(new int2(71, 500), new int2(274, 500), new int2(258, 0), true));
                specialPoints.Add(new BezierPoint(new int2(368, 203), new int2(489, 0), new int2(600, 200), true));

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