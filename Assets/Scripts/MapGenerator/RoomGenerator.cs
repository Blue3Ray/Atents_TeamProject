using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

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
    /// 타일을 그릴 맵들(0 배경, 1 플랫폼, 2 반플랫폼, ... n-1 출입구)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    private void Awake()
    {
        // 타일을 작성할 레이어 불러오는 과정
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }
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

        for (int i = 0; i < roomSamplesWithExit[0].mapLayers.Count; i++)
        {
            GenerateMapLayer(roomSamplesWithExit[0], 0);
            GenerateMapLayer(roomSamplesWithExit[0], 1);
            GenerateMapLayer(roomSamplesWithExit[0], 2);
            GenerateExit(roomSamplesWithExit[0]);
        }

        cursor = new Vector3Int(15, 0);

        for (int i = 0; i < roomSamplesWithExit[1].mapLayers.Count; i++)
        {
            GenerateMapLayer(roomSamplesWithExit[1], 0);
            GenerateMapLayer(roomSamplesWithExit[1], 1);
            GenerateMapLayer(roomSamplesWithExit[1], 2);
            GenerateExit(roomSamplesWithExit[1]);
        }
    }

    //void GeneratePassExit(SampleRoomData targetRoomData)
    //{
    //    int index = targetRoomData.tilesPos.Count - 1;

    //    foreach (Vector3Int pos in targetRoomData.tilesPos[index])
    //    {
    //        m_tileMaps[index].SetTile(pos + cursor, targetRoomData.mapLayers[index].GetTile(pos));
    //    }
    //}


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

    void GenerateExit(SampleRoomData targetRoomData)
    {
        foreach(Exit temp in targetRoomData.exitPos)
        {
            int x = 0, y = 0,index = 0;
            if(temp.Direction == ExitDirection.Left|| temp.Direction == ExitDirection.Right)
            {
                y = -2;
            }
            else if(temp.Direction == ExitDirection.Up || temp.Direction == ExitDirection.Down)
            {
                x = -2;
                index = 1;
            }

            for (int i = 0; i < exitSamples[index].height; i++)    // 문 높이 만큼
            {
                for (int j = 0; j < exitSamples[index].width; j++)  // 문 너비 만큼
                {
                    if (exitSamples[index].mapLayers[1].HasTile(new Vector3Int(exitSamples[index].min.x + j, exitSamples[index].min.y + i)))
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), targetRoomData.mapLayers[1].GetTile(temp.Pos));
                        //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                    }
                    else       // 빈 타일이면 null(빈타일)로 바꾸기
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);
                    }
                    
                }
            }


        }
    }




}

