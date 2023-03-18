using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

/// <summary>
/// Przez niego s¹ wysy³ane zmiany do symulacji typu:
/// -czy symulujemy
/// -zasiêg symulacji
/// 
/// -si³a brusha (gdy 0 to po prostu nic siê nie maluje)
/// -œrodek brusha (pozwalamy tylko na rysowanie kó³)
/// -si³a - czyli ile dodaje do ka¿dego pola
/// </summary>
public class RenderScript : MonoBehaviour
{
    //Shader resources
    public ComputeShader shader;    
    [SerializeField] RenderTexture TextResult;
    public RenderTexture TextSource;
    RawImage image;
          

    bool srcToRes = true;


    int width = 1024;
    int height = 1024;

    public struct SimData
    {
        public int simRange;
        public int simulating;
        public int resetMap;

    }
    SimData[] simData = new SimData[1];
    ComputeBuffer simBuffer;
    public struct BrushData
    {

        //brush parameteres
        public float strength;
        public int brushRange;
        public uint2 brushCenter;        
    }
    BrushData[] brushData = new BrushData[1];
    ComputeBuffer brushBuffer;

    const int COEFFWIDTH = 21;

    float[] coefficients = new float[COEFFWIDTH * COEFFWIDTH];                   
    ComputeBuffer coefficientsBuffer;

    bool circleToDraw = false;
    bool taskToResetMap = false;

    public BezierCurve bezier;
    // Start is called before the first frame update
    void Start()
    {
        //creating buffers
        simBuffer = new ComputeBuffer(1, 4+4+4);
        brushBuffer = new ComputeBuffer(1, 16);
        coefficientsBuffer = new ComputeBuffer(COEFFWIDTH* COEFFWIDTH, 4);


        //creatung textures
        TextResult = new RenderTexture(width, height, 24);
        TextResult.enableRandomWrite = true;
        TextResult.Create();


        TextSource = new RenderTexture(width, height, 24);
        TextSource.enableRandomWrite = true;
        TextSource.Create();



        //initializing start values
        //sim
        simData[0].simRange = 1;
        simData[0].simulating = 1;

        //brush
        brushData[0].strength = 0f;
        brushData[0].brushRange = 2;
        brushData[0].brushCenter = uint2.zero;

        image = gameObject.GetComponent<RawImage>();
        
    }

    // Update is called once per frame
    void Update()
    {                        
        brushBuffer.SetData(brushData);
        simBuffer.SetData(simData);
        coefficientsBuffer.SetData(coefficients);

        shader.SetBuffer(0, "simBuffer", simBuffer);        
        shader.SetBuffer(0, "brush", brushBuffer);
        shader.SetBuffer(0, "coeff", coefficientsBuffer);

        if (srcToRes) //swap textures for simulation
        {
            shader.SetTexture(0, "Result", TextResult);
            shader.SetTexture(0, "Source", TextSource);            
        }
        else
        {
            shader.SetTexture(0, "Result", TextSource);
            shader.SetTexture(0, "Source", TextResult);
        }

        

        shader.Dispatch(0, TextSource.width / 8, TextSource.height / 8, 1); //one frame of simulation

        //set texture
        if (srcToRes)
        {
            image.texture = TextResult;
        }
        else
        {
            image.texture = TextSource;
        }


        srcToRes = !srcToRes;
        if (circleToDraw)
        {
            circleToDraw = false;
            brushData[0].strength = 0f;
        }
        
        simData[0].resetMap = 0;
        


    }

    public void SetiSimulating(bool val)
    {
        simData[0].simulating=val? 1:0;
    }
    public bool GetiSimulating()
    {
        return simData[0].simulating == 1 ? true : false;
    }

    public void SimRange(float range)
    {
        simData[0].simRange=(int)range;
    }

    public int GetSimRange()
    {
        return simData[0].simRange;
    }


    public void SetBrushStrength(float strength)
    {
        brushData[0].strength=strength;
    }
    public void SetBrushRange(float range)
    {
        brushData[0].brushRange= (int)range;
    }

    public void SetBrushCenter(uint2 center)
    {
        brushData[0].brushCenter=center;
    }

    public void DrawCircle(uint x, uint y,int range, float strength)
    {
        SetBrushCenter(new uint2(x, y));
        SetBrushRange(range);
        SetBrushStrength(strength);
        circleToDraw = true;
    }


    public void RefreshCoefficients()
    {        
        for (int i = 0; i < COEFFWIDTH; i++)
        {
            for (int j = 0; j < COEFFWIDTH; j++)
            {
                if (Vector2.Distance(new Vector2(i, j), new Vector2(COEFFWIDTH / 2, COEFFWIDTH / 2)) <= simData[0].simRange)
                {
                    float coef = bezier.Evaluate(Vector2.Distance(new Vector2(i, j), new Vector2(COEFFWIDTH / 2, COEFFWIDTH / 2)) / (simData[0].simRange));
                    coefficients[i * COEFFWIDTH + j] = coef;  
                    
                }
                else
                {
                    coefficients[i * COEFFWIDTH + j] = 0;                    
                }
            }
        }
    }

    public float[] GetCoeffs()
    {
        return coefficients;
    }

    public void ResetMap()
    {
        simData[0].resetMap = 1;
    }

}
