using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderScript : MonoBehaviour
{
            
    [SerializeField] Texture2D text;
    [SerializeField] Material material;


    private (int x, int y) size;


    float[,] array;//while not working simulation
    public Simulation simulation;
    // Start is called before the first frame update
    void Start()
    {
        size = simulation.GetSimulationSize();//while not working simulation

        array = new float[size.x, size.y]; //while not working simulation


        text = new Texture2D(size.x, size.y, TextureFormat.ARGB32,false);
        for(int i=0; i < size.x; i++)
        {
            text.SetPixel(i, i, Color.red);
        }
        text.Apply();        
        
        material.mainTexture = text;

        //Time.timeScale = (1 / 50f) * ( 4f );

    }    
    


    /// <summary>
    /// converts array of valus to texture to render.
    /// It applies all changes so the render will change!
    /// </summary>
    /// <param name="array"></param>
    public void ConvertArrayToTexture(float[,] array)
    {
        
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                text.SetPixel(i, j, new Color(array[i, j], array[i, j], array[i, j])); //najbardziej problematyczna linijka
            }
        }
       
        text.Apply();
    }
}
