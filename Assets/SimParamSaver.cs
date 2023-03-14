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

}

static class Extensions
{
    public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}