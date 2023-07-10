using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public int width;
    public int Width
    {
        get { return width; }
        set { width = value; 
        ResetMap();
        }
    }
    public int height;
    public int Height
    {
        get => height;
        set { height = value;
        ResetMap() ;
        }
    }

    public float mapFillRate = 0.8f;

    public bool[] boxes;

    private void Start()
    {
        ResetMap();
    }

    void GatherData()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
            }
        }
    }

    bool CheckNearBoxesBool(int x, int y)
    {
        int count = 0;
        int boolCount = 0;
        for(int a = -1; a <= 1; a++)
        {
            for(int b = -1; b <= 1; b++)
            {
                if((x + b >= 0 || x + b < width) && (y + a >= 0 || y + b < height))
                {
                    if (boxes[GetIndex(x + b, y + a)]) boolCount++;
                    count++;
                }
            }
        }

        return boolCount / count > 0.5f;
    }


    void ResetMap()
    {
        boxes = new bool[Width * Height];

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i] = Random.Range(0.0f, 1.0f) < mapFillRate;
        }
    }

    private void OnDrawGizmos()
    {
        if (boxes.Length > 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Gizmos.color = Color.black;
                    if (boxes[GetIndex(x, y)]) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                }
            }
        }
    }

    int GetIndex(int x, int y)
    {
        return x + y * width;
    }
}
