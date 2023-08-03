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
    List<Room> sortList = new();

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

        randomMap.StartMapData(10);
        randomMap.SortingRoomList(sortList, randomMap.roomList[0]);

        Test(randomMap.roomList[0], cursor);
        
        
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

        // roomlist�� �ι�°�� X�� �������� �Ǿ� ����. �׷��Ƿ� ����� �� �������� �ٽ� ����
        
        randomMap.SortingRoomList(sortList, randomMap.roomList[0]);


        List<Vector3Int> roomAnchor = new();        // ���� ������ ����(��� �� �������� 0,0�� �������̿��� ��)
        List<Vector3Int> roomsSize = new();         // ���� �������



        for(int i = 0; i < sortList.Count; i++)     // ���η���� ����Ǿ� �ִ� �� ������� ���ʴ��
        {
            // ���� � ���� �������� ������

            // �ⱸ ������ �����ϴ� �渮��Ʈ�� ���� ����
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

            Debug.Log($"{i}��° ���� UP : {upDir.Count}, Left : {leftDir.Count}, Down : {downDir.Count} ,right : {rightDir.Count}");
            List<SampleRoomData> canBuildRoomList = new();

            foreach (SampleRoomData roomData in roomSamplesWithExit) 
            {
                // ���ÿ��� �� ���� �ⱸ ������ ���ؼ� �� �̻��� ��鸸 �ɷ���(���� ������ ��鸸 ������)
                if(roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
                    roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
                {
                    canBuildRoomList.Add(roomData);
                }
                
            }
            if(!(canBuildRoomList.Count > 0)) Debug.LogWarning("���� ������ ���� �����ϴ�.");

            // ��ġ ������ ��� �� �������� �ϳ� ����
            SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

            SetRoomAnchor(cursor, targetRoom, ref roomAnchor, ref roomsSize);       // ù ���� 0,0���� ����

            Debug.Log($"{i}��° �� ���� �õ�");

            cursor = new Vector3Int((int) sortList[i].CenterX, (int)sortList[i].CenterY);       // ǥ�� �ߵǳ� �ӽ÷� ���� ��

            GenerateRoom(targetRoom, cursor);           // ����
            

            // Ŀ���� ���� ��ġ�� �̵�(����? ��� �ٽ� ���ƿ���?)
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
            // ���ÿ��� �� ���� �ⱸ ������ ���ؼ� �� �̻��� ��鸸 �ɷ���(���� ������ ��鸸 ������)
            if (roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
                roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
            {
                canBuildRoomList.Add(roomData);
            }

        }
        if (!(canBuildRoomList.Count > 0)) Debug.LogWarning("���� ������ ���� �����ϴ�.");

        SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

        GenerateRoom(targetRoom, tempCursor);           // ����

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

    // ù��° ���� ������ �Ŀ� ����� �ڽĵ� ������� �ٽ� �����ϱ�

    void SetRoomAnchor(Vector3Int cursor, SampleRoomData roomData, ref List<Vector3Int> anchors, ref List<Vector3Int> sizes)
    {
        anchors.Add(cursor);
        sizes.Add(roomData.max);
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

    /// <summary>
    /// ���� ���̾�� �����ϴ� �޼���
    /// </summary>
    /// <param name="targetRoomData">�� ������ ���� ������</param>
    /// <param name="index">���ÿ��� ������ ���̾�</param>
    void GenerateRoom(SampleRoomData targetRoomData, Vector3Int cursor)
    {
        //foreach (List<Vector3Int> poses in targetRoomData.tilesPos)
        for(int i = 0; i < targetRoomData.tilesPos.Count - 1; i++)          // ���̾� ���� ����(���������� �ⱸ ���̾�� ǥ�þ���
        {
            List<Vector3Int> poses = targetRoomData.tilesPos[i];
            foreach (Vector3Int pos in poses)                       // ���̾ �ִ� Ÿ�ϵ�
            {
                m_tileMaps[i].SetTile(pos + cursor, targetRoomData.mapLayers[i].GetTile(pos));
            }
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

