using System.Collections;
using System.Collections.Generic;
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

        List<Vector3Int> roomSpots = new List<Vector3Int>();        // 방을 생성할 기준이 될 스팟들
        List<Vector3Int> roomsSize = new();
        for(int i = 0; i < randomMap.roomList.Count; i++)
        {
            if (roomSpots.Count == 0) roomSpots.Add(new Vector3Int(0, 0));
            //Vector3Int targetPoint = 
        }


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


    void GenerateMap(SampleRoomData targetRoomData)
    {
        foreach (List<Vector3Int> tilesPos in targetRoomData.tilesPos)
        {
            for(int i =0;  i < tilesPos.Count; i++)
            {
                m_tileMaps[i].SetTile(tilesPos[i] + cursor, targetRoomData.mapLayers[i].GetTile(tilesPos[i]));
            }
        }
    }

    /// <summary>
    /// 맵을 레이어별로 생성하는 메서드
    /// </summary>
    /// <param name="targetRoomData">맵 생성할 샘플 데이터</param>
    /// <param name="index">샘플에서 생성할 레이어</param>
    void GenerateMapLayer(SampleRoomData targetRoomData, int layer)
    {
        foreach (Vector3Int pos in targetRoomData.tilesPos[layer])      // 레이어(배경, 플랫폼 등)가 가지고 있는 각 타일을 하나씩 꺼냄
        {
            m_tileMaps[layer].SetTile(pos + cursor, targetRoomData.mapLayers[layer].GetTile(pos));
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

