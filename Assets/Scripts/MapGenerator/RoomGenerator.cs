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
    /// ������ ����� �׿����� ����
    /// </summary>
    public Stack<SampleRoomData> roomStack;

    /// <summary>
    /// Ÿ���� �׸� �ʵ�(0 ���, 1 �÷���, 2 ���÷���, ... n-1 ���Ա�)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
    /// </summary>
    Vector3Int cursor;


    // ��� �κ� ------------------
    RandomMap randomMap;

    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public uint roomCount = 8;


    private void Awake()
    {
        // Ÿ���� �ۼ��� ���̾� �ҷ����� ����
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
        
        
        
        // ����

        //GenerateMap(roomSamplesWithExit[0]);

        //roomStack.Push(roomSamplesWithExit[0]);

        //for (int i = 0; i < roomSamplesWithExit[0].mapLayers.Count; i++)      //���̾� ���� �����ϱ�(��ȿ�����̱�)
        //{
        //    GenerateMapLayer(roomSamplesWithExit[0], 0);
        //    GenerateMapLayer(roomSamplesWithExit[0], 1);
        //    GenerateMapLayer(roomSamplesWithExit[0], 2);
        //    GenerateExit(roomSamplesWithExit[0], ExitDirection.Right);
        //}

        // ��������� ���� �� ����(�ⱸ ����)

        //cursor += new Vector3Int(roomStack.Peek().width, 0) + GetRoomGap(5);
    }


    public void SetupRooms()
    {
        // roomSamplesWithExit �� ����
        // randomMap ���� ��

        List<Vector3Int> roomSpots = new List<Vector3Int>();        // ���� ������ ������ �� ���̵�
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

        int xDir = 0, yDir = 0;             // ��ΰ� ��������� ����(�밢�� ���� ����)
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

    void GeneratePass(Vector3Int pos, Vector3Int dir)
    {
        int exitIndex = 0;      // �¿�� �׸����� ���Ϸ� �׸����� ����
        if (dir.x == 0)
        {
            exitIndex = 1;
        }

        for (int i = 0; i < exitSamples[exitIndex].Height; i++)    // �� ���� ��ŭ
        {
            for (int j = 0; j < exitSamples[exitIndex].Width; j++)  // �� �ʺ� ��ŭ
            {
                if (exitSamples[exitIndex].mapLayers[1].HasTile(new Vector3Int(exitSamples[exitIndex].min.x + j, exitSamples[exitIndex].min.y + i)))
                {
                    m_tileMaps[1].SetTile(cursor + new Vector3Int(j, i), exitSamples[exitIndex].mapLayers[1].GetTile(new Vector3Int(j, i)));
                    //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                }
                else
                {
                    //m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // �� Ÿ���̸� null(��Ÿ��)�� �ٲٱ�
                }
            }
        }


    }

    void GenerateExit(SampleRoomData targetRoomData, ExitDirection exitDir)
    {
        foreach(Exit temp in targetRoomData.exitPos)
        {
            if (temp.Direction != exitDir) continue;            // ���� ������ ���Ա��� ���� �Ķ���� ����� ���� ������ ��ŵ
            int x = 0, y = 0;   // ������ �¿쿡 ���� ���Ա� �׷����� ������ġ
            int index = 0;      // 0�� �¿� ���Ա�, 1�� ���� ���Ա�
            if(temp.Direction == ExitDirection.Left|| temp.Direction == ExitDirection.Right)
            {
                y = -2;
            }
            else if(temp.Direction == ExitDirection.Up || temp.Direction == ExitDirection.Down)
            {
                x = -2;
                index = 1;
            }

            for (int i = 0; i < exitSamples[index].Height; i++)    // �� ���� ��ŭ
            {
                for (int j = 0; j < exitSamples[index].Width; j++)  // �� �ʺ� ��ŭ
                {
                    if (exitSamples[index].mapLayers[1].HasTile(new Vector3Int(exitSamples[index].min.x + j, exitSamples[index].min.y + i)))
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), targetRoomData.mapLayers[1].GetTile(temp.Pos));
                        //Debug.Log($"{targetRoomData.mapLayers[1].GetTile(pos)}");
                    }
                    else       
                    {
                        m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // �� Ÿ���̸� null(��Ÿ��)�� �ٲٱ�
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

