using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveTest : MonoBehaviour
{
    public AnimationCurve curve;

    public Vector3 offset = new Vector3(2,2,-1);

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(Vector3.zero, offset, curve.Evaluate(Time.time % 1));
    }
}
