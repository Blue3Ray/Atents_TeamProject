using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class RandomMap : MonoBehaviour
{
    /// <summary>
    /// 맵 생성할 때 사용할 Node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 노드가 빈칸인지 아닌지 판별하는 bool
        /// </summary>
        public bool data;
        /// <summary>
        /// 노드가 RoomList 생성할 때 이미 검사를 했는지 안했는지 여부
        /// </summary>
        public bool isChecked;
        /// <summary>
        /// 노드 위치
        /// </summary>
        public Vector2Int gridPos;

        /// <summary>
        /// 노드 클래스 초기화
        /// </summary>
        /// <param name="data">참일경우 빈칸, 거짓일 경우 빈칸 아님</param>
        /// <param name="pos">노드 위치</param>
        /// <param name="isChecked">초기 값은 false</param>
        public Node(bool data, Vector2Int pos, bool isChecked = false)
        {
            this.data = data;
            this.gridPos = pos;
            this.isChecked = isChecked;
        }
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
        /// <summary>
        /// 인덱서 (막상 만들고 나니 쓸일이 없음)
        /// </summary>
        /// <param name="index">확인할 index</param>
        /// <returns>index에 해당하는 index 반환</returns>
        public Node this[int index] => nodes[index];

        public List<Node> nodes = new List<Node>();

        public int minX;
        public int maxX;
        public int minY;
        public int maxY;

        public void SetXYData()
        {
            minX = minY = int.MaxValue;
            maxX = maxY = -1;
            foreach (Node item in nodes)
            {
                if(item.gridPos.x < minX)
                {
                    minX = item.gridPos.x;
                }
                else if(item.gridPos.x > maxX)
                {
                    maxX = item.gridPos.x;
                }

                if (item.gridPos.y < minY)
                {
                    minY = item.gridPos.y;
                }
                else if (item.gridPos.y > maxY)
                {
                    maxY = item.gridPos.y;
                }
            }
        }
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

    /// <summary>
    /// Room(Node의 리스트)들을 가지고 있는 리스트
    /// </summary>
    List<Room> roomlist = new List<Room>();


    private void OnValidate()
    {
        // 인스펙터 값이 바뀔 때 마다 실행
        StartMethods();
    }

    void Start()
    {
        StartMethods();
    }

    /// <summary>
    /// RoomList를 랜덤하게 생성하는 함수
    /// </summary>
    public void StartMethods()
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
                Room tempRoom = CheckRoomList(x, y);

                if (tempRoom != null)       // 정상적으로 검사를 마치면
                {
                    roomlist.Add(tempRoom);     // 리스트에 추가
                    Debug.Log($"room list : {roomlist.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                        $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
                }
            }
        }
    }

    /// <summary>
    /// 특정 노드에서 연결되어 있는 노드들을 찾아 방 클래스를 반환하는 함수
    /// </summary>
    /// <param name="x">노드의 X좌표</param>
    /// <param name="y">노드의 Y좌표</param>
    /// <returns>해당 노드가 포함된 Room 클래스(이미 체크한 경우나 검사가 안되는 노드(벽)일 경우 null)</returns>
    Room CheckRoomList(int x, int y)
    {
        // 해당 노드가 이미 체크가 되거나 체크안하는 노드일 경우
        if (nodes[GetIndex(x, y)].isChecked || !nodes[GetIndex(x, y)].data) return null;

        Room room = new();
        Stack<Node> stack = new();

        stack.Push(nodes[GetIndex(x, y)]);                  // 노드를 스택에 넣는다(스택 1)
        nodes[GetIndex(x, y)].isChecked = true;             // 확인 함을 표시

        while (stack.Count > 0)             // 스택에 아무것도 없을 때까지 반복(해당 노드와 연결되어 있는 모든 노드를 검사 하면 끝난다)
        {
            Node target = stack.Pop();      // 스택의 맨위를 꺼낸다
            room.nodes.Add(target);         // 꺼낸 노드를 리스트에 추가한다.

            // target 노드의 상하좌우 노드를 검사한다.(+모양 위치만, 대각선 제외)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // 맵의 바깥일 경우를 거른다.
                    if (i * j == 0 && CheckInMap(target.gridPos.x + j, target.gridPos.y + i))
                    {
                        Node tempTarget = nodes[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
                        // 주변 노드 중 검사를 아직 안하고 빈칸인 노드를 확인한다.
                        if (tempTarget.data && !tempTarget.isChecked)
                        {
                            stack.Push(tempTarget);                 // 노드를 스택에 넣는다
                            tempTarget.isChecked = true;            // 확인함을 표시
                        }
                    }
                }
            }
        }
        room.SetXYData();      // room 클래스에 최소 좌표값 최대 좌표값을 동기화 한다.
        return room;
    }

    /// <summary>
    /// CheckNearNodesBool를 모든 노드를 실행 시키는 함수
    /// </summary>
    void GatherData()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodes[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
            }
        }
    }

    /// <summary>
    /// 해당 좌표값의 주변 노드들의 data(bool)을 확인해서 자신의 값을 주변 bool 값 비율에 따라 수정하는 메서드
    /// </summary>
    /// <param name="x">x 좌표값</param>
    /// <param name="y">y 좌표값</param>
    /// <returns>주변 노드가 true가 절반 이상이면 true, 아니면 false</returns>
    bool CheckNearNodesBool(int x, int y)
    {
        int count = 0;
        int boolTCount = 0;
        for(int a = -1; a <= 1; a++)
        {
            for(int b = -1; b <= 1; b++)
            {
                if(CheckInMap(x + b, y + a))    // 맵 바깥일 때 false로 처리
                {
                    // 근처 타겟 노드가 true 일때 ture 카운트 증가
                    if (nodes[GetIndex(x + b, y + a)].data) boolTCount++;
                }
                count++;
            }
        }
        //boolTCount가 count의 절반보다 크면 true, 작으면 false
        return (boolTCount - (count * 0.5f)) > 0f;
    }


    /// <summary>
    /// mapFillRate 비율에 따라 가로(Width) 세로(Height)크기 만큼 빈칸 혹은 검은칸을 생성해서 nodes리스트에 저장하는 메서드
    /// </summary>
    void ResetMap()
    {   
        nodes = new Node[Width * Height];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node(
                (Random.Range(0.0f, 1.0f) < mapFillRate),       // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
                new Vector2Int(i % Width, i / Width));          // 노드 위치는 
        }
    }

    /// <summary>
    /// 맵 안쪽의 좌표인지 구분하는 메서드
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns></returns>
    bool CheckInMap(int x, int y)
    {
        return !((x < 0 || x >= width) || (y < 0 || y >= height));
    }

    /// <summary>
    /// x, y값을 받아 nodes의 index를 반환하는 메서드
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns></returns>
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
        foreach(Room room in roomlist)
        {
            Debug.Log($"{i}번째 {room.nodes.Count}개 방");
        }
    }



    private void OnDrawGizmos()
    {
        if (nodes != null && nodes.Length > 0 && roomlist.Count > 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    //Gizmos.color = Color.white;

                    //foreach(Room room in roomlist)
                    //{
                    //    //Vector3[] tempsVec = new Vector3[4];
                    //    //tempsVec[0] = new Vector3(room.minX, room.minY);
                    //    //tempsVec[1] = new Vector3(room.minX, room.maxY);
                    //    //tempsVec[2] = new Vector3(room.maxX, room.maxY);
                    //    //tempsVec[3] = new Vector3(room.maxX, room.minY);
                    //    //Handles.DrawLines(tempsVec);
                    //    //Gizmos.DrawLine(new Vector3(room.minX, room.minY), new Vector3(room.minX, room.maxY));
                    //    //Gizmos.DrawLine(new Vector3(room.minX, room.maxY), new Vector3(room.maxX, room.maxY));
                    //    //Gizmos.DrawLine(new Vector3(room.maxX, room.maxY), new Vector3(room.maxX, room.minY));
                    //    //Gizmos.DrawLine(new Vector3(room.maxX, room.minY), new Vector3(room.minX, room.minY));
                    //    Gizmos.DrawCube(new Vector3((room.minX + room.maxX) * 0.5f, (room.minY + room.maxY) * 0.5f), new Vector3(room.maxX - room.minX, room.maxY - room.minY));
                    //}

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
