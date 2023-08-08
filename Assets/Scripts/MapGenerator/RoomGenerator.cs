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

public class RoomGenerator : MonoBehaviour
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


    // 기능 부분 ------------------
    RandomMapGenerator randomMap;

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

    private void Awake()
    {
        // 타일을 작성할 레이어 불러오는 과정
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }
        
        roomStack = new Stack<SampleRoomData>();

        randomMap = new RandomMapGenerator();
        randomMap.SetUp(roomCount, width, height, fillRate, collecBoxBoolCount);
    }

    private void Start()
    {
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

        //SetUpRooms();
    }


    public void SetUpRooms()
    {
        Vector2Int startPos = randomMap.gridMap.GetRoomGrid(randomMap.roomList[0]);

        for(int x = 0; x < randomMap.gridMap.Width; x++)
        {
            for(int y = 0; y < randomMap.gridMap.Height; y++)
            {
                if (randomMap.gridMap.mapGrid[x,y] != null)
                {
                    // 방 선택하는 함수
                    GenerateRoom(new Vector3Int((x - startPos.x) * maxSingleRoomSize, (y - startPos.y) * maxSingleRoomSize), roomSamplesWithExit[Random.Range(0, roomSamplesWithExit.Length - 1)]);
                }
            }
        }
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

    public void GeneratePassway(Exit startPos, Exit endPos)
    {
        cursor = startPos.Pos;

        // 만약 두 출구가 같은 선상에 있는 경우가 아니면(대각으로 움직여야하면) 중간 지점에서 S자 꺽기위한 지점
        Vector3Int halfPos = new Vector3Int((int)((startPos.Pos.x + endPos.Pos.x) * 0.5f), (int)((startPos.Pos.y + endPos.Pos.y) * 0.5f));

        Vector3Int targetPos = endPos.Pos;

        bool isY;

        if(Mathf.Abs(startPos.Pos.x - endPos.Pos.x) > Mathf.Abs(startPos.Pos.y - endPos.Pos.y)) // x축 쪽으로 더 길 때
        {
            isY = false;
            if (startPos.Pos.x != endPos.Pos.x) targetPos = halfPos;
        }
        else
        {
            isY = true;
            if (startPos.Pos.x != endPos.Pos.x) targetPos = halfPos;
        }
        if (isY) isY = false;
        GeneratePass(cursor, PassWayType.UpRight, 0);
        //GenerateRoom(cursor, roomSamplesWithExit[2]);

        //int i = 0;      // 통로 만드는 호출 개수 제한(무한반복 대비)

        //ExitDirection dir = startPos.Direction;

        //while (i < 1000 && cursor != endPos.Pos)
        //{
        //    Vector3Int tempPos = Vector3Int.zero;
        //    int drawOverCount = -1;


        //    if(cursor == targetPos)
        //    {
        //        if(dir == ExitDirection.Up || dir == ExitDirection.Down)
        //        {
        //            if(IsXDir(cursor, targetPos))
        //            {

        //            }
        //        }
        //    }

        //    cursor += tempPos;
        //}
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

    Vector3Int GeneratePass(Vector3Int cursorPos, PassWayType passWayType, int drawOverCount)
    {
        Vector3Int result = Vector3Int.zero;
        //int passWayType = 0;      // 좌우로 그릴건지 상하로 그릴건지 구분(샘플 번호임, 0 : X방향, 1 : Y방향)

        Vector2Int orin = Vector2Int.zero;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        Debug.Log(targetData.min);

        for (int i = 0; i < targetData.Height; i++)    // 문 높이 만큼
        {
            for (int j = 0; j < targetData.Width; j++)  // 문 너비 만큼
            {
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    // 플랫폼 칸일 때
                    m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[(int)MapLayer.PlatForm].GetTile(targetDrawPos));
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // 반플랫폼 칸일 때
                    m_tileMaps[(int)MapLayer.HalfPlatForm].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[(int)MapLayer.HalfPlatForm].GetTile(targetDrawPos));
                }
                else
                {
                    //m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // 빈 타일이면 null(빈타일)로 바꾸기
                }
            }
        }

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
        foreach(Exit temp in targetRoomData.exitPos)
        {
            if (temp.Direction != exitDir) continue;            // 만약 선택한 출입구가 받은 파라미터 방향과 같지 않으면 스킵
            int x = 0, y = 0;   // 방향이 좌우에 따라 출입구 그려지는 시작위치
            int index = 0;      // 0은 좌우 출입구, 1은 상하 출입구
            if(temp.Direction == ExitDirection.Left|| temp.Direction == ExitDirection.Right)
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
}

