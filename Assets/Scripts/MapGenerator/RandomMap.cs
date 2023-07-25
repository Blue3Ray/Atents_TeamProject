using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public struct Node
    {
        public bool data;
        public bool isChecked;
    }


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
    public Node[] nodes;

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

    /// <summary>
    /// 작은 방 기준
    /// </summary>
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
                if (nodes[GetIndex(x, y)].data)
                {
                    //CheckSmallRoom(x, y);
                }
            }
        }
    }

    Stack<Node> stack = new();
    List<List<Node>> roomlist = new List<List<Node>>();
    List<Node> room = new List<Node>();

    List<Node> CheckRoomList(int x, int y)
    {
        stack.Push(nodes[GetIndex(x, y)]);                  // 노드를 스택에 넣는다

        if (nodes[GetIndex(x, y)].data)                     // 노드가 비어 있으면 검사 한다
        {
            room = new List<Node>();
            Node targetNode = stack.Pop();
            room.Add(targetNode);

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1;j++)
                {
                    if (i == 0 && j == 0) continue;
                    if(!targetNode.isChecked)
                    {
                        stack.Push(nodes[GetIndex(x + j, y + i)]);
                    }
                }
            }
        }
        else    // 노드가 안비어 있으면 바로 뺀다
        {
            stack.Pop();
            room = null;
        }
        return null;
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
                    nodes[GetIndex(x, y)].data = true;
                }
                else
                {
                    nodes[GetIndex(x, y)].data = false;
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
                    if (nodes[GetIndex(x + b, y + a)].data) boolTCount++;
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
        nodes = new Node[Width * Height];

        for (int i = 0; i < nodes.Length; i++)
        {
            // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
            nodes[i].data = Random.Range(0.0f, 1.0f) < mapFillRate;
            nodes[i].isChecked = false;
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
                    if (!nodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
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
