using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    
    private struct Point //a struct to keep all the information needed about a point to perform simulation calculations
    {
        public Point(int x, int y,float r)
        {
            X = x;
            Y = y;
            distance = r;
        }

        public int X { get; }
        public int Y { get; }
        public float distance { get; } //wartoœæ z przedzia³u od 0 do 1 u¿ywana do obliczania wagi
    }


    public float[,] map;
    public float[,] map2;

    private List<Point>[,] pointsInCircle;
    private int sizeX;
    private int sizeY;
    private bool mapChoice = true; //which map to use to avoid cloning the map
    private bool simulate = false;


    [SerializeField] private int calculation_radius = 16;
    [SerializeField] private int resolution = 2;
    void Start()
    {
        sizeX = Screen.width * resolution;
        sizeY = Screen.height * resolution;
        map = new float[sizeX,sizeY];
        map2 = new float[sizeX, sizeY]; // 2*map[x,y]
        pointsInCircle = new List<Point>[sizeX, sizeY]; //points that are included for the calculations(in the calc radius) for a specific point
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                findPointsInTheCircle(x,y);
            }
        }
        simulate = true;
    }
    private void Update()
    {
        if (simulate)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    calculateFieldValue(x, y);
                }
            }
            mapChoice = !mapChoice; //switch which map is used as an input and which as output
        }
    }
    void calculateFieldValue(int x,int y)
    {
        float newValue = 0; //new value of the field
        if (mapChoice)
        {
            foreach (Point point in pointsInCircle[x, y])
            {
                newValue += map[point.X, point.Y] * (point.distance);//razy jakas funkcja(r/calculation_radius)
            }
            map2[x, y] = Mathf.Clamp(newValue, 0, 1);
        }else
        {
            foreach (Point point in pointsInCircle[x, y])
            {
                newValue += map2[point.X, point.Y] * (point.distance);//razy jakas funkcja(r/calculation_radius)
            }
            map[x, y] = Mathf.Clamp(newValue, 0, 1);
        }
    }
    void findPointsInTheCircle(int x, int y)
    {
        pointsInCircle[x, y] = new List<Point>();
        int topRestriction = Mathf.Max(0, y - calculation_radius);
        int bottomRestriction = Mathf.Min(sizeY, y + calculation_radius);
        int leftRestriction = Mathf.Max(0, x - calculation_radius);
        int rightRestriction = Mathf.Min(sizeX, x + calculation_radius);

        for (int i = leftRestriction; i < rightRestriction; i++)
        {
            for (int j = topRestriction; j < bottomRestriction; j++)
            {
                float r = Mathf.Sqrt(Mathf.Pow(x - i, 2) + Mathf.Pow(y - j, 2)); //distance between 2 points
                if (r <= calculation_radius && (i != x) && (j != y))
                    pointsInCircle[x, y].Add(new Point(i, j, r / calculation_radius)); //adding points that are in the calculation circle to the list
            }
        }
    }
    public float[,] getMap() //returns a map that is up to date 
    {
        if(mapChoice)
        {
            return map;
        }else
        {
            return map2;
        }
          
    }
}
