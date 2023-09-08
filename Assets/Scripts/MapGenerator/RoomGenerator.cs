using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;


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
    /// 방 데이터 불러올 샘플들
    /// </summary>
    public SampleRoomData[] roomSamplesWithExit;

    /// <summary>
    /// 시작 방
    /// </summary>
    public SampleRoomData startRoom;

    /// <summary>
    /// 타일을 그릴 맵들(0 배경, 1 플랫폼, 2 반플랫폼, ... n-1 출입구)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    // 기능 부분 ------------------
    /// <summary>
    /// 그리드 맵 생성하는 클래스
    /// </summary>
    RandomMapGenerator randomMap;

    /// <summary>
    /// 그려질 맵 정보를 가지고 있는 리스트
    /// </summary>
    List<MakeRoom> makeRooms;

    // 초기 랜덤 맵 설정 ---------------------
    
    public int width = 100;

    public int height = 100;

    public float fillRate = 0.46f;

    public int collecBoxBoolCount = 3;

    public int roomCount = 8;

    public int roomGap = 10;

    // 샘플 맵 설정 ----------------------

    /// <summary>
    /// 샘플 맵의 가장 큰 놈 기준 크기(정사각형 한변의 길이)
    /// </summary>
    int maxSingleRoomSize = 0;

    // 싱글톤이기 때문에 오브젝트 찾는건 start에서 실행함
    private void Start()
    {
        // 타일을 그릴 레이어 불러오는 과정
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        randomMap = new RandomMapGenerator();

        cursor = new Vector3Int(0, 0);


        // 방 정보 정리하는 구간
        // 시작 방
        startRoom.OnInitialize();

        // 샘플 방
        foreach(SampleRoomData sampleRoom in roomSamplesWithExit)
        {
            int t = sampleRoom.OnInitialize();
            if (t > maxSingleRoomSize) maxSingleRoomSize = t;
        }

        // 통로
        foreach (SampleRoomData passWay in passWaySamples)
        {
            passWay.OnInitialize();
        }

        // 방과 방 사이 거리 추가해서 방 크기의 여유 두기
        maxSingleRoomSize += roomGap;
    }

    /*
    방 생성하는 알고리즘 설정
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

    /// <summary>
    /// 방을 생성하는 함수
    /// </summary>
    public void SetUpRooms()
    {
        randomMap.SetUp(roomCount, width, height, fillRate, collecBoxBoolCount);        // 맵 정보 생성

        makeRooms = new();      // 방 리스트 초기화

        // 시작 방 생성(시작 방은 만들어진 그리드 맵에서 가장 좌측에 있는 방의 좌측에 배치가 된다
        Vector2Int startRoomGrid = randomMap.roomList[0].gridCoord + new Vector2Int(-1, 0);

        MakeRoom startMakeRoom = new MakeRoom();
        startMakeRoom.GetRoomData(startRoomGrid);
        startMakeRoom.GetSampleIndex = -1;
        startMakeRoom.SetOrigineCoord(new Vector3Int(startRoom.Width, startRoom.Height), maxSingleRoomSize);

        GenerateRoom(startMakeRoom.origineCoord, startRoom);

        makeRooms.Add(startMakeRoom);



        // 방 생성하고 리스트에 등록하는 과정
        foreach (var item in randomMap.roomList)
        {
            MakeRoom targetRoom = new MakeRoom();
            targetRoom.GetRoomData(item);
            targetRoom.GetSampleIndex = GetRandomRoom(item);
            targetRoom.SetOrigineCoord(maxSingleRoomSize);

            GenerateRoom(targetRoom.origineCoord, roomSamplesWithExit[targetRoom.GetSampleIndex]);

            makeRooms.Add(targetRoom);
        }


        // 만들어진 방을 통로 연결 여부 확인 후에 통로 생성하는 과정
        foreach (var roomOne in makeRooms)
        {
            Room targetOne = roomOne.roomData;
            foreach (var roomTwo in targetOne.connectedRooms)
            {
                if(!targetOne.IsConnectedBuildRoom(roomTwo.Item1))
                {
                    targetOne.alreadyConnectPassWayRooms.Add(roomTwo.Item1);
                    roomTwo.Item1.alreadyConnectPassWayRooms.Add(targetOne);

                    if(TryGetMakeRoomByCoord(roomTwo.Item1.gridCoord, out MakeRoom result))
                    {
                        ConnectPassway(roomOne, result);
                    }
                }
            }
        }

        // 시작 방과 첫방을 연결함(그리드 맵 정보에 시작방 정보가 없기 때문에 수동으로 따로 해줌)
        ConnectPassway(makeRooms[0], makeRooms[1]);
    }

    /// <summary>
    /// 출구 개수를 만족하는 방을 반환하는 함수
    /// </summary>
    /// <param name="targetRoom">조사할 방</param>
    /// <returns>조건에 맞는 샘플 룸</returns>
    int GetRandomRoom(Room targetRoom)
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
        return Random.Range(0, canBuildRoomList.Count);
    }

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
    }

    /// <summary>
    /// 두개의 방을 연결 할때 사용가능한 출입구를 확인해서 연결하는 함수
    /// </summary>
    /// <param name="temp1">연결할 첫번째 방</param>
    /// <param name="temp2">연결할 두번째 방</param>
    public void ConnectPassway(MakeRoom temp1, MakeRoom temp2)
    {
        float distance = float.MaxValue;
        PassWay one = temp1.passWays[0];
        PassWay two = temp2.passWays[0];

        // 각 출구들을 하나 씩 꺼내서 최단 거리인지 비교
        foreach (var exit1 in temp1.passWays)
        {
            if (exit1.isConnected) continue;    // 이미 연결 되어 있으면 패스
            foreach (var exit2 in temp2.passWays)
            {
                if (exit2.isConnected) continue; // 이미 연결 되어 있으면 패스

                float tempDistance = Vector3Int.Distance(exit1.Pos + temp1.origineCoord, exit2.Pos + temp2.origineCoord);
                if (tempDistance < distance)    // 기존 저장한 거리보다 짧으면 초기화
                {
                    distance = tempDistance;
                    one = exit1;
                    two = exit2;
                }
            }
        }

        GeneratePassway(one, two);  // 통로 생성
        one.isConnected = true;
        two.isConnected = true;
    }

    /// <summary>
    /// 해당 그리드 좌표에 있는 makeRoom 데이터가 있는지 확인하고 반환하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 그리드 좌표</param>
    /// <param name="resultRoom">결과 방</param>
    /// <returns>해당 그리드 좌표에 방 정보가 있으면 참, 없으면 거짓</returns>
    bool TryGetMakeRoomByCoord(Vector2Int gridPos, out MakeRoom resultRoom)
    {
        bool result = false;
        resultRoom = null;

        foreach (var item in makeRooms)
        {
            if (item.gridCoord == gridPos)      // 리스트 중 해당 방 좌표가 찾는 좌표이면
            {
                resultRoom = item;
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 통로를 한개를 그리는 함수
    /// </summary>
    /// <param name="passPos">위치, 방향 정보</param>
    /// <param name="passWayType">그려야 할 방향 정보</param>
    /// <param name="drawOverCount">그려야 할 칸이 통로 크기보다 작을 때 넘치는 줄 수</param>
    /// <returns>통로 한개를 그리고 나서 반환될 다음 위치</returns>
    Vector3Int GeneratePass(PassWay passPos, PassWayType passWayType, int drawOverCount = 0)
    {
        Vector3Int cursorPos = passPos.Pos;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        // 그려야 할 방향이 방 크기보다 작으면(passWaytype이 0 또는 1인경우만)
        // 그 방향의 작은 만큼 높이 또는 너비를 줄여서 그리기
        Vector3Int decreaseFromMin = Vector3Int.zero;
        Vector3Int decreaseFromMax = Vector3Int.zero;

        // 그려야 할 방향에 따라 줄이는 방향 정하기
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

        // 통로 그리는 부분
        for (int i = decreaseFromMin.y; i < targetData.Height - decreaseFromMax.y; i++)    // 문 높이 만큼
        {
            for (int j = decreaseFromMin.x; j < targetData.Width - decreaseFromMax.x; j++)  // 문 너비 만큼
            {
                // ******** 배경있는 곳이랑 없는곳이랑 차이 둬서 맵을 만들어야하는 알고리즘짜야됨 다시 고려할 것
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;        // 샘플 위치 까지 고려한 그릴 위치

                // 레이어 설정
                int targetLayer = (int) MapLayer.Background;

                TileBase tempTile;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    if (!m_tileMaps[(int)MapLayer.HalfPlatForm].HasTile(cursorPos + targetDrawPos) && !m_tileMaps[(int)MapLayer.Background].HasTile(cursorPos + targetDrawPos))
                    {
                        // 플랫폼 칸일 때(반플렛폼이거나 이미 배경 타일이있으면 안함)
                        targetLayer = (int)MapLayer.PlatForm;
                        tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                        m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    }
                    else
                    {
                        //Debug.Log("반플랫폼 타일이 있음");
                    }
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // 반플랫폼 칸일 때 기존에 플랫폼 칸 있으면 지움
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);

                    if (m_tileMaps[(int)MapLayer.PlatForm].HasTile(cursorPos + targetDrawPos))
                    {
                        m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, null);
                    }
                }
                else
                {
                    // 플랫폼도 반플랫폼도 없을 때
                    // 빈칸일 때 기존의 곂치는 플렛폼과 반플렛폼 지우기
                    tempTile = null;
                    targetLayer = (int)MapLayer.PlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                }

                targetLayer = (int)MapLayer.Background;

                // 배경 그리기
                m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[targetLayer].GetTile(targetDrawPos));
            }
        }

        // 다음 커서 위치를 계산하기위해 움직여야 할 위치 값을 반환하는 변수
        Vector3Int result = Vector3Int.zero;

        // 현재 그려진 방향에 따라 다름 위치 설정
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
        result -= decreaseFromMax;
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
        for(int i = 0; i < targetRoomData.tilesPos.Count - 1; i++)          // 레이어 별로 나눔(마지막꺼는 출구 레이어라 표시안함
        {
            List<Vector3Int> poses = targetRoomData.tilesPos[i];
            foreach (Vector3Int pos in poses)                       // 레이어에 있는 타일들
            {
                m_tileMaps[i].SetTile(pos + cursor, targetRoomData.mapLayers[i].GetTile(pos));
            }
        }
    }

    /// <summary>
    /// 그려질 방 정보 클래스
    /// </summary>
    public class MakeRoom
    {
        int getSampleIndex = -1;

        /// <summary>
        /// 방에다가 설치될 샘플 방 인덱스, 설정이 되면 해당 방의 출구 정보를 자동으로 가져온다
        /// </summary>
        public int GetSampleIndex
        {
            get => getSampleIndex;
            set
            {
                getSampleIndex = value;
                if (getSampleIndex != -1)           // 시작 방일 때 -1
                {
                    passWays = RoomGenerator.Ins.roomSamplesWithExit[getSampleIndex].exitPos.ToArray();
                }
                else
                {
                    // 시작 방일 때
                    passWays = RoomGenerator.Ins.startRoom.exitPos.ToArray();
                }
            }
        }

        /// <summary>
        /// 출구 정보
        /// </summary>
        public PassWay[] passWays;

        /// <summary>
        /// 맵 그리드 좌표(방 한칸 당 하나)
        /// </summary>
        public Vector2Int gridCoord;

        /// <summary>
        /// 해당 방이 가지고 있을 Room데이터
        /// </summary>
        public Room roomData;

        /// <summary>
        /// 방 원점의 실제 위치값(그리드 좌표와 방 기본 크기에 따라 바뀜)
        /// </summary>
        public Vector3Int origineCoord;

        public void GetRoomData(Room room)
        {
            gridCoord = room.gridCoord;
            roomData = room;
        }

        public void GetRoomData(Vector2Int pos)
        {
            gridCoord = pos;
            roomData = new Room(pos);
        }

        public void SetOrigineCoord(int roomSize)
        {
            origineCoord = new Vector3Int(gridCoord.x * roomSize, gridCoord.y * roomSize);
            for(int i =0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }

        // 시작 방을 위한 함수
        public void SetOrigineCoord(Vector3Int startSize, int roomSize)
        {
            int startRoomGab = 20;
            // 고쳐야됨 !!
            // Debug.LogWarning("수정 필요"); 식을 잘못 썻음
            origineCoord = new Vector3Int((gridCoord.x + 1) * roomSize - startSize.x - startRoomGab, gridCoord.y * roomSize);
            for (int i = 0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }
    }

    //public void Test()
    //{
    //    MakeRoom test1 = new MakeRoom();
    //    test1.gridCoord = new Vector2Int(0, 0);
    //    test1.GetSampleIndex = 0;
    //    test1.SetOrigineCoord(maxSingleRoomSize);
        
    //    MakeRoom test2 = new MakeRoom();
    //    test2.gridCoord = new Vector2Int(1, 1);
    //    test2.GetSampleIndex = 1;
    //    test2.SetOrigineCoord(maxSingleRoomSize);

    //    MakeRoom test3 = new MakeRoom();
    //    test3.gridCoord = new Vector2Int(2, 0);
    //    test3.GetSampleIndex = 2;
    //    test3.SetOrigineCoord(maxSingleRoomSize);

    //    MakeRoom test4 = new MakeRoom();
    //    test4.gridCoord = new Vector2Int(2, 1);
    //    test4.GetSampleIndex = 2;
    //    test4.SetOrigineCoord(maxSingleRoomSize);

    //    GenerateRoom(test1.origineCoord, roomSamplesWithExit[test1.GetSampleIndex]);
    //    GenerateRoom(test2.origineCoord, roomSamplesWithExit[test2.GetSampleIndex]);
    //    GenerateRoom(test3.origineCoord, roomSamplesWithExit[test3.GetSampleIndex]);
    //    GenerateRoom(test4.origineCoord, roomSamplesWithExit[test4.GetSampleIndex]);

    //    //Test_ConnectPassway(test1, test2);
    //    ConnectPassway(test2, test4);
    //    ConnectPassway(test3, test2);
    //}

    
}

