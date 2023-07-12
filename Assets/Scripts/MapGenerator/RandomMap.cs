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

    /// <summary>
    /// ³ëµåµé
    /// </summary>
    public bool[] nodes;

    List<List<Vector2Int>> rooms = new();

    [Range(0,1)]
    public float mapFillRate = 0.8f;

    public int collectBoxBoolCount = 3;


    private void OnValidate()
    {
        ResetMap();
        for (int i = 0; i < collectBoxBoolCount; i++)
        {
            GatherData();
        }
    }

    void CheckSmallNodes()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (nodes[GetIndex(x,y)])
                {
                    Stack nodeStack = new Stack();
                    nodeStack.Push(nodes[GetIndex(x,y)]);
                }
            }
        }
    }

    void IsAlreadySteak(int x, int y)
    {
        
    }

    void GatherData()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckNearNodesBool(x, y))
                {
                    nodes[GetIndex(x, y)] = true;
                }
                else
                {
                    nodes[GetIndex(x, y)] = false;
                }
            }
        }
    }

    bool CheckNearNodesBool(int x, int y)
    {
        int count = 0;
        int boolTCount = 0;
        for(int a = -1; a <= 1; a++)
        {
            for(int b = -1; b <= 1; b++)
            {
                // ¸Ê ¹Ù±ùÀÏ ¶§ false·Î Ã³¸®
                if((x + b < 0 || x + b >= width)||(y + a < 0 || y + a >= height))
                {
                    //boolCount++;
                }
                else
                {
                    // ±ÙÃ³ Å¸°Ù ³ëµå°¡ true ÀÏ¶§ ture Ä«¿îÆ® Áõ°¡
                    if (nodes[GetIndex(x + b, y + a)]) boolTCount++;
                }

                //if((x + b >= 0 && x + b < width) && (y + a >= 0 && y + a < height))
                //{
                //    //Debug.Log($"{x + b}, {y + a}");
                //    if (boxes[GetIndex(x + b, y + a)]) boolCount++;
                //}
                count++;
            }
        }

        return (boolTCount - (count * 0.5f)) > 0f;
    }


    void ResetMap()
    {
        nodes = new bool[Width * Height];

        for (int i = 0; i < nodes.Length; i++)
        {
            // fiilrateº¸´Ù ÀÛÀ¸¸é true(ºóÄ­) ¾Æ´Ï¸é false(°ËÀºÄ­)
            nodes[i] = Random.Range(0.0f, 1.0f) < mapFillRate;
        }
    }

    private void OnDrawGizmos()
    {
        if (nodes.Length > 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Gizmos.color = Color.black;
                    // false¸é °ËÀºÄ­, true¸é ºóÄ­
                    if (!nodes[GetIndex(x, y)]) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                }
            }
        }
    }

    int GetIndex(int x, int y)
    {
        return x + y * width;
    }
}
