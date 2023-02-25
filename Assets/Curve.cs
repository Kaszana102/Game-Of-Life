using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Curve : EditorWindow
{
    AnimationCurve curveX = AnimationCurve.Linear(0, 0, 10, 10);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [MenuItem("Custom/Generate City %g")]
    public static void OpenWindow()
    {
        GetWindow<Krzywa>();
    }


    void OnGUI()
    {
        curveX = EditorGUILayout.CurveField("Animation on X", curveX);
    }
}
