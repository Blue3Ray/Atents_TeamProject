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
    /// ����
    /// </summary>
    public Node[] nodes;

    List<Room> rooms = new();

    public class Room
    {
        public List<Vector2> nodes = new List<Vector2>();
    }

    /// <summary>
    /// �ʱ� ���� bool ä��� ���� (���� 0.45 ~ 0.47 ����)
    /// </summary>
    [Range(0,1)]
    public float mapFillRate = 0.8f;

    /// <summary>
    /// ����ȭ ��Ű�� Ƚ��
    /// </summary>
    public int collectBoxBoolCount = 3;

    /// <summary>
    /// ���� �� ����
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
                    List<Node> list = CheckRoomList(x, y);
                    if (list != null)
                    {
                        roomlist.Add(list);
                        Debug.Log($"room list : {roomlist.Count} => ({x}, {y}), Count : {list.Count}");
                    }
                }
            }
        }
    }

    Stack<Node> stack = new();


    List<List<Node>> roomlist = new List<List<Node>>();


    List<Node> CheckRoomList(int x, int y)
    {
        if (nodes[GetIndex(x, y)].isChecked) return null;
        stack.Push(nodes[GetIndex(x, y)]);                  // ��带 ���ÿ� �ִ´�
        nodes[GetIndex(x, y)].isChecked = true;

        int a = x;
        int b = y;
        List<Node> room = new List<Node>();

        while(stack.Count > 0)
        {
            Node target = stack.Pop();
            room.Add(target);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i*j == 0 && a + j > -1 && a + j < width && b + i > -1 && b + i < height)
                    {
                        if (nodes[GetIndex(a + j, b + i)].data && !nodes[GetIndex(a + j, b + i)].isChecked)
                        {
                            Debug.Log($"({a + j}, { b + i}) is empty");
                            stack.Push(nodes[GetIndex(a + j, b + i)]);
                            nodes[GetIndex(a + j, b + i)].isChecked = true;
                        }
                    }
                }
            }

            a += 1;
            if(a >= width)
            {
                a = 0;
                b += 1;
            }
        }

        if (room.Count > 0) return room;
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
                nodes[GetIndex(x, y)].isChecked = false;
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
                // �� �ٱ��� �� false�� ó��
                if(CheckInMap(x + b, y + a))
                {
                    //boolCount++;
                }
                else
                {
                    // ��ó Ÿ�� ��尡 true �϶� ture ī��Ʈ ����
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
            // fiilrate���� ������ true(��ĭ) �ƴϸ� false(����ĭ)
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
                    // false�� ����ĭ, ���� Ȯ�εȰŸ� ����, true�� ��ĭ
                    if (!nodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    else if(nodes[GetIndex(x, y)].isChecked)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    }
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
