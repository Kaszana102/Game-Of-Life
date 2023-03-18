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

    int COEFFWIDTH = RenderScript.COEFFWIDTH;

    int[] range = new int[1];
    private void Start()
    {
        coefficientsBuffer = new ComputeBuffer(COEFFWIDTH * COEFFWIDTH, 4);
        this.rangeBuffer = new ComputeBuffer(1, 4);

        text = new RenderTexture(1024, 1024, 24);
        text.enableRandomWrite = true;
        text.Create();

        gameObject.GetComponent<RawImage>().texture = text;

        shader.SetTexture(0, "Result", text);



        //set coeff ranges for compute shader
        int[] coeffMaxRange = new int[1];
        int[] coeffWidth = new int[1];
        ComputeBuffer rangeBuffer = new ComputeBuffer(1, 4);
        ComputeBuffer widthBuffer = new ComputeBuffer(1, 4);
        coeffMaxRange[0] = RenderScript.MAX_SIM_RANGE;
        rangeBuffer.SetData(coeffMaxRange);
        shader.SetBuffer(0, "COEFFMAXRANGE", rangeBuffer);
        coeffWidth[0] = RenderScript.COEFFWIDTH;
        widthBuffer.SetData(coeffWidth);
        shader.SetBuffer(0, "COEEFWIDTH", widthBuffer);


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
