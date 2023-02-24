using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Krzywa : EditorWindow
{
    AnimationCurve curveX = AnimationCurve.Linear(0, 0, 1, 0);
    AnimationCurve curveY = AnimationCurve.Linear(0, 0, 1, 1);
    AnimationCurve curveZ = AnimationCurve.Linear(0, 0, 1, 0);

    [MenuItem("Examples/Curve Field demo")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(Editor));
        window.position = new Rect(0, 0, 400, 199);
        window.Show();
    }

    void OnGUI()
    {
        curveX = EditorGUI.CurveField(
            new Rect(3, 3, position.width - 6, 50),
            "Animation on X", curveX);
        curveY = EditorGUI.CurveField(
            new Rect(3, 56, position.width - 6, 50),
            "Animation on Y", curveY);
        curveZ = EditorGUI.CurveField(
            new Rect(3, 109, position.width - 6, 50),
            "Animation on Z", curveZ);
    }

}