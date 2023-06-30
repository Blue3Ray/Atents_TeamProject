using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// ����� �׸� Ÿ��
    /// </summary>
    public Tile backgroundTile;

    /// <summary>
    /// �ⱸ�� �����ϱ����� ���� Ÿ��
    /// </summary>
    public Tile exitTile;

    /// <summary>
    /// �ⱸ ������ �ҷ��� ���õ�(0 ����, 1 ����)
    /// </summary>
    public SampleRoomData[] exitSamples;

    /// <summary>
    /// �� ������ �ҷ��� ���õ�(ù�����Ҷ� 0�� �׻� ���� ��)
    /// </summary>
    public SampleRoomData[] roomSamplesWithExit;

    /// <summary>
    /// Ÿ���� �׸� �ʵ�(0 ���, 1 �÷���, 2 ���÷���, ... n-1 ���Ա�)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
    /// </summary>
    Vector3Int cursor;

    private void Awake()
    {
        // Ÿ���� �׸� ���̾� �ҷ����� ����
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
        // ����

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
                Debug.Log($"{exitSamples[0].width}, {exitSamples[0].height} : �ⱸũ��");
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
