using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCoeefs : MonoBehaviour
{
    public RenderScript render;

    public ComputeShader shader;
    ComputeBuffer coefficientsBuffer;
    ComputeBuffer rangeBuffer;

    protected RenderTexture text;

    const int COEFFWIDTH = 21;

    int[] range = new int[1];
    private void Start()
    {
        coefficientsBuffer = new ComputeBuffer(COEFFWIDTH * COEFFWIDTH, 4);
        rangeBuffer = new ComputeBuffer(1, 4);

        text = new RenderTexture(1024, 1024, 24);
        text.enableRandomWrite = true;
        text.Create();

        gameObject.GetComponent<RawImage>().texture = text;

        shader.SetTexture(0, "Result", text);
    }

    void Update()
    {
        coefficientsBuffer.SetData(render.GetCoeffs());

        range[0] = render.GetSimRange();
        rangeBuffer.SetData(range);

        shader.SetBuffer(0,"coeffs",coefficientsBuffer);
        shader.SetBuffer(0, "simRange", rangeBuffer);

        shader.Dispatch(0, 1024 / 8, 1024 / 8, 1);
    }
}
