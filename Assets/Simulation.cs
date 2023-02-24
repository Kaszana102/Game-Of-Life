using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    

    public float[,] map; //w sumie, czy nie powinno byæ prywatno/chronione? Nie chcemy by coœ z zewn¹rz to modyfikowa³o.
    private int sizeX;
    private int sizeY; 
    private bool simulate = true;

    public RenderScript renderScript; // odwo³anie do skryptu zwi¹zanego z renderem

    [SerializeField] private int calculation_radius = 16;
    [SerializeField] private int resolution = 2;
    void Start()
    {
        sizeX = Screen.width * resolution;
        sizeY = Screen.height * resolution;
        map = new float[sizeX,sizeY];     //map[x,y]
    }

    // Update is called once per frame
    void Update()
    {
        if(simulate)
        {
            float[,] oldMap = (float[,])map.Clone();
            for (int x=0;x<sizeX;x++)
            {
                for(int y=0;y<sizeY;y++)
                {
                    calculateFieldValue(x, y,oldMap);
                }
            }
        }
    }
    void calculateFieldValue(int x,int y,float[,] input)
    {
        float newValue = 0; //new value of the field
        //restrictions for checking if the point is in the calculation radius
        int topRestriction = Mathf.Max(0, y - calculation_radius);
        int bottomRestriction = Mathf.Min(sizeY, y + calculation_radius);
        int leftRestriction = Mathf.Max(0, x - calculation_radius);
        int rightRestriction = Mathf.Min(sizeX, x + calculation_radius);

        for(int i=leftRestriction;i<rightRestriction;i++)
        {
            for(int j=topRestriction;j<bottomRestriction;j++)
            {
                float r = Mathf.Sqrt(Mathf.Pow(x - i, 2) + Mathf.Pow(y - j, 2)); //distance between 2 points
                if (r <= calculation_radius)
                    newValue += input[i, j]*(r/calculation_radius);//razy jakas funkcja(r/calculation_radius)
            }
        }
        map[x, y] = Mathf.Clamp(newValue,0,1);
    }


    /// <summary>
    /// zwraca nie przetworzon¹ macierz punktów
    /// </summary>
    /// <returns></returns>
    void ActualizeRender()
    {
        renderScript.ConvertArrayToTexture(map);// refresh render
    }

    /// <summary>
    /// return the size of simulation
    /// </summary>
    /// <returns></returns>
    public (int,int) GetSimulationSize()
    {
        return (sizeY,sizeX);   
    }
}
