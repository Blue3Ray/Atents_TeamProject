using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum MapLayer
{
    Background = 0,
    PlatForm,
    HalfPlatForm,
    Exit
}

public enum PassWayType
{
    UpDown = 0,
    LeftRight,
    UpRight,
    RightDown,
    DownLeft,
    LeftUp
}




public class RoomGenerator : Singleton<RoomGenerator>
{
    /// <summary>
    /// 배경을 그릴 타일
    /// </summary>
    public Tile backgroundTile;

    /// <summary>
    /// 출구를 구별하기위한 기준 타일
    /// </summary>
    public Tile exitTile;

    /// <summary>
    /// 출구 데이터 불러올 샘플들(0 가로, 1 세로. 2 UR, 3 RD, 4 DL, 5LU)
    /// </summary>
    public SampleRoomData[] passWaySamples;

    /// <summary>
    /// 방 데이터 불러올 샘플들(첫생성할때 0은 항상 시작 방)
    /// </summary>
    public SampleRoomData[] roomSamplesWithExit;


    /// <summary>
    /// 생성된 방들이 쌓여있을 스택
    /// </summary>
    public Stack<SampleRoomData> roomStack;

    /// <summary>
    /// 타일을 그릴 맵들(0 배경, 1 플랫폼, 2 반플랫폼, ... n-1 출입구)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    struct Passes
    {
        public PassWay start;
        public PassWay end;
    }

    // 기능 부분 ------------------
    RandomMapGenerator randomMap;

    SampleRoomData[,] mapGrid;

    // 초기 랜덤 맵 설정 ---------------------
    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public int roomCount = 8;

    // 샘플 맵 설정 ----------------------

    /// <summary>
    /// 샘플 맵의 가장 큰 놈 기준 크기(정사각형 한변의 길이)
    /// </summary>
    int maxSingleRoomSize = 0;

    

    private void Start()
    {
        // 타일을 작성할 레이어 불러오는 과정
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        roomStack = new Stack<SampleRoomData>();

        randomMap = new RandomMapGenerator();

        cursor = new Vector3Int(0, 0);

        foreach(SampleRoomData sampleRoom in roomSamplesWithExit)
        {
            int t = sampleRoom.OnInitialize();
            if (t > maxSingleRoomSize) maxSingleRoomSize = t;
        }
        foreach (SampleRoomData passWay in passWaySamples)
        {
            passWay.OnInitialize();
        }

        maxSingleRoomSize += 5;
        //SetUpRooms();
    }


    public void SetUpRooms()
    {
        randomMap.SetUp(roomCount, width, height, fillRate, collecBoxBoolCount);

        Vector2Int startPos = randomMap.gridMap.GetRoomGrid(randomMap.roomList[0]);

        for(int y = 0; y < randomMap.gridMap.Height; y++)
        {
            for(int x = 0; x < randomMap.gridMap.Width; x++)
            {
                Room target = randomMap.gridMap.mapGrid[x, y];
                if (target != null)
                {
                    //gridMap[x, y] = new NodeRoom();
                }
            }
        }

        // 방을 만드는 부분
        // 방을 만들때 통로 정보를 저장해야된다.
        List<(PassWay, PassWay)> passWays = new List<(PassWay, PassWay)>();

        /*
        randomMap에 roomList가 있다.
        randomMap에 gridMap이 있으며 roomList바탕으로 만들어진 그리드 맵이다.

        List<SampleRoom> roomBuild = new(roomList.Count);
        

        1. 첫 방을 설정한다. / GenerateRoom
            => randomMap.roomList[0]의 출구 조건중 Left하나는 무조건 startRoom과 연결되어야 한다.
            => gridMap의 시작방의 출구 조건이 같거나 이상인 sampleRoom을 선정한다.
                roomBuild.Add(targetRoom);
            
            2. 방과 연결된 방들중 아직 연결 안된 방을 확인한다 
                => randomMap의 해당 Grid드의 Room의 ConnectedRoom의 item2의 isConnected를 확인
                방과 연결된 방향 중 연결이 안된 출구를 하나 골라 저장한다 
                => 선택된 SampleRoom의 exitPos리스트의 해당 isConnected조사한후 랜덤 반환
                => PassWay targetOne

            3. 해당 방을 연결 가능한 방을 선택한다 / GenerateRoom
                => 해당 방이 이미 있으면 그 방을 선택(하지만 그럴리 없음)
                => randomMap의 해당 그리드에 배치 가능한 SampleRoom 선택
                 
            4. 해당 방과 연결할 출입구를 조사한다 
                => 선택된 SampleRoom의 exitPos리스트의 해당 isConnected조사한후 랜덤 반환
                => PassWay targetTwo
            5. 두개 출구 정보를 리스트에 저장한다.passWays.Add(targetOne, targetTwo)

            6. 2번으로 돌아가서 해당방과 연결된 방이 모두 연결 될때까지 반복한다. => 조건은?
                
        7. 그 다음 방으로 넘어간 뒤 모든 방의 연결이 연결 될 때까지 반복한다.

        생성할 때 방이 가지고 있어야 할 정보
        1. 크기 가로세로, 원점으로부터의 거리 등등 레이어별 타일 정보
        2. 출구 정보(출구의 위치 및 방향)
        3. 연결되어 있는 방과 연결 여부

        */

        // for문으로 모든 그리드 검사는 효율적이지 안혹 연결 관계 확인하기도 어려움
        //for (int x = 0; x < randomMap.gridMap.Width; x++)
        //{
        //    for (int y = 0; y < randomMap.gridMap.Height; y++)
        //    {
        //        if (randomMap.gridMap.mapGrid[x, y] != null)
        //        {
        //            GenerateRoom(new Vector3Int((x - startPos.x) * maxSingleRoomSize, (y - startPos.y) * maxSingleRoomSize), roomSamplesWithExit[Random.Range(0, roomSamplesWithExit.Length - 1)]);
        //        }
        //    }
        //}
        //----------------------------



        // 통로 만드는 부분

        int[] dirCount = new int[4];

        for (int i = 0; i < randomMap.gridMap.mapGrid[startPos.x, startPos.y].connectedRooms.Count; i++)
        {
            switch (randomMap.gridMap.mapGrid[startPos.x, startPos.y].connectedRooms[i].Item2)
            {
                case ExitDirection.Up:
                    dirCount[0]++;
                    break;
                case ExitDirection.Left:
                    dirCount[1]++;
                    break;
                case ExitDirection.Right:
                    dirCount[2]++;
                    break;
                case ExitDirection.Down:
                    dirCount[3]++;
                    break;
                default:
                    break;
            }
        }


    }

    /// <summary>
    /// 출구 개수를 만족하는 방을 반환하는 함수
    /// </summary>
    /// <param name="targetRoom">조사할 방</param>
    /// <returns>조건에 맞는 샘플 룸</returns>
    SampleRoomData GetRandomRoom(Room targetRoom)
    {
        //출구 개수와 만족하는 방리스트를 따로 만듬
        List<ExitDirection> upDir = new();
        List<ExitDirection> downDir = new();
        List<ExitDirection> rightDir = new();
        List<ExitDirection> leftDir = new();

        foreach (var item in targetRoom.connectedRooms)
        {
            switch (item.Item2)
            {
                case ExitDirection.Up:
                    upDir.Add(item.Item2);
                    break;
                case ExitDirection.Left:
                    leftDir.Add(item.Item2);
                    break;
                case ExitDirection.Right:
                    rightDir.Add(item.Item2);
                    break;
                case ExitDirection.Down:
                    downDir.Add(item.Item2);
                    break;
            }
        }

        List<SampleRoomData> canBuildRoomList = new();

        foreach (SampleRoomData roomData in roomSamplesWithExit)
        {
            // 샘플에서 각 방향 출구 개수를 비교해서 그 이상인 방들만 걸러냄(구현 가능한 방들만 꺼내기)
            if (roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
                roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
            {
                canBuildRoomList.Add(roomData);
            }

        }
        if (!(canBuildRoomList.Count > 0)) Debug.LogWarning("구현 가능한 방이 없습니다.");

        // 배치 가능한 방들 중 랜덤으로 하나 선택
        return canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];
    }

    //public void SetupRooms()
    //{
    //    // roomSamplesWithExit 방 샘플
    //    // randomMap 랜덤 맵

    //    // roomlist의 두번째는 X값 기준으로 되어 있음. 그러므로 연결된 방 기준으로 다시 정렬
        
    //    randomMap.SortingRoomList(sortList, randomMap.roomList[0]);


    //    List<Vector3Int> roomAnchor = new();        // 방을 생성할 기준(모든 방 프리펩은 0,0이 시작점이여야 함)
    //    List<Vector3Int> roomsSize = new();         // 방의 사이즈들



    //    for(int i = 0; i < sortList.Count; i++)     // 메인룸부터 연결되어 있는 방 순서대로 차례대로
    //    {
    //        // 먼저 어떤 방을 생성할지 선택함

    //        // 출구 개수와 만족하는 방리스트를 따로 만듬
    //        List<ExitDirection> upDir = new();
    //        List<ExitDirection> downDir = new();
    //        List<ExitDirection> rightDir = new();
    //        List<ExitDirection> leftDir = new();
    //        foreach (var dir in sortList[i].connectedExit)
    //        {
    //            switch (dir.Item2)
    //            {
    //                case ExitDirection.Up:
    //                    upDir.Add(dir.Item2);
    //                    break;
    //                case ExitDirection.Left:
    //                    leftDir.Add(dir.Item2);
    //                    break;
    //                case ExitDirection.Right:
    //                    rightDir.Add(dir.Item2);
    //                    break;
    //                case ExitDirection.Down:
    //                    downDir.Add(dir.Item2);
    //                    break;
    //            }   
    //        }

    //        Debug.Log($"{i}번째 방은 UP : {upDir.Count}, Left : {leftDir.Count}, Down : {downDir.Count} ,right : {rightDir.Count}");
    //        List<SampleRoomData> canBuildRoomList = new();

    //        foreach (SampleRoomData roomData in roomSamplesWithExit) 
    //        {
    //            // 샘플에서 각 방향 출구 개수를 비교해서 그 이상인 방들만 걸러냄(구현 가능한 방들만 꺼내기)
    //            if(roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
    //                roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
    //            {
    //                canBuildRoomList.Add(roomData);
    //            }
                
    //        }
    //        if(!(canBuildRoomList.Count > 0)) Debug.LogWarning("구현 가능한 방이 없습니다.");

    //        // 배치 가능한 방들 중 랜덤으로 하나 선택
    //        SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

    //        SetRoomAnchor(cursor, targetRoom, ref roomAnchor, ref roomsSize);       // 첫 방은 0,0에서 시작

    //        Debug.Log($"{i}번째 방 생성 시도");

    //        cursor = new Vector3Int((int) sortList[i].CenterX, (int)sortList[i].CenterY);       // 표시 잘되나 임시로 만든 것

    //        GenerateRoom(targetRoom, cursor);           // 생성
            
    //        // 커서는 다음 위치로 이동(어디로? 어떻게 다시 돌아오지?)
    //    }
    //}

    //void Test(OldRoom room, Vector3Int cursor)
    //{
    //    Vector3Int tempCursor = cursor;

    //    List<ExitDirection> upDir = new();
    //    List<ExitDirection> downDir = new();
    //    List<ExitDirection> rightDir = new();
    //    List<ExitDirection> leftDir = new();
    //    foreach (var dir in room.connectedExit)
    //    {
    //        switch (dir.Item2)
    //        {
    //            case ExitDirection.Up:
    //                upDir.Add(dir.Item2);
    //                break;
    //            case ExitDirection.Left:
    //                leftDir.Add(dir.Item2);
    //                break;
    //            case ExitDirection.Right:
    //                rightDir.Add(dir.Item2);
    //                break;
    //            case ExitDirection.Down:
    //                downDir.Add(dir.Item2);
    //                break;
    //        }
    //    }

        
    //    List<SampleRoomData> canBuildRoomList = new();

    //    foreach (SampleRoomData roomData in roomSamplesWithExit)
    //    {
    //        // 샘플에서 각 방향 출구 개수를 비교해서 그 이상인 방들만 걸러냄(구현 가능한 방들만 꺼내기)
    //        if (roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
    //            roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
    //        {
    //            canBuildRoomList.Add(roomData);
    //        }

    //    }
    //    if (!(canBuildRoomList.Count > 0)) Debug.LogWarning("구현 가능한 방이 없습니다.");

    //    SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

    //    if (CheckBuildable(tempCursor, targetRoom))
    //    {
    //        GenerateRoom(targetRoom, tempCursor);           // 생성
    //    }
    //    else
    //    {
    //        Debug.Log("맵 생성이 안됩니다.");
    //    }
    //    room.isBuilt = true;

    //    for (int j = 0; j < room.connectedRooms.Count; j++)
    //    {
    //        switch (room.connectedExit[j].Item2)
    //        {
    //            case ExitDirection.Up:
    //                tempCursor.y += maxSingleRoomSize;
    //                break;
    //            case ExitDirection.Left:
    //                tempCursor.x -= maxSingleRoomSize;
    //                break;
    //            case ExitDirection.Right:
    //                tempCursor.x += maxSingleRoomSize;
    //                break;
    //            case ExitDirection.Down:
    //                tempCursor.y -= maxSingleRoomSize;
    //                break;
    //        }
    //        if(!room.connectedRooms[j].isBuilt) Test(room.connectedRooms[j], tempCursor);
    //    }
    //}


    //// 첫번째 방을 선택한 후에 연결된 자식들 대상으로 다시 실행하기
    //void SetRoomAnchor(Vector3Int cursor, SampleRoomData roomData, ref List<Vector3Int> anchors, ref List<Vector3Int> sizes)
    //{
    //    anchors.Add(cursor);
    //    sizes.Add(roomData.max);
    //}

    ///// <summary>
    ///// 커서 위치로부터 그리려는 roomData가 그려질수 있는지 체크하는 함수
    ///// </summary>
    ///// <param name="cursor">방의 기준점이 될 위치</param>
    ///// <param name="roomData">그릴 방</param>
    ///// <returns>참이면 해당 범위에 다른 방과 인접한 사항 없음, 거짓이면 다른 방과 곂침</returns>
    //bool CheckBuildable(Vector3Int cursor, SampleRoomData roomData)
    //{
    //    bool result = true;

    //    for(int y = 0; y < roomData.max.y; y++)
    //    {
    //        for(int x = 0; x  < roomData.max.x; x++)
    //        {
    //            if (m_tileMaps[(int) MapLayer.PlatForm].HasTile(cursor + new Vector3Int(x, y)))
    //            {
    //                result = false; break;
    //            }
    //        }
    //        if (!result) break;
    //    }

    //    return result;
    //}

    /// <summary>
    /// 두 위치와 방향을 가지고 통로를 제작하는 함수
    /// </summary>
    /// <param name="startPos">시작 지점</param>
    /// <param name="endPos">종료 지점</param>
    public void GeneratePassway(PassWay startPos, PassWay endPos)
    {
        // 출구쪽 한단계 빼기 위한 단계
        PassWay endPosByOne = endPos;
        //PassWayType pwt = PassWayType.LeftRight;
        //if (endPosByOne.Direction == ExitDirection.Up || endPosByOne.Direction == ExitDirection.Down) pwt = PassWayType.UpDown;
        //endPosByOne.Pos += GeneratePass(new PassWay(endPosByOne.Pos, endPosByOne.Direction), pwt);

        cursor = startPos.Pos;

        Queue<Vector3Int> wayPoints = new ();

        // 만약 두 출구가 같은 선상에 있는 경우가 아니면(대각으로 움직여야하면) 중간 지점에서 S자 꺽기위한 지점
        Vector3Int halfPos = new Vector3Int((int)((startPos.Pos.x + endPos.Pos.x) * 0.5f), (int)((startPos.Pos.y + endPos.Pos.y) * 0.5f));

        // 비트플래그로 했어야 했는데...
        // 입구 출구 방향이 같을 때(수직이거나 수평일때)
        if((startPos.Direction == ExitDirection.Up && endPos.Direction == ExitDirection.Down) ||
            (startPos.Direction == ExitDirection.Down && endPos.Direction == ExitDirection.Up) ||
            (startPos.Direction == ExitDirection.Right && endPos.Direction == ExitDirection.Left) ||
            (startPos.Direction == ExitDirection.Left && endPos.Direction == ExitDirection.Right))
        {
            if (startPos.Pos.x == endPos.Pos.x || startPos.Pos.y == endPos.Pos.y)
            {
                // 일자로 감
                wayPoints.Enqueue(endPos.Pos);
            }
            else
            {
                //s자로 감
                if(startPos.Direction == ExitDirection.Up || startPos.Direction == ExitDirection.Down)
                {
                    wayPoints.Enqueue(new Vector3Int(startPos.Pos.x, halfPos.y));
                    wayPoints.Enqueue(new Vector3Int(endPos.Pos.x, halfPos.y));
                    //wayPoints.Add(new Vector3Int(startPos.Pos.x, halfPos.y));
                    //wayPoints.Add(new Vector3Int(endPos.Pos.x, halfPos.y));
                }
                else
                {
                    wayPoints.Enqueue(new Vector3Int(halfPos.x, startPos.Pos.y));
                    wayPoints.Enqueue(new Vector3Int(halfPos.x, endPos.Pos.y));
                    //wayPoints.Add(new Vector3Int(halfPos.x, startPos.Pos.y));
                    //wayPoints.Add(new Vector3Int(halfPos.x, endPos.Pos.y));
                }
            }
        }
        else
        {
            // 입구 출구가 직각 일때
            if (startPos.Direction == ExitDirection.Up || startPos.Direction == ExitDirection.Down)
            {
                wayPoints.Enqueue(new Vector3Int(startPos.Pos.x, endPos.Pos.y));
                //wayPoints.Add(new Vector3Int(startPos.Pos.x, halfPos.y));
            }
            else
            {
                wayPoints.Enqueue(new Vector3Int(endPos.Pos.x, startPos.Pos.y));
                //wayPoints.Add(new Vector3Int(halfPos.x, startPos.Pos.y));
            }
        }


        Vector3Int targetPos = wayPoints.Dequeue();

        ExitDirection targetDir = startPos.Direction;

        PassWayType lastone = PassWayType.UpDown;

        int a = 0;      // 무한루프 방지용
        do
        {
            if (cursor.x == targetPos.x)        // 직선 길일 때
            {
                int decreasePass = Mathf.Min(Mathf.Abs(cursor.y - targetPos.y), 5);
                lastone = PassWayType.UpDown;
                cursor += GeneratePass(new PassWay(cursor, targetDir), PassWayType.UpDown, 5 - decreasePass);
            }
            else
            {
                int decreasePass = Mathf.Min(Mathf.Abs(cursor.x - targetPos.x), 5);
                lastone = PassWayType.LeftRight;
                cursor += GeneratePass(new PassWay(cursor, targetDir), PassWayType.LeftRight, 5 - decreasePass);
            }
            a++;

            if (cursor == targetPos && targetPos != endPos.Pos)      // 중간 지점에 왔으면
            {
                if (wayPoints.Count > 0) targetPos = wayPoints.Dequeue();     // 최종 목적지 설정 후
                else targetPos = endPos.Pos;
                // 방향에 따라 교차로 생성 코드
                // targetDir와 cursor 위치와 targetPos으로 구할수 있을거 같음

                PassWayType passWay = PassWayType.UpDown;
                switch (targetDir)
                {
                    case ExitDirection.Up:
                        if (targetPos.x > cursor.x)
                        {
                            passWay = PassWayType.RightDown;
                            lastone = PassWayType.LeftRight;
                        }
                        else
                        {
                            passWay = PassWayType.DownLeft;
                            lastone = PassWayType.UpDown;
                        }
                        break;
                    case ExitDirection.Left:
                        if (targetPos.y > cursor.y)
                        {
                            passWay = PassWayType.UpRight;
                            lastone = PassWayType.UpDown;
                        }
                        else
                        {
                            passWay = PassWayType.RightDown;
                            lastone = PassWayType.LeftRight;
                        }
                        break;
                    case ExitDirection.Right:
                        if (targetPos.y > cursor.y)
                        {
                            passWay = PassWayType.LeftUp;
                            lastone = PassWayType.UpDown;
                        }
                        else
                        {
                            passWay = PassWayType.DownLeft;
                            lastone = PassWayType.LeftRight;
                        }
                        break;
                    case ExitDirection.Down:
                        if (targetPos.x > cursor.x)
                        {
                            passWay = PassWayType.UpRight;
                            lastone = PassWayType.LeftRight;
                        }
                        else
                        {
                            passWay = PassWayType.LeftUp;
                            lastone = PassWayType.UpDown;
                        }
                        break;
                }

                cursor += GeneratePass(new PassWay(cursor, targetDir), passWay);        // 타일맵 생성


                if (targetPos.x != cursor.x)
                {
                    if (targetPos.x > cursor.x)
                    {
                        targetDir = ExitDirection.Right;
                    }
                    else
                    {
                        targetDir = ExitDirection.Left;
                    }
                }
                else
                {
                    if (targetPos.y > cursor.y)
                    {
                        targetDir = ExitDirection.Up;
                    }
                    else
                    {
                        targetDir = ExitDirection.Down;
                    }
                }
            }

        } while (cursor != endPos.Pos && a < 100);

        GeneratePass(new PassWay(cursor, targetDir), lastone);        // 마지막 통로 타일맵 생성


        //cursor += GeneratePass(new Exit(cursor, ExitDirection.Right), PassWayType.LeftRight);
    }

    bool IsBuildable(Vector3Int cursor, SampleRoomData targetData)
    {
        bool result = true;
        for(int x = targetData.min.x; x < targetData.max.x; x++ )
        {
            for(int y = targetData.min.y; y < targetData.max.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y) + cursor;
                if (m_tileMaps[1].GetTile(targetPos) != null)
                {
                    result = false; 
                    return result;
                }
            }
        }
        return result;
    }

    bool IsXDir(Vector3Int posA, Vector3Int posB)
    {
        bool result;
        if(Mathf.Abs(posA.x - posB.x) > Mathf.Abs(posA.y - posB.y))
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    Vector3Int GeneratePass(PassWay passPos, PassWayType passWayType, int drawOverCount = 0)
    {
        Vector3Int cursorPos = passPos.Pos;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        // 그려야 할 방향이 방 크기보다 작으면(passWaytype이 0 또는 1인경우만)
        // 그 방향의 작은 만큼 높이 또는 너비를 줄여서 그리기
        Vector3Int decreaseFromMin = Vector3Int.zero;
        Vector3Int decreaseFromMax = Vector3Int.zero;

        if(passWayType == PassWayType.UpDown)
        {
            if(passPos.Direction == ExitDirection.Up) 
            {
                decreaseFromMax.y = drawOverCount;
            }
            else
            {
                decreaseFromMin.y = drawOverCount;
            }
        }
        else if(passWayType == PassWayType.LeftRight)
        {
            if(passPos.Direction == ExitDirection.Right)
            {
                decreaseFromMax.x = drawOverCount;
            }
            else
            {
                decreaseFromMin.x = drawOverCount;
            }
        }


        for (int i = decreaseFromMin.y; i < targetData.Height - decreaseFromMax.y; i++)    // 문 높이 만큼
        {
            for (int j = decreaseFromMin.x; j < targetData.Width - decreaseFromMax.x; j++)  // 문 너비 만큼
            {
                // ******** 배경있는 곳이랑 없는곳이랑 차이 둬서 맵을 만들어야하는 알고리즘짜야됨 다시 고려할 것
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;        // 샘플 위치 까지 고려한 그릴 위치

                // 레이어 설정
                int targetLayer = (int) MapLayer.Background;

                // 배경 그리기
                m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[targetLayer].GetTile(targetDrawPos));

                TileBase tempTile;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    if (!m_tileMaps[(int)MapLayer.HalfPlatForm].HasTile(cursorPos + targetDrawPos))
                    {
                        // 플랫폼 칸일 때(반플렛폼이 있으면 안함)
                        targetLayer = (int)MapLayer.PlatForm;
                        tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                        m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    }
                    else
                    {
                        Debug.Log("반플랫폼 타일이 있음");
                    }
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // 반플랫폼 칸일 때 기존에 플랫폼 칸 있으면 지움
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);

                    if (m_tileMaps[(int)MapLayer.PlatForm].HasTile(targetDrawPos))
                    {
                        m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, null);
                    }

                }
                else
                {
                    // 빈칸일 때 기존의 곂치는 플렛폼과 반플렛폼 지우기
                    tempTile = null;
                    targetLayer = (int)MapLayer.PlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                }

                // 플랫폼 그리기
                // m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
            }
        }

        // 다음 커서 위치를 계산하기위해 움직여야 할 위치 값을 반환하는 변수
        Vector3Int result = Vector3Int.zero;

        switch (passWayType)
        {
            case PassWayType.UpDown:
                if (passPos.Direction == ExitDirection.Up)
                {
                    result.y = targetData.Height;
                }
                else
                {
                    result.y = -targetData.Height;
                }
                break;
            case PassWayType.LeftRight:
                if (passPos.Direction == ExitDirection.Right) result.x = targetData.Width;
                else result.x = -targetData.Width;
                break;
            case PassWayType.UpRight:
                if (passPos.Direction == ExitDirection.Down) result.x = targetData.Width;
                else result.y = targetData.Height;
                break;
            case PassWayType.RightDown:
                if (passPos.Direction == ExitDirection.Left) result.y = -targetData.Height;
                else result.x = targetData.Width;
                break;
            case PassWayType.DownLeft:
                if (passPos.Direction == ExitDirection.Right) result.y = -targetData.Height;
                else result.x = -targetData.Width;
                break;
            case PassWayType.LeftUp:
                if (passPos.Direction == ExitDirection.Right) result.y = targetData.Height;
                else result.x = -targetData.Width;
                break;
        }

        // 통로를 짧게 그린만큼 다음 그릴 지점도 위치 반영
        //decreaseFromMin;
        result -= decreaseFromMax;
        //decreaseFromMax;
        result += decreaseFromMin;

        return result;
    }

    /// <summary>
    /// 맵을 레이어별로 생성하는 메서드
    /// </summary>
    /// <param name="targetRoomData">맵 생성할 샘플 데이터</param>
    /// <param name="index">샘플에서 생성할 레이어</param>
    void GenerateRoom(Vector3Int cursor, SampleRoomData targetRoomData)
    {
        //foreach (List<Vector3Int> poses in targetRoomData.tilesPos)
        for(int i = 0; i < targetRoomData.tilesPos.Count - 1; i++)          // 레이어 별로 나눔(마지막꺼는 출구 레이어라 표시안함
        {
            List<Vector3Int> poses = targetRoomData.tilesPos[i];
            foreach (Vector3Int pos in poses)                       // 레이어에 있는 타일들
            {
                m_tileMaps[i].SetTile(pos + cursor, targetRoomData.mapLayers[i].GetTile(pos));
            }
        }
    }



    void GenerateExit(SampleRoomData targetRoomData, ExitDirection exitDir)
    {
        foreach(PassWay temp in targetRoomData.exitPos)
        {
            if (temp.Direction != exitDir) continue;            // 만약 선택한 출입구가 받은 파라미터 방향과 같지 않으면 스킵
            int x = 0, y = 0;   // 방향이 좌우에 따라 출입구 그려지는 시작위치
            int index = 0;      // 0은 좌우 출입구, 1은 상하 출입구
            if(temp.Direction == ExitDirection.Left || temp.Direction == ExitDirection.Right)
            {
                y = -2;
            }
            else if(temp.Direction == ExitDirection.Up || temp.Direction == ExitDirection.Down)
            {
                x = -2;
                index = 1;
            }

            for (int i = 0; i < passWaySamples[index].Height; i++)    // 문 높이 만큼
            {
                for (int j = 0; j < passWaySamples[index].Width; j++)  // 문 너비 만큼
                {
                    if (passWaySamples[index].mapLayers[1].HasTile(new Vector3Int(passWaySamples[index].min.x + j, passWaySamples[index].min.y + i)))
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), targetRoomData.mapLayers[1].GetTile(temp.Pos));
                        //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                    }
                    else       
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // 빈 타일이면 null(빈타일)로 바꾸기
                    }
                }
            }
        }
    }

    private Vector3Int GetRoomGap(int num)
    {
        int x = Random.Range(3, num);
        int y = Random.Range(-num, num);
        return new Vector3Int(x, y);
    }

    public class TestRoom
    {
        int getSampleIndex;
        public int GetSampleIndex
        {
            get => getSampleIndex;
            set
            {
                getSampleIndex = value;
                passWays = RoomGenerator.Ins.roomSamplesWithExit[getSampleIndex].exitPos.ToArray();
            }
        }

        public PassWay[] passWays;

        public Vector2Int gridCoord;

        public Vector3Int origineCoord;

        public void SetOrigineCoord(int roomSize)
        {
            origineCoord = new Vector3Int(gridCoord.x * roomSize, gridCoord.y * roomSize);
            for(int i =0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }
    }

    public void Test_SetOrigine(SampleRoomData data)
    {
        //Vector2Int mapSize = new Vector2Int(roomSamplesWithExit[getSampleIndex]);
    }

    public void Test()
    {
        TestRoom test1 = new TestRoom();
        test1.gridCoord = new Vector2Int(0, 0);
        test1.GetSampleIndex = 0;
        test1.SetOrigineCoord(maxSingleRoomSize);
        
        TestRoom test2 = new TestRoom();
        test2.gridCoord = new Vector2Int(1, 1);
        test2.GetSampleIndex = 1;
        test2.SetOrigineCoord(maxSingleRoomSize);

        TestRoom test3 = new TestRoom();
        test3.gridCoord = new Vector2Int(2, 0);
        test3.GetSampleIndex = 2;
        test3.SetOrigineCoord(maxSingleRoomSize);

        TestRoom test4 = new TestRoom();
        test4.gridCoord = new Vector2Int(2, 1);
        test4.GetSampleIndex = 2;
        test4.SetOrigineCoord(maxSingleRoomSize);

        GenerateRoom(test1.origineCoord, roomSamplesWithExit[test1.GetSampleIndex]);
        GenerateRoom(test2.origineCoord, roomSamplesWithExit[test2.GetSampleIndex]);
        GenerateRoom(test3.origineCoord, roomSamplesWithExit[test3.GetSampleIndex]);
        GenerateRoom(test4.origineCoord, roomSamplesWithExit[test4.GetSampleIndex]);

        Test_ConnectPassway(test1, test2);
        Test_ConnectPassway(test3, test2);
        Test_ConnectPassway(test2, test4);
    }

    public void Test_ConnectPassway(TestRoom temp1, TestRoom temp2)
    {
        float distance = float.MaxValue;
        PassWay one = temp1.passWays[0];
        PassWay two = temp2.passWays[0];

        foreach(var exit1 in temp1.passWays)
        {
            if (exit1.isConnected) continue;    // 이미 연결 되어 있으면 패스
            foreach(var exit2 in temp2.passWays)
            {
                if (exit2.isConnected) continue;
                float tempDistance = Vector3Int.Distance(exit1.Pos + temp1.origineCoord, exit2.Pos + temp2.origineCoord);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    one = exit1;
                    two = exit2;
                }
            }
        }

        GeneratePassway(one, two);
    }
}

