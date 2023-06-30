using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        // 타일을 그릴 레이어 불러오는 과정
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
            GenerateMapLayer(roomSamplesWithExit[0], 3);
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

    void GenerateMapLayer(SampleRoomData targetRoomData, int index)
    {
        foreach (Vector3Int pos in targetRoomData.tilesPos[index])
        {
            if (index == targetRoomData.tilesPos.Count - 1)
            {
                Debug.Log($"{exitSamples[0].width}, {exitSamples[0].height} : 출구크기");
                for (int i = 2; i < exitSamples[0].height - 2; i++)
                {
                    for (int j = 0; j < exitSamples[0].width; j++)
                    {
                        if (targetRoomData.mapLayers[1].HasTile(pos))
                        {
                            m_tileMaps[3].SetTile(pos + cursor + new Vector3Int(j, i), targetRoomData.mapLayers[1].GetTile(pos));
                        }
                        else
                        {
                            Debug.Log("asd");
                        }
                        
                    }
                }
            }
            else
            {
                m_tileMaps[index].SetTile(pos + cursor, targetRoomData.mapLayers[index].GetTile(pos));
            }
        }

    }




}
