using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public int width;
    public int Width
    {
        get => width;
        set 
        { 
            width = value; 
            ResetMap();
        }
    }
    public int height;
    public int Height
    {
        get => height;
        set 
        { 
            height = value;
            ResetMap() ;
        }
    }

    /// <summary>
    /// 노드들
    /// </summary>
    public bool[] nodes;

    List<Room> rooms = new();

    public class Room
    {
        public List<Vector2> nodes = new List<Vector2>();
    }

    /// <summary>
    /// 초기 랜덤 bool 채우는 정도 (대충 0.45 ~ 0.47 적당)
    /// </summary>
    [Range(0,1)]
    public float mapFillRate = 0.8f;

    /// <summary>
    /// 집약화 시키는 횟수
    /// </summary>
    public int collectBoxBoolCount = 3;

    public int smallRoomLimt = 20;


    private void OnValidate()
    {
        ResetMap();

        for (int i = 0; i < collectBoxBoolCount; i++)
        {
            GatherData();
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (nodes[GetIndex(x, y)])
                {
                    //CheckSmallRoom(x, y);
                }
            }
        }
    }

    void CheckSmallRoom(int x, int y)
    {
        Stack<Vector2Int> nodeStack = new Stack<Vector2Int>();
        nodeStack.Push(new Vector2Int(x,y));
        Room room = new Room();

        Test(room, nodeStack);
    }

    List<Vector2Int> Test(Room room, Stack<Vector2Int> stack)
    {
        Vector2Int temp = stack.Pop();
        stack.Pop();
        return null;
    }

    bool IsAlreadyStack(int x, int y)
    {
        bool result = false;

        return result;
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
                // 맵 바깥일 때 false로 처리
                if(CheckInMap(x + b, y + a))
                {
                    //boolCount++;
                }
                else
                {
                    // 근처 타겟 노드가 true 일때 ture 카운트 증가
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
            // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
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
                    // false면 검은칸, true면 빈칸
                    if (!nodes[GetIndex(x, y)]) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                }
            }
        }
    }

    bool CheckInMap(int x, int y)
    {
        return (x < 0 || x >= width) || (y < 0 || y >= height);
    }

    int GetIndex(int x, int y)
    {
        return x + y * width;
    }
}
