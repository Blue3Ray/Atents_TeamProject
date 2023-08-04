using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomMap;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Room
{
    public List<Node> nodes = new List<Node>();
    public List<Room> connectedRooms = new List<Room>();
    public List<ExitDirection> connectedExit = new List<ExitDirection>();

    public bool isAccessibleMainRoom;
    public bool isMainRoom;
    public bool isBuilt = false;

    public float CenterX => (maxX + minX) * 0.5f;
    public float CenterY => (maxY + minY) * 0.5f;

    // 방 위치에 따라 출구 위치와 방향을 설정하기 위한 변수
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;

    public void SetXYData()
    {
        minX = minY = int.MaxValue;
        maxX = maxY = -1;
        if (nodes.Count <= 1)
        {
            minX = maxX = nodes[0].gridPos.x;
            minY = maxY = nodes[0].gridPos.y;
        }

        foreach (Node item in nodes)
        {
            if (item.gridPos.x < minX)
            {
                minX = item.gridPos.x;
            }
            else if (item.gridPos.x > maxX)
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

    public void ClearNodes()
    {
        foreach (Node node in nodes)
        {
            node.data = false;
        }
    }

    /// <summary>
    /// MainRoom과 연결시 자신과 연결되어 있는 다른 방들도 연결 속성 부여
    /// </summary>
    public void SetAccessibleMainRoom()
    {
        if (!isAccessibleMainRoom)
        {
            isAccessibleMainRoom = true;
            foreach (Room room in connectedRooms)
            {
                if (!room.isAccessibleMainRoom)
                {
                    room.SetAccessibleMainRoom();           // 해당 방과 연결되어 있는 방들도 메인룸과 연결 가능으로 바꿈
                }
            }
        }
    }

    public static void ConnectRooms(Room rA, Room rB)
    {
        // 연결할 때 둘 중 하나가 메인 룸과 연결이 되어 있으면 둘다 연결 설정
        if (rA.isAccessibleMainRoom)
        {
            rB.SetAccessibleMainRoom();
        }
        else if (rB.isAccessibleMainRoom)
        {
            rA.SetAccessibleMainRoom();
        }

        rA.connectedRooms.Add(rB);
        rB.connectedRooms.Add(rA);
    }

    /// <summary>
    /// target방과 연결 되어 있는지 확인하는 함수(인접만 확인)
    /// </summary>
    /// <param name="targetRoom">확인할 방</param>
    /// <returns>참이면 연결 되어 있음, 거짓이면 연결 안되어 있음</returns>
    public bool IsConnected(Room targetRoom)
    {
        return connectedRooms.Contains(targetRoom);
    }

    public void SetExitList(ref int width, ref int height)
    {
        ExitDirection exitDir = ExitDirection.Up;
        foreach (Room room in connectedRooms)
        {
            if(room.CenterY > CenterY)      // 룸이 위쪽에 있고
            {
                if(room.CenterY - CenterY > Mathf.Abs(room.CenterX - CenterX))     // 룸이 현재 룸 기준으로 X축보다 Y축 쪽으로 올라가 있을 때
                {
                    exitDir = ExitDirection.Up;
                    height++;
                }
                else
                {
                    if (room.CenterX > CenterX)
                    {
                        exitDir = ExitDirection.Right;
                    }
                    else
                    {
                        exitDir = ExitDirection.Left;
                    }
                    width++;
                }
            }
            else                            // 룸이 아래쪽에 있고
            {
                if (CenterY - room.CenterY > Mathf.Abs(room.CenterX - CenterX))     // 룸이 현재 룸 기준으로 Y축이 X축보다 더 아래 쪽에 있을 때
                {
                    exitDir = ExitDirection.Down;
                    height++;
                }
                else
                {
                    if (room.CenterX > CenterX)
                    {
                        exitDir = ExitDirection.Right;
                    }
                    else
                    {
                        exitDir = ExitDirection.Left;
                    }
                    width++;
                }
            }

            connectedExit.Add(exitDir);         // 리스트 순서(연결되어 있는 방과 index가 같아야됨)대로 저장
            Debug.Log($"{exitDir}");
        }
    }
}

// 여기서부터 기능 구현----------------------------------------------------------------------------------------------
// 여기서부터 기능 구현----------------------------------------------------------------------------------------------

public class RandomMap
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

    public int widthCount = 0;
    public int heightCount = 0;

    /// <summary>
    /// 노드들
    /// </summary>
    public Node[] mapNodes;
    
    /// <summary>
    /// 초기 랜덤 bool 채우는 정도 (대충 0.45 ~ 0.47 적당)
    /// </summary>
    [Range(0,1)]
    public float mapFillRate = 0.46f;

    /// <summary>
    /// 집약화 시키는 횟수
    /// </summary>
    public int collectBoxBoolCount = 3;

    /// <summary>
    /// Room(Node의 리스트)들을 가지고 있는 리스트
    /// </summary>
    public List<Room> roomList = new List<Room>();

    /// <summary>
    /// 테스트 용 임시 생성자(어짭히 기본값이 있어서 넣기 귀찮아서 만듬)
    /// </summary>
    public RandomMap()
    {

    }


    public RandomMap(int width, int height, float fillRate = 0.46f, int collecBoxBoolCount = 3)
    {
        Width = width;
        Height = height;
        mapFillRate = fillRate;
        collectBoxBoolCount = collecBoxBoolCount;
    }

    /// <summary>
    /// 테스트용 삭제 예정
    /// </summary>
    public void SetUpNodes()
    {
        RandomFillNodeInMap(Width, Height, mapFillRate);
    
        GatherData(collectBoxBoolCount);

        roomList = new List<Room>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);
                Room tempRoom = new();
                tempRoom.nodes = nodeList;
                tempRoom.SetXYData();      // room 클래스에 최소 좌표값 최대 좌표값을 동기화 한다.

                if (tempRoom != null)       // 정상적으로 검사를 마치면
                {
                    roomList.Add(tempRoom);     // 리스트에 추가
                    //Debug.Log($"room list : {roomList.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                    //    $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
                }
            }
        }
    }

    /// <summary>
    /// RoomList를 roomCount 개수에 맞춰 랜덤하게 생성하는 함수
    /// </summary>
    /// <param name="roomCount">생성할 방 개수</param>
    public void StartMapData(uint roomCount)
    {
        RandomFillNodeInMap(Width, Height, mapFillRate);

        GatherData(collectBoxBoolCount);


        roomList = new List<Room>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);
                

                if (nodeList != null)       // 정상적으로 검사를 마치면
                {
                    Room tempRoom = new();

                    tempRoom.nodes = nodeList;
                    tempRoom.SetXYData();      // room 클래스에 최소 좌표값 최대 좌표값을 동기화 한다.

                    roomList.Add(tempRoom);     // 리스트에 추가
                    //Debug.Log($"room list : {roomList.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                    //    $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
                }
            }
        }

        LimitRoomCount(roomCount);

        roomList.Sort((x, y) => x.CenterX < y.CenterX ? -1 : 1);      // 가장 왼쪽부터 오른쪽으로 정렬

        roomList[0].isMainRoom = true;                          // 가장 왼쪽에 있는 방을 메인 방으로 설정(시작 방)
        roomList[0].isAccessibleMainRoom = true;

        ConnectNearRoom(roomList);

        widthCount = 0;
        heightCount = 0;

        CheckExitDir(roomList);

        //mapGrid = new int[widthCount, heightCount];

        //for(int i = 0; i < widthCount; i++)
        //{
        //    for(int j = 0; j < heightCount; j++)
        //    {
        //        mapGrid[i, j] = -1;
        //    }
        //}

        //Vector2Int grid = Vector2Int.zero;
        //Vector2Int stand = Vector2Int.zero;
        
        //for (int i = 0; i < roomList.Count; i++)
        //{
        //    mapGrid[grid.x + stand.x, grid.y + stand.y] = i;        // 방 index
        //    for (int j = 0; j < roomList[i].connectedExit.Count; j++)       // 방과 연결된 방들을 검사
        //    {
        //        switch (roomList[i].connectedExit[j])       // 연결된 방들을 하나씩 꺼내서
        //        {
        //            case ExitDirection.Up:
        //                grid.x++;
        //                break;
        //            case ExitDirection.Right:
        //                grid.y++;
        //                break;
        //            case ExitDirection.Down:
        //                // 그리드 전체가 올라가야됨
        //                if(grid.y == 0) MoveAllGridCell(true);

        //                break;
        //            case ExitDirection.Left:
        //                // 그리드 전체가 오른쪽으로 이동해야 됨
        //                if (grid.x == 0) MoveAllGridCell(false);

        //                break;
        //        }
        //    }
        //}
        foreach (Room tempRoom in roomList)     // 디버그용
        {
            //Debug.Log($"{tempRoom.connectedRooms.Count}");
            foreach (Room tempBRoom in tempRoom.connectedRooms)
            {
                Debug.DrawLine(new Vector3(tempRoom.CenterX, tempRoom.CenterY), new Vector3(tempBRoom.CenterX, tempBRoom.CenterY), Color.red, 8f);
            }
        }
    }

    int[,] mapGrid;

    void MoveAllGridCell(bool isY)
    {
        for(int y = 0; y < mapGrid.GetLength(0);y++)
        {
            for (int x = 0; x < mapGrid.GetLength(1); x++)
            {
                if (mapGrid[x, y] != -1)
                {
                    int temp = mapGrid[x, y];
                    if (isY)
                    {
                        mapGrid[x, y + 1] = temp;
                        mapGrid[x, y] = -1;
                    }
                    else
                    {
                        mapGrid[x + 1 , y] = temp;
                        mapGrid[x, y] = -1;
                    }
                }
            }
        }
    }



    /// <summary>
    /// 리스트에 있는 방에 대해서 연결되어 있는 방들에 대해 방향을 지정함
    /// </summary>
    /// <param name="rooms">확인할 room 리스트 들</param>
    public void CheckExitDir(List<Room> rooms)
    {
        int i = 0;
        foreach(Room room in rooms)
        {
            Debug.Log($"{i}번째 방");
            room.SetExitList(ref widthCount, ref heightCount);
            i++;
        }
    }

    /// <summary>
    /// 메인룸 부터(0번째는 가장 왼쪽에 있는 메인룸) 가장 가까운 방과 연결하기(거짓이면 일단 근처 1개씩, 참이면 메인룸과 연결확인 후 연결)
    /// </summary>
    public void ConnectNearRoom(List<Room> allRooms, bool onceChecked = false)
    {
        Room tempA = new();
        Room tempB = new();

        List<Room> roomListA = new();
        List<Room> roomListB = new();

        bool isPossible = false;
        float nearDistance = float.MaxValue;

        if (onceChecked)     // 이미 한번 체크 한거 확인
        {
            //foreach (Room temp in allRooms)       // bool isMainRoom으로 설정 했음
            //{
            //    if (mainRoom == null)
            //    {
            //        mainRoom = temp;
            //        continue;
            //    }

            //    if (mainRoom.minX > temp.minX)
            //    {
            //        mainRoom = temp;
            //    }
            //}

            foreach (Room temp in allRooms)      // 모든 방을 검사하면서 
            {
                if (temp.isAccessibleMainRoom)         // 메인 룸이랑 연결 되어 있는지 확인 후 연결 되어 있으면 A리스트 아니면 B리스트
                {
                    roomListA.Add(temp);
                }
                else
                {
                    roomListB.Add(temp);
                }
            }
        }
        else            // 처음 들어온거면 A,B에 모두 넣음
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        foreach (Room roomA in roomListA)
        {
            if (!onceChecked) 
            {
                nearDistance = float.MaxValue;              // 기본값 설정

                if (roomA.connectedRooms.Count > 0)         // 방이 뭔가 연결 되어 있으면
                continue;                                   // 패스하기
            }

            foreach(Room roomB in roomListB)
            {
                if (roomA == roomB) continue;   // A와 B가 같은 방이면 스킵(자기자신)
                
                float distance = Mathf.Pow(roomA.CenterX - roomB.CenterX,2) + Mathf.Pow(roomA.CenterY - roomB.CenterY, 2);

                if(distance < nearDistance)         // 두 방이 현재 기록되어 있는 최단 거리보다 작으면
                {
                    nearDistance = distance;
                    isPossible = true;
                    tempA = roomA;
                    tempB = roomB;
                }
            }

            if (tempA.IsConnected(tempB))       // 가장 가까운 두 방이 이미 서로 연결되어 있으면 넘어가기
            {
                continue;
            }
            else if(isPossible && !onceChecked)                 // 두 방이 연결 가능하면 연결하기, 처음 모든 방을 비교할 때
            {
                Room.ConnectRooms(tempA, tempB);
            }
        }

        if(isPossible && onceChecked)           // 서로 다른 Room리스트에서 가까운 방을 연결하기
        {
            Room.ConnectRooms(tempA, tempB);
        }

        bool isAllRoomConnectedWithMainRoom = true;
        foreach(Room temp in allRooms)
        {
            if(!temp.isAccessibleMainRoom)
            {
                isAllRoomConnectedWithMainRoom = false;
            }
        }

        if(!onceChecked)
        {         
            // 방과 방 연결된 선 그리는 디버그
            foreach (Room tempRoom in allRooms)
            {
                //Debug.Log($"{tempRoom.connectedRooms.Count}");
                foreach (Room tempBRoom in tempRoom.connectedRooms)
                {
                    Debug.DrawLine(new Vector3(tempRoom.CenterX - 1, tempRoom.CenterY - 1), new Vector3(tempBRoom.CenterX - 1, tempBRoom.CenterY - 1), Color.blue, 8f);
                }
            }
        }

        if (!isAllRoomConnectedWithMainRoom)           // 하나라도 연결이 안되어 있으면
        {
            ConnectNearRoom(allRooms, true);
        }
    }

    /// <summary>
    /// 제한된 방 개수로 줄이기
    /// </summary>
    /// <param name="roomCount">제한할 방 개수</param>
    public void LimitRoomCount(uint roomCount)
    {
        if(roomCount < roomList.Count)
        {
            roomList.Sort((x, y) => x.nodes.Count > y.nodes.Count ? -1 : 1);        // 큰 방부터 작은 방으로 정렬

            for (int i = 0; i < roomList.Count - roomCount; i++)                     // roomCount 만큼 개수를 제외하고 작은 방부터 제거
            {
                roomList[(roomList.Count - 1) - i].ClearNodes();
            }
            roomList.RemoveRange((int)roomCount, (int) (roomList.Count - roomCount));
        }

        Debug.Log($" 최종 Room List Count : {roomList.Count}");
    }

    

    /// <summary>
    /// 특정 노드에서 연결되어 있는 노드들을 찾아 방 클래스를 반환하는 함수
    /// </summary>
    /// <param name="x">노드의 X좌표</param>
    /// <param name="y">노드의 Y좌표</param>
    /// <returns>해당 노드가 포함된 Room 클래스(이미 체크한 경우나 검사가 안되는 노드(벽)일 경우 null)</returns>
    List<Node> NearNodeList(int x, int y)
    {
        // 해당 노드가 이미 체크가 되거나 체크안하는 노드일 경우
        if (mapNodes[GetIndex(x, y)].isChecked || !mapNodes[GetIndex(x, y)].data) return null;

        Stack<Node> stack = new();
        List<Node> list = new List<Node>();

        stack.Push(mapNodes[GetIndex(x, y)]);                  // 노드를 스택에 넣는다(스택 1)
        mapNodes[GetIndex(x, y)].isChecked = true;             // 확인 함을 표시

        while (stack.Count > 0)             // 스택에 아무것도 없을 때까지 반복(해당 노드와 연결되어 있는 모든 노드를 검사 하면 끝난다)
        {
            Node target = stack.Pop();      // 스택의 맨위를 꺼낸다
            list.Add(target);
            //room.nodes.Add(target);         // 꺼낸 노드를 리스트에 추가한다.

            // target 노드의 상하좌우 노드를 검사한다.(+모양 위치만, 대각선 제외)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // 맵의 바깥일 경우를 거른다.
                    if (i * j == 0 && CheckInMap(target.gridPos.x + j, target.gridPos.y + i))
                    {
                        Node tempTarget = mapNodes[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
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
        
        return list;
    }

    /// <summary>
    /// CheckNearNodesBool를 모든 노드를 실행 시키는 함수
    /// </summary>
    void GatherData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapNodes[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
                }
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
                    if (mapNodes[GetIndex(x + b, y + a)].data) boolTCount++;
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
    void RandomFillNodeInMap(int mapWidth, int mapHeight, float fillRate)
    {   
        mapNodes = new Node[mapWidth * mapHeight];

        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = new Node(
                (Random.Range(0.0f, 1.0f) < fillRate),       // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
                new Vector2Int(i % mapWidth, i / mapWidth));          // 노드 위치는 
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

    /// <summary>
    /// 메인룸[0]으로부터 연결된 순서대로 정렬하는 함수
    /// </summary>
    /// <param name="sortRoom">하나씩 정렬해서 새로 반환할 리스트</param>
    /// <param name="targetRoom">정렬 할 대상(중복이면 추가 안함)</param>
    public void SortingRoomList(List<Room> sortRoom, Room targetRoom)
    {
        sortRoom.Add(targetRoom);

        foreach (Room room in targetRoom.connectedRooms)
        {
            if (!sortRoom.Contains(room))
            {
                SortingRoomList(sortRoom, room);
            }
        }
    }


#if UNITY_EDITOR
    /// <summary>
    /// 테스트용 리스트 확인 함수
    /// </summary>
    public void PrintRoomlist()
    {
        Debug.Log($"총 {roomList.Count}개 방");
        int i = 0;
        foreach(Room room in roomList)
        {
            Debug.Log($"{i}번째 {room.nodes.Count}개 방");
        }
    }

    public List<Vector3Int> GetRoomPosList()
    {
        List<Vector3Int> result = new();

        foreach(Room room in roomList)
        {
            result.Add(new Vector3Int((int) room.CenterX, (int) room.CenterY));
        }
        return result;  
    }



    //private void OnDrawGizmos()
    //{
    //    if (mapNodes != null && mapNodes.Length > 0 && roomList.Count > 0)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int x = 0; x < width; x++)
    //            {

    //                Color temp = new Color(255, 255, 255, 0.2f);
    //                Gizmos.color = temp;
    //                // false면 검은칸, 스택 확인된거면 빨강, true면 빈칸
    //                if (!mapNodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
    //                else if (mapNodes[GetIndex(x, y)].isChecked)
    //                {
    //                    Gizmos.color = new Color(255,0,0,0.2f);
    //                    Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
    //                }



    //                //Gizmos.color = Color.blue;
    //                //foreach (var roomA in roomList)
    //                //{
    //                //    foreach(Room roomB in roomA.connectedRooms)
    //                //    {
    //                //        Gizmos.DrawCube(new Vector3(roomA.minX, roomA.minY), Vector3.one);
    //                //        Gizmos.DrawCube(new Vector3(roomB.minX, roomB.minY), Vector3.one);
    //                //    }
    //                //}

    //            }
    //        }
    //    }
    //}
#endif
}
