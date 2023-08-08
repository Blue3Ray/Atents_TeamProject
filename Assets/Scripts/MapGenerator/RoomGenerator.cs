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
    /// ����� �׸� Ÿ��
    /// </summary>
    public Tile backgroundTile;

    /// <summary>
    /// �ⱸ�� �����ϱ����� ���� Ÿ��
    /// </summary>
    public Tile exitTile;

    /// <summary>
    /// �ⱸ ������ �ҷ��� ���õ�(0 ����, 1 ����. 2 UR, 3 RD, 4 DL, 5LU)
    /// </summary>
    public SampleRoomData[] passWaySamples;

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
    RandomMapGenerator randomMap;

    // �ʱ� ���� �� ���� ---------------------
    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public int roomCount = 8;

    // ���� �� ���� ----------------------

    /// <summary>
    /// ���� ���� ���� ū �� ���� ũ��(���簢�� �Ѻ��� ����)
    /// </summary>
    int maxSingleRoomSize = 0;

    private void Awake()
    {
        // Ÿ���� �ۼ��� ���̾� �ҷ����� ����
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
                    // �� �����ϴ� �Լ�
                    GenerateRoom(new Vector3Int((x - startPos.x) * maxSingleRoomSize, (y - startPos.y) * maxSingleRoomSize), roomSamplesWithExit[Random.Range(0, roomSamplesWithExit.Length - 1)]);
                }
            }
        }
    }



    //public void SetupRooms()
    //{
    //    // roomSamplesWithExit �� ����
    //    // randomMap ���� ��

    //    // roomlist�� �ι�°�� X�� �������� �Ǿ� ����. �׷��Ƿ� ����� �� �������� �ٽ� ����
        
    //    randomMap.SortingRoomList(sortList, randomMap.roomList[0]);


    //    List<Vector3Int> roomAnchor = new();        // ���� ������ ����(��� �� �������� 0,0�� �������̿��� ��)
    //    List<Vector3Int> roomsSize = new();         // ���� �������



    //    for(int i = 0; i < sortList.Count; i++)     // ���η���� ����Ǿ� �ִ� �� ������� ���ʴ��
    //    {
    //        // ���� � ���� �������� ������

    //        // �ⱸ ������ �����ϴ� �渮��Ʈ�� ���� ����
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

    //        Debug.Log($"{i}��° ���� UP : {upDir.Count}, Left : {leftDir.Count}, Down : {downDir.Count} ,right : {rightDir.Count}");
    //        List<SampleRoomData> canBuildRoomList = new();

    //        foreach (SampleRoomData roomData in roomSamplesWithExit) 
    //        {
    //            // ���ÿ��� �� ���� �ⱸ ������ ���ؼ� �� �̻��� ��鸸 �ɷ���(���� ������ ��鸸 ������)
    //            if(roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
    //                roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
    //            {
    //                canBuildRoomList.Add(roomData);
    //            }
                
    //        }
    //        if(!(canBuildRoomList.Count > 0)) Debug.LogWarning("���� ������ ���� �����ϴ�.");

    //        // ��ġ ������ ��� �� �������� �ϳ� ����
    //        SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

    //        SetRoomAnchor(cursor, targetRoom, ref roomAnchor, ref roomsSize);       // ù ���� 0,0���� ����

    //        Debug.Log($"{i}��° �� ���� �õ�");

    //        cursor = new Vector3Int((int) sortList[i].CenterX, (int)sortList[i].CenterY);       // ǥ�� �ߵǳ� �ӽ÷� ���� ��

    //        GenerateRoom(targetRoom, cursor);           // ����
            
    //        // Ŀ���� ���� ��ġ�� �̵�(����? ��� �ٽ� ���ƿ���?)
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
    //        // ���ÿ��� �� ���� �ⱸ ������ ���ؼ� �� �̻��� ��鸸 �ɷ���(���� ������ ��鸸 ������)
    //        if (roomData.GetExitCount(ExitDirection.Up) >= upDir.Count && roomData.GetExitCount(ExitDirection.Down) >= downDir.Count &&
    //            roomData.GetExitCount(ExitDirection.Left) >= leftDir.Count && roomData.GetExitCount(ExitDirection.Right) >= rightDir.Count)
    //        {
    //            canBuildRoomList.Add(roomData);
    //        }

    //    }
    //    if (!(canBuildRoomList.Count > 0)) Debug.LogWarning("���� ������ ���� �����ϴ�.");

    //    SampleRoomData targetRoom = canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];

    //    if (CheckBuildable(tempCursor, targetRoom))
    //    {
    //        GenerateRoom(targetRoom, tempCursor);           // ����
    //    }
    //    else
    //    {
    //        Debug.Log("�� ������ �ȵ˴ϴ�.");
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


    //// ù��° ���� ������ �Ŀ� ����� �ڽĵ� ������� �ٽ� �����ϱ�
    //void SetRoomAnchor(Vector3Int cursor, SampleRoomData roomData, ref List<Vector3Int> anchors, ref List<Vector3Int> sizes)
    //{
    //    anchors.Add(cursor);
    //    sizes.Add(roomData.max);
    //}

    ///// <summary>
    ///// Ŀ�� ��ġ�κ��� �׸����� roomData�� �׷����� �ִ��� üũ�ϴ� �Լ�
    ///// </summary>
    ///// <param name="cursor">���� �������� �� ��ġ</param>
    ///// <param name="roomData">�׸� ��</param>
    ///// <returns>���̸� �ش� ������ �ٸ� ��� ������ ���� ����, �����̸� �ٸ� ��� ��ħ</returns>
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

        // ���� �� �ⱸ�� ���� ���� �ִ� ��찡 �ƴϸ�(�밢���� ���������ϸ�) �߰� �������� S�� �������� ����
        Vector3Int halfPos = new Vector3Int((int)((startPos.Pos.x + endPos.Pos.x) * 0.5f), (int)((startPos.Pos.y + endPos.Pos.y) * 0.5f));

        Vector3Int targetPos = endPos.Pos;

        bool isY;

        if(Mathf.Abs(startPos.Pos.x - endPos.Pos.x) > Mathf.Abs(startPos.Pos.y - endPos.Pos.y)) // x�� ������ �� �� ��
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

        //int i = 0;      // ��� ����� ȣ�� ���� ����(���ѹݺ� ���)

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
        //int passWayType = 0;      // �¿�� �׸����� ���Ϸ� �׸����� ����(���� ��ȣ��, 0 : X����, 1 : Y����)

        Vector2Int orin = Vector2Int.zero;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        Debug.Log(targetData.min);

        for (int i = 0; i < targetData.Height; i++)    // �� ���� ��ŭ
        {
            for (int j = 0; j < targetData.Width; j++)  // �� �ʺ� ��ŭ
            {
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    // �÷��� ĭ�� ��
                    m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[(int)MapLayer.PlatForm].GetTile(targetDrawPos));
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // ���÷��� ĭ�� ��
                    m_tileMaps[(int)MapLayer.HalfPlatForm].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[(int)MapLayer.HalfPlatForm].GetTile(targetDrawPos));
                }
                else
                {
                    //m_tileMaps[1].SetTile(temp.Pos + cursor + new Vector3Int(j + x, i + y), null);   // �� Ÿ���̸� null(��Ÿ��)�� �ٲٱ�
                }
            }
        }

        return result;
    }

    /// <summary>
    /// ���� ���̾�� �����ϴ� �޼���
    /// </summary>
    /// <param name="targetRoomData">�� ������ ���� ������</param>
    /// <param name="index">���ÿ��� ������ ���̾�</param>
    void GenerateRoom(Vector3Int cursor, SampleRoomData targetRoomData)
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

            for (int i = 0; i < passWaySamples[index].Height; i++)    // �� ���� ��ŭ
            {
                for (int j = 0; j < passWaySamples[index].Width; j++)  // �� �ʺ� ��ŭ
                {
                    if (passWaySamples[index].mapLayers[1].HasTile(new Vector3Int(passWaySamples[index].min.x + j, passWaySamples[index].min.y + i)))
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
        return new Vector3Int(x, y);
    }
}

