using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public enum ExitDirection
{
    None = -1,
    Up = 0,
    Left,
    Right,
    Down
}

class NodeMap
{
    class Node
    {
        public bool data;
        public bool isChecked;
        public Vector2Int gridPos;
        public Node(bool data, Vector2Int pos, bool isChecked = false)
        {
            this.data = data;
            this.isChecked = isChecked;
            gridPos = pos;
        }
    }

    Node[] mapNode;
    List<List<Node>> nodesList;

    int width;
    int height;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="w">가로길이</param>
    /// <param name="h">세로길이</param>
    public NodeMap(int w, int h)
    {
        width = w;
        height = h;
        mapNode = new Node[w * h];
    }

    /// <summary>
    /// 맵을 랜덤하게 채우는 함수
    /// </summary>
    /// <param name="fillRate">채우는 비율</param>
    public void RandomFillNodeInMap(float fillRate)
    {
        for (int i = 0; i < mapNode.Length; i++)
        {
            mapNode[i] = new Node(
                (Random.Range(0.0f, 1.0f) < fillRate),       // fiilrate보다 작으면 true(빈칸) 아니면 false(검은칸)
                new Vector2Int(i % width, i / height));          // 노드 위치는 
        }
    }

    /// <summary>
    /// 노드들을 뭉치게 만드는 함수
    /// </summary>
    /// <param name="count">작업 실행할 횟수</param>
    public void GatherData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapNode[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
                }
            }
        }
    }

    /// <summary>
    /// 노드 근처의 데이터 량에 따라 자신 노드의 값이 결정되게 하는 함수
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns>절반이상이 참이면 참, 아니면 거짓</returns>
    bool CheckNearNodesBool(int x, int y)
    {
        int count = 0;
        int boolTCount = 0;
        for (int a = -1; a <= 1; a++)
        {
            for (int b = -1; b <= 1; b++)
            {
                if (CheckInMap(x + b, y + a))    // 맵 바깥일 때 false로 처리
                {
                    // 근처 타겟 노드가 true 일때 ture 카운트 증가
                    if (mapNode[GetIndex(x + b, y + a)].data) boolTCount++;
                }
                count++;
            }
        }
        //boolTCount가 count의 절반보다 크면 true, 작으면 false
        return (boolTCount - (count * 0.5f)) > 0f;
    }

    /// <summary>
    /// 방을 구성하는 노드 리스트 만들기
    /// </summary>
    public void GetNodesList()
    {
        nodesList = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);

                if (nodeList != null)       // 정상적으로 검사를 마치면
                {
                    nodesList.Add(nodeList);     // 리스트에 추가
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
    List<Node> NearNodeList(int x, int y)
    {
        // 해당 노드가 이미 체크가 되거나 체크안하는 노드일 경우
        if (mapNode[GetIndex(x, y)].isChecked || !mapNode[GetIndex(x, y)].data) return null;

        Stack<Node> stack = new();
        List<Node> list = new List<Node>();

        stack.Push(mapNode[GetIndex(x, y)]);                  // 노드를 스택에 넣는다(스택 1)
        mapNode[GetIndex(x, y)].isChecked = true;             // 확인 함을 표시

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
                        Node tempTarget = mapNode[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
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
    /// 제한된 방개수보다 많으면 작은 방부터 삭제하기
    /// </summary>
    /// <param name="listCount"></param>
    public void LimitRoomCount(int listCount)
    {
        if (listCount < nodesList.Count)
        {
            nodesList.Sort((x, y) => x.Count > y.Count ? -1 : 1);        // 큰 방부터 작은 방으로 정렬

            for (int i = 0; i < nodesList.Count - listCount; i++)                     // roomCount 만큼 개수를 제외하고 작은 방부터 제거
            {
                foreach (Node node in nodesList[(nodesList.Count - 1) - i])
                {
                    node.data = false;
                }
            }
            nodesList.RemoveRange((int)listCount, (int)(nodesList.Count - listCount));
        }
    }
    
    /// <summary>
    /// 모든 노드들의 리스트에 대해서 center좌표들을 반환하는 함수
    /// </summary>
    /// <returns>Center좌표값 리스트</returns>
    public List<Vector2Int> GetRoomCoord()
    {
        List<Vector2Int> list = new();
        foreach(List<Node> temp in nodesList)
        {
            list.Add(GetCenterCoord(temp));
        }
        return list;
    }

    // 자잘한 기능 부분===============================================
    /// <summary>
    /// center 좌표값을 받아오는 함수
    /// </summary>
    /// <param name="nodes">center 좌표를 알고 싶은 노드들</param>
    /// <returns></returns>
    Vector2Int GetCenterCoord(List<Node> nodes)
    {
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        
        foreach(Node node in nodes)
        {
            if (node.gridPos.x < minX)
            {
                minX = node.gridPos.x;
            }
            else if (node.gridPos.x > maxX)
            {
                maxX = node.gridPos.x;
            }

            if (node.gridPos.y < minY)
            {
                minY = node.gridPos.y;
            }
            else if (node.gridPos.y > maxY)
            {
                maxY = node.gridPos.y;
            }
        }

        int centerX = (int)((maxX + minX) * 0.5f);
        int centerY = (int)((maxY + minY) * 0.5f);

        return new Vector2Int(centerX, centerY);
    }


    /// <summary>
    /// 지정된 맵 안쪽인지 확인하는 부분
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns></returns>
    bool CheckInMap(int x, int y)
    {
        return !((x < 0 || x >= width) || (y < 0 || y >= height));
    }

    /// <summary>
    /// x,y값으로 맵 배열 인덱스 찾기
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns>index</returns>
    int GetIndex(int x, int y)
    {
        return x + y * width;
    }
}


public class RoomMap
{
    public List<Room> roomList;

    public RoomMap(List<Vector2Int> map)
    {
        roomList = GetRoomList(map);
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

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB) continue;   // A와 B가 같은 방이면 스킵(자기자신)

                float distance = Mathf.Pow(roomA.roomCoord.x - roomB.roomCoord.x, 2) + Mathf.Pow(roomA.roomCoord.y - roomB.roomCoord.y, 2);

                if (distance < nearDistance)         // 두 방이 현재 기록되어 있는 최단 거리보다 작으면
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
            else if (isPossible && !onceChecked)                 // 두 방이 연결 가능하면 연결하기, 처음 모든 방을 비교할 때
            {
                Room.ConnectRooms(tempA, tempB);
            }
        }

        if (isPossible && onceChecked)           // 서로 다른 Room리스트에서 가까운 방을 연결하기
        {
            Room.ConnectRooms(tempA, tempB);
        }

        bool isAllRoomConnectedWithMainRoom = true;     // 모든 방이 연결되어 있는지 여부 확인
        foreach (Room temp in allRooms)
        {
            if (!temp.isAccessibleMainRoom)             // 하나라도 연결 안되어 있으면 false
            {
                isAllRoomConnectedWithMainRoom = false;
            }
        }

        if (!isAllRoomConnectedWithMainRoom)           // 하나라도 연결이 안되어 있으면
        {
            ConnectNearRoom(allRooms, true);            // 함수 재실행(내부적으로 연결되어 있는거 안되어 있는거 검사함)
        }
    }

    /// <summary>
    /// 노드맵으로 만든 위치 리스트를 Room리스트로 전환
    /// </summary>
    /// <param name="map">노드맵</param>
    /// <returns>RoomList</returns>
    List<Room> GetRoomList(List<Vector2Int> map)
    {
        List<Room> result = new List<Room>();
        foreach (var roomCoord in map)
        {
            result.Add(new Room(roomCoord));
        }

        return result;
    }

    /// <summary>
    /// X 좌표 기준으로 정렬
    /// </summary>
    /// <param name="roomList">정렬할 리스트</param>
    public void SortByCoordX()
    {
        roomList.Sort((a, b) => a.roomCoord.x < b.roomCoord.x ? -1 : 1);      // 가장 왼쪽부터 오른쪽으로 정렬(아직 방 연결 안됨)

        roomList[0].isMainRoom = true;                          // 가장 왼쪽에 있는 방을 메인 방으로 설정(시작 방)
        roomList[0].isAccessibleMainRoom = true;
    }


}

public class Room
{
    /// <summary>
    /// 연결되어 있는 방과 해당방의 방향
    /// </summary>
    public List<(Room, ExitDirection)> connectedRooms;

    public bool isMainRoom = false;
    public bool isAccessibleMainRoom = false;

    public Vector2Int roomCoord;

    public Room() { }

    public Room(Vector2Int coord)
    {
        roomCoord = coord;
        connectedRooms = new();
    }

    public void SetAccessibleMainRoom()
    {
        isAccessibleMainRoom = true;            // 해당 방을 true로 설정

        for(int i = 0; i < connectedRooms.Count; i++)       // 연결되어 있는 방들에 대해서 검사
        {
            Room room = connectedRooms[i].Item1;
            if (!room.isAccessibleMainRoom)             // 연결되어 있는 방들 중 ture가 아닌 방이 있으면
            {
                room.SetAccessibleMainRoom();           // 해당 방과 연결되어 있는 방들도 메인룸과 연결 가능으로 바꿈
            }
        }
    }

    /// <summary>
    /// 이방이 target과 직접 연결(바로 옆) 되어 있는지 확인하는 함수
    /// </summary>
    /// <param name="targetRoom">확인할 방</param>
    /// <returns>참이면 연결 되어 있음, 거짓이면 연결 안되어 있음</returns>
    public bool IsConnected(Room targetRoom)
    {
        foreach((Room, ExitDirection) room in connectedRooms)
        {
            if(room.Equals(targetRoom))
            {
                return true;
            }
        }
        return false;
    }

    public static void ConnectRooms(Room room1, Room room2)
    {
        // 연결할 때 둘 중 하나가 메인 룸과 연결이 되어 있으면 둘다 연결 설정
        if (room1.isAccessibleMainRoom)
        {
            room2.SetAccessibleMainRoom();
        }
        else if (room2.isAccessibleMainRoom)
        {
            room1.SetAccessibleMainRoom();
        }

        room1.SetExitDirAndAddConnectList(room2);
        room2.SetExitDirAndAddConnectList(room1);
    }

    void SetExitDirAndAddConnectList(Room targetRoom)
    {
        ExitDirection result = ExitDirection.None;

        // target이 현재 룸보다 위에 있을 때
        if(targetRoom.roomCoord.y > roomCoord.y)
        {
            // target이 현재 룸 기준으로 X축보다 Y축 쪽으로 올라가 있을 때
            if (targetRoom.roomCoord.y - roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Up;
            }
            else
            {
                // target이 오른쪽에 있을 때
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }
        else
        {
            // target이 현재 룸 기준으로 X축보다 Y축 쪽으로 내려가 있을 때
            if (roomCoord.y - targetRoom.roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Down;
            }
            else
            {
                // target이 오른쪽에 있을 때
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }

        if (result != ExitDirection.None)
        {
            connectedRooms.Add((targetRoom, result));
        }
        else
        {
            Debug.LogWarning("방 연결이 안됩니다!");
        }
    }
    
}


public class GridMap
{
    public Room[,] mapGrid;
    int width;
    public int Width => width;

    int height;
    public int Height => height;
    public static Vector2Int cursor = Vector2Int.zero;

    public GridMap(Vector2Int size)
    {
        width = size.x; height = size.y;
        mapGrid = new Room[size.x, size.y];
        // 모든 Grid을 null으로 초기화
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                mapGrid[i, j] = null;
            }
        }
        cursor = Vector2Int.zero;
    }

    

    public void MakeGridMap(Room room, ExitDirection exitDir)
    {
        Room tempRoom = room;

        if (cursor.y >= mapGrid.GetLength(1) || cursor.x >= mapGrid.GetLength(0) || cursor.x < 0 || cursor.y < 0)
        {
            int aaa = 0;
        }

        mapGrid[cursor.x, cursor.y] = tempRoom;

        for(int i = 0; i < tempRoom.connectedRooms.Count; i++)
        {
            if (!IsContain(tempRoom.connectedRooms[i].Item1))       // 만약 그리드 맵에 존재하지 않으면
            {
                int a = 100;
                while (a > 0)
                {
                    bool isXDir = false;
                    switch (tempRoom.connectedRooms[i].Item2)           // 방향에 따라 커서 조정한 후
                    {
                        case ExitDirection.Up:
                            cursor.y++;
                            isXDir = false;
                            break;
                        case ExitDirection.Right:
                            cursor.x++;
                            isXDir = true;
                            break;
                        case ExitDirection.Left:
                            if (cursor.x == 0) MoveAllGridCell(false);
                            else cursor.x--;
                            isXDir = true;
                            break;
                        case ExitDirection.Down:
                            if (cursor.y == 0) MoveAllGridCell(true);
                            else cursor.y--;
                            isXDir = false;
                            break;
                        default:
                            Debug.LogWarning("존재하지 않은 연결입니다.");
                            break;
                    }

                    bool canDeploy = true;

                    if (isXDir) // 가로로 가는 중일 때
                    {
                        for (int y = 0; y < mapGrid.GetLength(1); y++)      
                        {
                            if (mapGrid[cursor.x, y] != null)   // 같은 라인에 뭐가 있으면
                            {
                                canDeploy = false;
                                break;
                            }
                        }
                    }
                    else    // 세로로 가는 중일 때
                    {
                        for (int x = 0; x < mapGrid.GetLength(0); x++)
                        {
                            if (mapGrid[x, cursor.y] != null)   // 같은 라인에 뭐가 있으면
                            {
                                canDeploy = false;
                                break;
                            }
                        }
                    }

                    if (canDeploy)
                    {
                        break;
                    }
                    a--;
                }

                MakeGridMap(tempRoom.connectedRooms[i].Item1, tempRoom.connectedRooms[i].Item2);
            }
        }

        switch (exitDir)
        {
            case ExitDirection.Up:
                cursor.y--;
                break;
            case ExitDirection.Left:
                cursor.x++;
                break;
            case ExitDirection.Right:
                cursor.x--;
                break;
            case ExitDirection.Down:
                cursor.y++;
                break;
        }
    }

    public Vector2Int GetRoomGrid(Room room)
    {
        Vector2Int temp = -Vector2Int.one;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapGrid[x, y] != null && mapGrid[x, y].Equals(room))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return temp;
    }


    bool IsContain(Room room)
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(mapGrid[x, y] != null && mapGrid[x, y].Equals(room))
                {
                    return true;
                }
            }
        }
        return false;
    }


    void MoveAllGridCell(bool isY)
    {
        for (int y = mapGrid.GetLength(1) - 2; y >= 0 ; y--)
        {
            for (int x = mapGrid.GetLength(0) - 2; x >= 0 ; x--)
            {
                if (mapGrid[x, y] != null)
                {
                    Room temp = mapGrid[x, y];
                    if (isY)
                    {
                        mapGrid[x, y + 1] = temp;
                        mapGrid[x, y] = null;
                    }
                    else
                    {
                        mapGrid[x + 1, y] = temp;
                        mapGrid[x, y] = null;
                    }
                }
            }
        }
    }
}


public class RandomMapGenerator
{
    /// <summary>
    /// 노드맵(방 리스트 만들 때까지만 사용)
    /// </summary>
    NodeMap nodeMap;

    /// <summary>
    /// 룸맵
    /// </summary>
    RoomMap roomMap;

    public GridMap gridMap;

    public List<Room> roomList;


    public void SetUp(int roomCount = 8, int width = 100, int height = 100, float mapFillRate = 0.46f, int collectNodeCount = 3)
    {
        nodeMap = new NodeMap(width, height);

        nodeMap.RandomFillNodeInMap(mapFillRate);           // 맵을 랜덤하게 뿌리고

        nodeMap.GatherData(collectNodeCount);               // 노드들을 집약화 시키고

        nodeMap.GetNodesList();                             // 생성된 노드들의 리스트를 만들고

        nodeMap.LimitRoomCount(roomCount);                  // 제한된 방 개수로 줄임(리스트 큰방 -> 작은방 순으로 정렬)

        roomMap = new RoomMap(nodeMap.GetRoomCoord());      // nodemap을 전환

        roomMap.SortByCoordX();                             // Room을 X좌표 기준으로 재정렬 및 메인룸 설정

        roomMap.ConnectNearRoom(roomMap.roomList);                          // 방 연결하기

        roomList = roomMap.roomList;        // 테스트용 임시

        Vector2Int size = GetGridMapSize(roomMap.roomList);

        gridMap = new GridMap(size);

        Debug.Log($"맵 크기 : {size}");

        gridMap.MakeGridMap(roomMap.roomList[0], ExitDirection.None);


    }


    public Vector2Int GetGridMapSize(List<Room> roomList)
    {
        int x = 0;
        int y = 0;
        foreach (Room room in roomList)
        {
            foreach ((Room, ExitDirection) connectedRoom in room.connectedRooms)
            {
                if (connectedRoom.Item2 == ExitDirection.Left || connectedRoom.Item2 == ExitDirection.Right)
                {
                    x++;
                }
                else if (connectedRoom.Item2 == ExitDirection.Up || connectedRoom.Item2 == ExitDirection.Down)
                {
                    y++;
                }
            }
        }
        return new Vector2Int((int)(x * 0.5f) + 1, (int)(y * 0.5f) + 1);
    }










}
