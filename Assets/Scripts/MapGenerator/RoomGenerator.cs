using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;


public struct RoomData
{
    public Vector3 pos;
    public SampleRoomData roomData;
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
    /// 출구 데이터 불러올 샘플들(0 가로, 1 세로)
    /// </summary>
    public SampleRoomData[] exitSamples;

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
    RandomMap randomMap;
    List<Room> sortList = new();

    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public uint roomCount = 8;


    private void Awake()
    {
        // 타일을 작성할 레이어 불러오는 과정
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }
        
        roomStack = new Stack<SampleRoomData>();

        randomMap = new RandomMap(width, height, fillRate, collecBoxBoolCount);
    }

    private void Start()
    {
        cursor = new Vector3Int(0, 0);

        foreach(SampleRoomData temp in roomSamplesWithExit)
        {
            temp.OnInitialize();
        }
        foreach (SampleRoomData temp in exitSamples)
        {
            temp.OnInitialize();
        }

        randomMap.StartMapData(10);
        randomMap.SortingRoomList(sortList, randomMap.roomList[0]);

        Test(randomMap.roomList[0], cursor);
        
        
        // 생성

        //GenerateMap(roomSamplesWithExit[0]);

        //roomStack.Push(roomSamplesWithExit[0]);

        //for (int i = 0; i < roomSamplesWithExit[0].mapLayers.Count; i++)      //레이어 별로 생성하기(비효율적이군)
        //{
        //    GenerateMapLayer(roomSamplesWithExit[0], 0);
        //    GenerateMapLayer(roomSamplesWithExit[0], 1);
        //    GenerateMapLayer(roomSamplesWithExit[0], 2);
        //    GenerateExit(roomSamplesWithExit[0], ExitDirection.Right);
        //}

        // 여기까지가 시작 방 생성(출구 포함)

        //cursor += new Vector3Int(roomStack.Peek().width, 0) + GetRoomGap(5);
    }


    public void SetupRooms()
    {
        // roomSamplesWithExit 방 샘플
        // randomMap 랜덤 맵

        // roomlist의 두번째는 X값 기준으로 되어 있음. 그러므로 연결된 방 기준으로 다시 정렬
        
        randomMap.SortingRoomList(sortList, randomMap.roomList[0]);


        List<Vector3Int> roomAnchor = new();        // 방을 생성할 기준(모든 방 프리펩은 0,0이 시작점이여야 함)
        List<Vector3Int> roomsSize = new();         // 방의 사이즈들



        for(int i = 0; i < sortList.Count; i++)     // 메인룸부터 연결되어 있는 방 순서대로 차례대로
        {
            // 먼저 어떤 방을 생성할지 선택함

            // 출구 개수와 만족하는 방리스트를 따로 만듬
            List<ExitDirection> upDir = new();
            List<ExitDirection> downDir = new();
            List<ExitDirection> rightDir = new();
            List<ExitDirection> leftDir = new();
            foreach (ExitDirection dir in sortList[i].connectedExit)
            {
                switch (dir)
                {
                    case ExitDirection.Up:
                        upDir.Add(dir);
                        break;
                    case ExitDirection.Left:
                        leftDir.Add(dir);
                        break;
                    case ExitDirection.Right:
                        rightDir.Add(dir);
                        break;
                    case ExitDirection.Down:
                        downDir.Add(dir);
                        break;
                }   
            }

            Debug.Log($"{i}번째 방은 UP : {upDir.Count}, Left : {leftDir.Count}, Down : {downDir.Count} ,right : {rightDir.Count}");
            List<SampleRoomData> canBuildRoomList = new();

            foreach (SampleRoomData roomData in roomSamplesWithExit) 
            {
                // 샘플에서 각 방향 출구 개수를 비교해서 그 이상인 방들만 걸러냄(구현 가능한 방들만 꺼내기)
                if(roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
                    roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
                {
                    canBuildRoomList.Add(roomData);
                }
                
            }
            if(!(canBuildRoomList.Count > 0)) Debug.LogWarning("구현 가능한 방이 없습니다.");

            // 배치 가능한 방들 중 랜덤으로 하나 선택
            SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

            SetRoomAnchor(cursor, targetRoom, ref roomAnchor, ref roomsSize);       // 첫 방은 0,0에서 시작

            Debug.Log($"{i}번째 방 생성 시도");

            cursor = new Vector3Int((int) sortList[i].CenterX, (int)sortList[i].CenterY);       // 표시 잘되나 임시로 만든 것

            GenerateRoom(targetRoom, cursor);           // 생성
            

            // 커서는 다음 위치로 이동(어디로? 어떻게 다시 돌아오지?)
        }
    }

    void Test(Room room, Vector3Int cursor)
    {
        Vector3Int tempCursor = cursor;

        List<ExitDirection> upDir = new();
        List<ExitDirection> downDir = new();
        List<ExitDirection> rightDir = new();
        List<ExitDirection> leftDir = new();
        foreach (ExitDirection dir in room.connectedExit)
        {
            switch (dir)
            {
                case ExitDirection.Up:
                    upDir.Add(dir);
                    break;
                case ExitDirection.Left:
                    leftDir.Add(dir);
                    break;
                case ExitDirection.Right:
                    rightDir.Add(dir);
                    break;
                case ExitDirection.Down:
                    downDir.Add(dir);
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

        SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

        GenerateRoom(targetRoom, tempCursor);           // 생성

        for (int j = 0; j < room.connectedRooms.Count; j++)
        {
            switch (room.connectedExit[j])
            {
                case ExitDirection.Up:
                    tempCursor.y += 10;
                    break;
                case ExitDirection.Left:
                    tempCursor.x -= 10;
                    break;
                case ExitDirection.Right:
                    tempCursor.x += 10;
                    break;
                case ExitDirection.Down:
                    tempCursor.y -= 10;
                    break;
            }
            Test(room.connectedRooms[j], tempCursor);
        }
        
    }

    // 첫번째 방을 선택한 후에 연결된 자식들 대상으로 다시 실행하기

    void SetRoomAnchor(Vector3Int cursor, SampleRoomData roomData, ref List<Vector3Int> anchors, ref List<Vector3Int> sizes)
    {
        anchors.Add(cursor);
        sizes.Add(roomData.max);
    }


    void GeneratePassway(Exit startPos, Exit endPos)
    {
        cursor = startPos.Pos;

        int xDir = 0, yDir = 0;             // 통로가 만들어지는 방향(대각선 경우는 없음)
        switch (startPos.Direction) 
        { 
            case ExitDirection.Up:
                yDir = 1;
                break;
            case ExitDirection.Down:
                yDir = -1;
                break;
            case ExitDirection.Right:
                xDir = 1;
                break;
            case ExitDirection.Left:
                xDir = -1;
                break;
            default:
                break;
        }

        int i = 0;

        while (i < 50 && cursor != endPos.Pos)
        {
            cursor += new Vector3Int(xDir, yDir);
            GeneratePass(cursor, new Vector3Int(xDir, yDir));

            if(cursor.y == endPos.Pos.y && yDir != 0)
            {
                yDir = 0;
                xDir = cursor.x < endPos.Pos.x ? 1 : -1;
                GeneratePass(cursor, new Vector3Int(xDir, yDir));
            }
            else if(cursor.x == endPos.Pos.x && xDir != 0)
            {
                xDir = 0;
                yDir = cursor.y < endPos.Pos.y ? 1 : -1;
            }
            i++;
        }
    }

    /// <summary>
    /// 맵을 레이어별로 생성하는 메서드
    /// </summary>
    /// <param name="targetRoomData">맵 생성할 샘플 데이터</param>
    /// <param name="index">샘플에서 생성할 레이어</param>
    void GenerateRoom(SampleRoomData targetRoomData, Vector3Int cursor)
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

    void GeneratePass(Vector3Int pos, Vector3Int dir)
    {
        int exitIndex = 0;      // 좌우로 그릴건지 상하로 그릴건지 구분
        if (dir.x == 0)
        {
            exitIndex = 1;
        }

        for (int i = 0; i < exitSamples[exitIndex].Height; i++)    // 문 높이 만큼
        {
            for (int j = 0; j < exitSamples[exitIndex].Width; j++)  // 문 너비 만큼
            {
                if (exitSamples[exitIndex].mapLayers[1].HasTile(new Vector3Int(exitSamples[exitIndex].min.x + j, exitSamples[exitIndex].min.y + i)))
                {
                    m_tileMaps[1].SetTile(cursor + new Vector3Int(j, i), exitSamples[exitIndex].mapLayers[1].GetTile(new Vector3Int(j, i)));
                    //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                }
                else
                {
                    //m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // 빈 타일이면 null(빈타일)로 바꾸기
                }
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

            for (int i = 0; i < exitSamples[index].Height; i++)    // 문 높이 만큼
            {
                for (int j = 0; j < exitSamples[index].Width; j++)  // 문 너비 만큼
                {
                    if (exitSamples[index].mapLayers[1].HasTile(new Vector3Int(exitSamples[index].min.x + j, exitSamples[index].min.y + i)))
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
        return new Vector3Int(x,y);
    }


}

