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




public class RoomGenerator : Singleton<RoomGenerator>
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

    struct Passes
    {
        public PassWay start;
        public PassWay end;
    }

    // ��� �κ� ------------------
    RandomMapGenerator randomMap;

    SampleRoomData[,] mapGrid;

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

    

    private void Start()
    {
        // Ÿ���� �ۼ��� ���̾� �ҷ����� ����
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        roomStack = new Stack<SampleRoomData>();

        randomMap = new RandomMapGenerator();

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

        maxSingleRoomSize += 5;
        //SetUpRooms();
    }


    public void SetUpRooms()
    {
        randomMap.SetUp(roomCount, width, height, fillRate, collecBoxBoolCount);

        Vector2Int startPos = randomMap.gridMap.GetRoomGrid(randomMap.roomList[0]);

        for(int y = 0; y < randomMap.gridMap.Height; y++)
        {
            for(int x = 0; x < randomMap.gridMap.Width; x++)
            {
                Room target = randomMap.gridMap.mapGrid[x, y];
                if (target != null)
                {
                    //gridMap[x, y] = new NodeRoom();
                }
            }
        }

        // ���� ����� �κ�
        // ���� ���鶧 ��� ������ �����ؾߵȴ�.
        List<(PassWay, PassWay)> passWays = new List<(PassWay, PassWay)>();

        /*
        randomMap�� roomList�� �ִ�.
        randomMap�� gridMap�� ������ roomList�������� ������� �׸��� ���̴�.

        List<SampleRoom> roomBuild = new(roomList.Count);
        

        1. ù ���� �����Ѵ�. / GenerateRoom
            => randomMap.roomList[0]�� �ⱸ ������ Left�ϳ��� ������ startRoom�� ����Ǿ�� �Ѵ�.
            => gridMap�� ���۹��� �ⱸ ������ ���ų� �̻��� sampleRoom�� �����Ѵ�.
                roomBuild.Add(targetRoom);
            
            2. ��� ����� ����� ���� ���� �ȵ� ���� Ȯ���Ѵ� 
                => randomMap�� �ش� Grid���� Room�� ConnectedRoom�� item2�� isConnected�� Ȯ��
                ��� ����� ���� �� ������ �ȵ� �ⱸ�� �ϳ� ��� �����Ѵ� 
                => ���õ� SampleRoom�� exitPos����Ʈ�� �ش� isConnected�������� ���� ��ȯ
                => PassWay targetOne

            3. �ش� ���� ���� ������ ���� �����Ѵ� / GenerateRoom
                => �ش� ���� �̹� ������ �� ���� ����(������ �׷��� ����)
                => randomMap�� �ش� �׸��忡 ��ġ ������ SampleRoom ����
                 
            4. �ش� ��� ������ ���Ա��� �����Ѵ� 
                => ���õ� SampleRoom�� exitPos����Ʈ�� �ش� isConnected�������� ���� ��ȯ
                => PassWay targetTwo
            5. �ΰ� �ⱸ ������ ����Ʈ�� �����Ѵ�.passWays.Add(targetOne, targetTwo)

            6. 2������ ���ư��� �ش��� ����� ���� ��� ���� �ɶ����� �ݺ��Ѵ�. => ������?
                
        7. �� ���� ������ �Ѿ �� ��� ���� ������ ���� �� ������ �ݺ��Ѵ�.

        ������ �� ���� ������ �־�� �� ����
        1. ũ�� ���μ���, �������κ����� �Ÿ� ��� ���̾ Ÿ�� ����
        2. �ⱸ ����(�ⱸ�� ��ġ �� ����)
        3. ����Ǿ� �ִ� ��� ���� ����

        */

        // for������ ��� �׸��� �˻�� ȿ�������� ��Ȥ ���� ���� Ȯ���ϱ⵵ �����
        //for (int x = 0; x < randomMap.gridMap.Width; x++)
        //{
        //    for (int y = 0; y < randomMap.gridMap.Height; y++)
        //    {
        //        if (randomMap.gridMap.mapGrid[x, y] != null)
        //        {
        //            GenerateRoom(new Vector3Int((x - startPos.x) * maxSingleRoomSize, (y - startPos.y) * maxSingleRoomSize), roomSamplesWithExit[Random.Range(0, roomSamplesWithExit.Length - 1)]);
        //        }
        //    }
        //}
        //----------------------------



        // ��� ����� �κ�

        int[] dirCount = new int[4];

        for (int i = 0; i < randomMap.gridMap.mapGrid[startPos.x, startPos.y].connectedRooms.Count; i++)
        {
            switch (randomMap.gridMap.mapGrid[startPos.x, startPos.y].connectedRooms[i].Item2)
            {
                case ExitDirection.Up:
                    dirCount[0]++;
                    break;
                case ExitDirection.Left:
                    dirCount[1]++;
                    break;
                case ExitDirection.Right:
                    dirCount[2]++;
                    break;
                case ExitDirection.Down:
                    dirCount[3]++;
                    break;
                default:
                    break;
            }
        }


    }

    /// <summary>
    /// �ⱸ ������ �����ϴ� ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="targetRoom">������ ��</param>
    /// <returns>���ǿ� �´� ���� ��</returns>
    SampleRoomData GetRandomRoom(Room targetRoom)
    {
        //�ⱸ ������ �����ϴ� �渮��Ʈ�� ���� ����
        List<ExitDirection> upDir = new();
        List<ExitDirection> downDir = new();
        List<ExitDirection> rightDir = new();
        List<ExitDirection> leftDir = new();

        foreach (var item in targetRoom.connectedRooms)
        {
            switch (item.Item2)
            {
                case ExitDirection.Up:
                    upDir.Add(item.Item2);
                    break;
                case ExitDirection.Left:
                    leftDir.Add(item.Item2);
                    break;
                case ExitDirection.Right:
                    rightDir.Add(item.Item2);
                    break;
                case ExitDirection.Down:
                    downDir.Add(item.Item2);
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

        // ��ġ ������ ��� �� �������� �ϳ� ����
        return canBuildRoomList[Random.Range(0, canBuildRoomList.Count - 1)];
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

    /// <summary>
    /// �� ��ġ�� ������ ������ ��θ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="startPos">���� ����</param>
    /// <param name="endPos">���� ����</param>
    public void GeneratePassway(PassWay startPos, PassWay endPos)
    {
        // �ⱸ�� �Ѵܰ� ���� ���� �ܰ�
        PassWay endPosByOne = endPos;
        //PassWayType pwt = PassWayType.LeftRight;
        //if (endPosByOne.Direction == ExitDirection.Up || endPosByOne.Direction == ExitDirection.Down) pwt = PassWayType.UpDown;
        //endPosByOne.Pos += GeneratePass(new PassWay(endPosByOne.Pos, endPosByOne.Direction), pwt);

        cursor = startPos.Pos;

        Queue<Vector3Int> wayPoints = new ();

        // ���� �� �ⱸ�� ���� ���� �ִ� ��찡 �ƴϸ�(�밢���� ���������ϸ�) �߰� �������� S�� �������� ����
        Vector3Int halfPos = new Vector3Int((int)((startPos.Pos.x + endPos.Pos.x) * 0.5f), (int)((startPos.Pos.y + endPos.Pos.y) * 0.5f));

        // ��Ʈ�÷��׷� �߾�� �ߴµ�...
        // �Ա� �ⱸ ������ ���� ��(�����̰ų� �����϶�)
        if((startPos.Direction == ExitDirection.Up && endPos.Direction == ExitDirection.Down) ||
            (startPos.Direction == ExitDirection.Down && endPos.Direction == ExitDirection.Up) ||
            (startPos.Direction == ExitDirection.Right && endPos.Direction == ExitDirection.Left) ||
            (startPos.Direction == ExitDirection.Left && endPos.Direction == ExitDirection.Right))
        {
            if (startPos.Pos.x == endPos.Pos.x || startPos.Pos.y == endPos.Pos.y)
            {
                // ���ڷ� ��
                wayPoints.Enqueue(endPos.Pos);
            }
            else
            {
                //s�ڷ� ��
                if(startPos.Direction == ExitDirection.Up || startPos.Direction == ExitDirection.Down)
                {
                    wayPoints.Enqueue(new Vector3Int(startPos.Pos.x, halfPos.y));
                    wayPoints.Enqueue(new Vector3Int(endPos.Pos.x, halfPos.y));
                    //wayPoints.Add(new Vector3Int(startPos.Pos.x, halfPos.y));
                    //wayPoints.Add(new Vector3Int(endPos.Pos.x, halfPos.y));
                }
                else
                {
                    wayPoints.Enqueue(new Vector3Int(halfPos.x, startPos.Pos.y));
                    wayPoints.Enqueue(new Vector3Int(halfPos.x, endPos.Pos.y));
                    //wayPoints.Add(new Vector3Int(halfPos.x, startPos.Pos.y));
                    //wayPoints.Add(new Vector3Int(halfPos.x, endPos.Pos.y));
                }
            }
        }
        else
        {
            // �Ա� �ⱸ�� ���� �϶�
            if (startPos.Direction == ExitDirection.Up || startPos.Direction == ExitDirection.Down)
            {
                wayPoints.Enqueue(new Vector3Int(startPos.Pos.x, endPos.Pos.y));
                //wayPoints.Add(new Vector3Int(startPos.Pos.x, halfPos.y));
            }
            else
            {
                wayPoints.Enqueue(new Vector3Int(endPos.Pos.x, startPos.Pos.y));
                //wayPoints.Add(new Vector3Int(halfPos.x, startPos.Pos.y));
            }
        }


        Vector3Int targetPos = wayPoints.Dequeue();

        ExitDirection targetDir = startPos.Direction;

        PassWayType lastone = PassWayType.UpDown;

        int a = 0;      // ���ѷ��� ������
        do
        {
            if (cursor.x == targetPos.x)        // ���� ���� ��
            {
                int decreasePass = Mathf.Min(Mathf.Abs(cursor.y - targetPos.y), 5);
                lastone = PassWayType.UpDown;
                cursor += GeneratePass(new PassWay(cursor, targetDir), PassWayType.UpDown, 5 - decreasePass);
            }
            else
            {
                int decreasePass = Mathf.Min(Mathf.Abs(cursor.x - targetPos.x), 5);
                lastone = PassWayType.LeftRight;
                cursor += GeneratePass(new PassWay(cursor, targetDir), PassWayType.LeftRight, 5 - decreasePass);
            }
            a++;

            if (cursor == targetPos && targetPos != endPos.Pos)      // �߰� ������ ������
            {
                if (wayPoints.Count > 0) targetPos = wayPoints.Dequeue();     // ���� ������ ���� ��
                else targetPos = endPos.Pos;
                // ���⿡ ���� ������ ���� �ڵ�
                // targetDir�� cursor ��ġ�� targetPos���� ���Ҽ� ������ ����

                PassWayType passWay = PassWayType.UpDown;
                switch (targetDir)
                {
                    case ExitDirection.Up:
                        if (targetPos.x > cursor.x)
                        {
                            passWay = PassWayType.RightDown;
                            lastone = PassWayType.LeftRight;
                        }
                        else
                        {
                            passWay = PassWayType.DownLeft;
                            lastone = PassWayType.UpDown;
                        }
                        break;
                    case ExitDirection.Left:
                        if (targetPos.y > cursor.y)
                        {
                            passWay = PassWayType.UpRight;
                            lastone = PassWayType.UpDown;
                        }
                        else
                        {
                            passWay = PassWayType.RightDown;
                            lastone = PassWayType.LeftRight;
                        }
                        break;
                    case ExitDirection.Right:
                        if (targetPos.y > cursor.y)
                        {
                            passWay = PassWayType.LeftUp;
                            lastone = PassWayType.UpDown;
                        }
                        else
                        {
                            passWay = PassWayType.DownLeft;
                            lastone = PassWayType.LeftRight;
                        }
                        break;
                    case ExitDirection.Down:
                        if (targetPos.x > cursor.x)
                        {
                            passWay = PassWayType.UpRight;
                            lastone = PassWayType.LeftRight;
                        }
                        else
                        {
                            passWay = PassWayType.LeftUp;
                            lastone = PassWayType.UpDown;
                        }
                        break;
                }

                cursor += GeneratePass(new PassWay(cursor, targetDir), passWay);        // Ÿ�ϸ� ����


                if (targetPos.x != cursor.x)
                {
                    if (targetPos.x > cursor.x)
                    {
                        targetDir = ExitDirection.Right;
                    }
                    else
                    {
                        targetDir = ExitDirection.Left;
                    }
                }
                else
                {
                    if (targetPos.y > cursor.y)
                    {
                        targetDir = ExitDirection.Up;
                    }
                    else
                    {
                        targetDir = ExitDirection.Down;
                    }
                }
            }

        } while (cursor != endPos.Pos && a < 100);

        GeneratePass(new PassWay(cursor, targetDir), lastone);        // ������ ��� Ÿ�ϸ� ����


        //cursor += GeneratePass(new Exit(cursor, ExitDirection.Right), PassWayType.LeftRight);
    }

    bool IsBuildable(Vector3Int cursor, SampleRoomData targetData)
    {
        bool result = true;
        for(int x = targetData.min.x; x < targetData.max.x; x++ )
        {
            for(int y = targetData.min.y; y < targetData.max.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y) + cursor;
                if (m_tileMaps[1].GetTile(targetPos) != null)
                {
                    result = false; 
                    return result;
                }
            }
        }
        return result;
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

    Vector3Int GeneratePass(PassWay passPos, PassWayType passWayType, int drawOverCount = 0)
    {
        Vector3Int cursorPos = passPos.Pos;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        // �׷��� �� ������ �� ũ�⺸�� ������(passWaytype�� 0 �Ǵ� 1�ΰ�츸)
        // �� ������ ���� ��ŭ ���� �Ǵ� �ʺ� �ٿ��� �׸���
        Vector3Int decreaseFromMin = Vector3Int.zero;
        Vector3Int decreaseFromMax = Vector3Int.zero;

        if(passWayType == PassWayType.UpDown)
        {
            if(passPos.Direction == ExitDirection.Up) 
            {
                decreaseFromMax.y = drawOverCount;
            }
            else
            {
                decreaseFromMin.y = drawOverCount;
            }
        }
        else if(passWayType == PassWayType.LeftRight)
        {
            if(passPos.Direction == ExitDirection.Right)
            {
                decreaseFromMax.x = drawOverCount;
            }
            else
            {
                decreaseFromMin.x = drawOverCount;
            }
        }


        for (int i = decreaseFromMin.y; i < targetData.Height - decreaseFromMax.y; i++)    // �� ���� ��ŭ
        {
            for (int j = decreaseFromMin.x; j < targetData.Width - decreaseFromMax.x; j++)  // �� �ʺ� ��ŭ
            {
                // ******** ����ִ� ���̶� ���°��̶� ���� �ּ� ���� �������ϴ� �˰���¥�ߵ� �ٽ� ����� ��
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;        // ���� ��ġ ���� ����� �׸� ��ġ

                // ���̾� ����
                int targetLayer = (int) MapLayer.Background;

                // ��� �׸���
                m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[targetLayer].GetTile(targetDrawPos));

                TileBase tempTile;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    if (!m_tileMaps[(int)MapLayer.HalfPlatForm].HasTile(cursorPos + targetDrawPos))
                    {
                        // �÷��� ĭ�� ��(���÷����� ������ ����)
                        targetLayer = (int)MapLayer.PlatForm;
                        tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                        m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    }
                    else
                    {
                        Debug.Log("���÷��� Ÿ���� ����");
                    }
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // ���÷��� ĭ�� �� ������ �÷��� ĭ ������ ����
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);

                    if (m_tileMaps[(int)MapLayer.PlatForm].HasTile(targetDrawPos))
                    {
                        m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, null);
                    }

                }
                else
                {
                    // ��ĭ�� �� ������ ��ġ�� �÷����� ���÷��� �����
                    tempTile = null;
                    targetLayer = (int)MapLayer.PlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                }

                // �÷��� �׸���
                // m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
            }
        }

        // ���� Ŀ�� ��ġ�� ����ϱ����� �������� �� ��ġ ���� ��ȯ�ϴ� ����
        Vector3Int result = Vector3Int.zero;

        switch (passWayType)
        {
            case PassWayType.UpDown:
                if (passPos.Direction == ExitDirection.Up)
                {
                    result.y = targetData.Height;
                }
                else
                {
                    result.y = -targetData.Height;
                }
                break;
            case PassWayType.LeftRight:
                if (passPos.Direction == ExitDirection.Right) result.x = targetData.Width;
                else result.x = -targetData.Width;
                break;
            case PassWayType.UpRight:
                if (passPos.Direction == ExitDirection.Down) result.x = targetData.Width;
                else result.y = targetData.Height;
                break;
            case PassWayType.RightDown:
                if (passPos.Direction == ExitDirection.Left) result.y = -targetData.Height;
                else result.x = targetData.Width;
                break;
            case PassWayType.DownLeft:
                if (passPos.Direction == ExitDirection.Right) result.y = -targetData.Height;
                else result.x = -targetData.Width;
                break;
            case PassWayType.LeftUp:
                if (passPos.Direction == ExitDirection.Right) result.y = targetData.Height;
                else result.x = -targetData.Width;
                break;
        }

        // ��θ� ª�� �׸���ŭ ���� �׸� ������ ��ġ �ݿ�
        //decreaseFromMin;
        result -= decreaseFromMax;
        //decreaseFromMax;
        result += decreaseFromMin;

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
        foreach(PassWay temp in targetRoomData.exitPos)
        {
            if (temp.Direction != exitDir) continue;            // ���� ������ ���Ա��� ���� �Ķ���� ����� ���� ������ ��ŵ
            int x = 0, y = 0;   // ������ �¿쿡 ���� ���Ա� �׷����� ������ġ
            int index = 0;      // 0�� �¿� ���Ա�, 1�� ���� ���Ա�
            if(temp.Direction == ExitDirection.Left || temp.Direction == ExitDirection.Right)
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

    public class TestRoom
    {
        int getSampleIndex;
        public int GetSampleIndex
        {
            get => getSampleIndex;
            set
            {
                getSampleIndex = value;
                passWays = RoomGenerator.Ins.roomSamplesWithExit[getSampleIndex].exitPos.ToArray();
            }
        }

        public PassWay[] passWays;

        public Vector2Int gridCoord;

        public Vector3Int origineCoord;

        public void SetOrigineCoord(int roomSize)
        {
            origineCoord = new Vector3Int(gridCoord.x * roomSize, gridCoord.y * roomSize);
            for(int i =0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }
    }

    public void Test_SetOrigine(SampleRoomData data)
    {
        //Vector2Int mapSize = new Vector2Int(roomSamplesWithExit[getSampleIndex]);
    }

    public void Test()
    {
        TestRoom test1 = new TestRoom();
        test1.gridCoord = new Vector2Int(0, 0);
        test1.GetSampleIndex = 0;
        test1.SetOrigineCoord(maxSingleRoomSize);
        
        TestRoom test2 = new TestRoom();
        test2.gridCoord = new Vector2Int(1, 1);
        test2.GetSampleIndex = 1;
        test2.SetOrigineCoord(maxSingleRoomSize);

        TestRoom test3 = new TestRoom();
        test3.gridCoord = new Vector2Int(2, 0);
        test3.GetSampleIndex = 2;
        test3.SetOrigineCoord(maxSingleRoomSize);

        TestRoom test4 = new TestRoom();
        test4.gridCoord = new Vector2Int(2, 1);
        test4.GetSampleIndex = 2;
        test4.SetOrigineCoord(maxSingleRoomSize);

        GenerateRoom(test1.origineCoord, roomSamplesWithExit[test1.GetSampleIndex]);
        GenerateRoom(test2.origineCoord, roomSamplesWithExit[test2.GetSampleIndex]);
        GenerateRoom(test3.origineCoord, roomSamplesWithExit[test3.GetSampleIndex]);
        GenerateRoom(test4.origineCoord, roomSamplesWithExit[test4.GetSampleIndex]);

        Test_ConnectPassway(test1, test2);
        Test_ConnectPassway(test3, test2);
        Test_ConnectPassway(test2, test4);
    }

    public void Test_ConnectPassway(TestRoom temp1, TestRoom temp2)
    {
        float distance = float.MaxValue;
        PassWay one = temp1.passWays[0];
        PassWay two = temp2.passWays[0];

        foreach(var exit1 in temp1.passWays)
        {
            if (exit1.isConnected) continue;    // �̹� ���� �Ǿ� ������ �н�
            foreach(var exit2 in temp2.passWays)
            {
                if (exit2.isConnected) continue;
                float tempDistance = Vector3Int.Distance(exit1.Pos + temp1.origineCoord, exit2.Pos + temp2.origineCoord);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    one = exit1;
                    two = exit2;
                }
            }
        }

        GeneratePassway(one, two);
    }
}

