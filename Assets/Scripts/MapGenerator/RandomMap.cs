using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class RandomMap : MonoBehaviour
{
    public class Node
    {
        public bool data;
        public bool isChecked;
        public Vector2Int gridPos;
    }


    public int width;
    public int Width
    {
        get => width;
        set 
        { 
            width = value; 
        }
    }
    public int height;
    public int Height
    {
        get => height;
        set 
        { 
            height = value;
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
                    List<Node> list = CheckRoomList(x, y);

                    if (list != null)       // 처음으로 체크 한 녀석이면
                    {
                        roomlist.Add(list);
                        //Debug.Log($"room list : {roomlist.Count} => ({x}, {y}), Count : {list.Count}");
                    }
                }
            }
        }
    }

    void Start()
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

                    if (list != null)       // 처음으로 체크 한 녀석이면
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
        List<Node> room = new List<Node>();

        if (nodes[GetIndex(x, y)].isChecked) return null;

        stack.Push(nodes[GetIndex(x, y)]);                  // 노드를 스택에 넣는다
        nodes[GetIndex(x, y)].isChecked = true;



        while (stack.Count > 0)
        {
            Node target = stack.Pop();
            room.Add(target);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i * j == 0 && target.gridPos.x + j > -1 && target.gridPos.x + j < width && target.gridPos.y + i > -1 && target.gridPos.y + i < height)
                    {
                        Node tempTarget = nodes[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
                        if (tempTarget.data && !tempTarget.isChecked)
                        {
                            //Debug.Log($"({tempTarget.gridPos}) is empty(i : {i}, j : {j})");
                            stack.Push(tempTarget);
                            tempTarget.isChecked = true;
                        }
                    }
                }
            }
        }

        return room;

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
            nodes[i] = new Node();
            // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
            nodes[i].data = Random.Range(0.0f, 1.0f) < mapFillRate;
            nodes[i].isChecked = false;
            nodes[i].gridPos = new Vector2Int(i % Width, i / width);
            
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

#if UNITY_EDITOR
    /// <summary>
    /// 테스트용 리스트 확인 함수
    /// </summary>
    public void PrintRoomlist()
    {
        Debug.Log($"총 {roomlist.Count}개 방");
        int i = 0;
        foreach(List<Node> room in roomlist)
        {
            Debug.Log($"{i}번째 {room.Count}개 방");
        }
    }



    private void OnDrawGizmos()
    {
        if (nodes != null && nodes.Length > 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Gizmos.color = Color.black;
                    // false면 검은칸, 스택 확인된거면 빨강, true면 빈칸
                    if (!nodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    else if (nodes[GetIndex(x, y)].isChecked)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    }
                }
            }
        }
    }
#endif
}
