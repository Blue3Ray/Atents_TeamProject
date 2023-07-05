using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;


public struct RoomData
{
    public Vector3 pos;
    public SampleRoomData roomData;
}



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

    Stack<Vector3Int> cursorPosStack;
    Stack<RoomData> roomStack;

    private void Awake()
    {
        // Ÿ���� �ۼ��� ���̾� �ҷ����� ����
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
    /// ���� ���̾�� �����ϴ� �޼���
    /// </summary>
    /// <param name="targetRoomData">�� ������ ���� ������</param>
    /// <param name="index">���ÿ��� ������ ���̾�</param>
    void GenerateMapLayer(SampleRoomData targetRoomData, int layer)
    {
        foreach (Vector3Int pos in targetRoomData.tilesPos[layer])      // ���̾�(���, �÷��� ��)�� ������ �ִ� �� Ÿ���� �ϳ��� ����
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

            for (int i = 0; i < exitSamples[index].height; i++)    // �� ���� ��ŭ
            {
                for (int j = 0; j < exitSamples[index].width; j++)  // �� �ʺ� ��ŭ
                {
                    if (exitSamples[index].mapLayers[1].HasTile(new Vector3Int(exitSamples[index].min.x + j, exitSamples[index].min.y + i)))
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), targetRoomData.mapLayers[1].GetTile(temp.Pos));
                        //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                    }
                    else       // �� Ÿ���̸� null(��Ÿ��)�� �ٲٱ�
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);
                    }
                }
            }
        }
    }




}

