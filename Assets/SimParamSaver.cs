using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }

    allSimParams[] paramsList = new allSimParams[5];

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SaveActParams()
    {
        int index = ((int)indexSlider.value) - 1;

        paramsList[index].simRange = render.GetSimRange();
        paramsList[index].points = bezierCurve.CopyBezierPoints();
    }

    public void RestoreParams()
    {
        int index = ((int)indexSlider.value) - 1;

        render.SimRange(paramsList[index].simRange);
        simRangeSlider.value = paramsList[index].simRange;
        bezierCurve.SetBezierPoints(Extensions.Clone(paramsList[index].points));

    }

}

static class Extensions
{
    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}