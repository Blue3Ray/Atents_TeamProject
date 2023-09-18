using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;


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
    /// �� ������ �ҷ��� ���õ�
    /// </summary>
    public SampleRoomData[] roomSamplesWithExit;

    /// <summary>
    /// ���� ��
    /// </summary>
    public SampleRoomData startRoom;

    /// <summary>
    /// Ÿ���� �׸� �ʵ�(0 ���, 1 �÷���, 2 ���÷���, ... n-1 ���Ա�)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
    /// </summary>
    Vector3Int cursor;

    // ��� �κ� ------------------
    /// <summary>
    /// �׸��� �� �����ϴ� Ŭ����
    /// </summary>
    RandomMapGenerator randomMap;

    /// <summary>
    /// �׷��� �� ������ ������ �ִ� ����Ʈ
    /// </summary>
    List<MakeRoom> makeRooms;

    // �ʱ� ���� �� ���� ---------------------
    
    public int width = 100;

    public int height = 100;

    public float fillRate = 0.46f;

    public int collecBoxBoolCount = 3;

    public int roomCount = 8;

    public int roomGap = 10;

    // ���� �� ���� ----------------------

    /// <summary>
    /// ���� ���� ���� ū �� ���� ũ��(���簢�� �Ѻ��� ����)
    /// </summary>
    int maxSingleRoomSize = 0;

    // �̱����̱� ������ ������Ʈ ã�°� start���� ������
    private void Start()
    {
        // Ÿ���� �׸� ���̾� �ҷ����� ����
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        randomMap = new RandomMapGenerator();

        cursor = new Vector3Int(0, 0);


        // �� ���� �����ϴ� ����
        // ���� ��
        startRoom.OnInitialize();

        // ���� ��
        foreach(SampleRoomData sampleRoom in roomSamplesWithExit)
        {
            int t = sampleRoom.OnInitialize();
            if (t > maxSingleRoomSize) maxSingleRoomSize = t;
        }

        // ���
        foreach (SampleRoomData passWay in passWaySamples)
        {
            passWay.OnInitialize();
        }

        // ��� �� ���� �Ÿ� �߰��ؼ� �� ũ���� ���� �α�
        maxSingleRoomSize += roomGap;
    }

    /*
    �� �����ϴ� �˰��� ����
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

    /// <summary>
    /// ���� �����ϴ� �Լ�
    /// </summary>
    public void SetUpRooms()
    {
        randomMap.SetUp(roomCount, width, height, fillRate, collecBoxBoolCount);        // �� ���� ����

        makeRooms = new();      // �� ����Ʈ �ʱ�ȭ

        // ���� �� ����(���� ���� ������� �׸��� �ʿ��� ���� ������ �ִ� ���� ������ ��ġ�� �ȴ�
        Vector2Int startRoomGrid = randomMap.roomList[0].gridCoord + new Vector2Int(-1, 0);

        MakeRoom startMakeRoom = new MakeRoom();
        startMakeRoom.GetRoomData(startRoomGrid);
        startMakeRoom.GetSampleIndex = -1;
        startMakeRoom.SetOrigineCoord(new Vector3Int(startRoom.Width, startRoom.Height), maxSingleRoomSize);

        GenerateRoom(startMakeRoom.origineCoord, startRoom);

        makeRooms.Add(startMakeRoom);



        // �� �����ϰ� ����Ʈ�� ����ϴ� ����
        foreach (var item in randomMap.roomList)
        {
            MakeRoom targetRoom = new MakeRoom();
            targetRoom.GetRoomData(item);
            targetRoom.GetSampleIndex = GetRandomRoom(item);
            targetRoom.SetOrigineCoord(maxSingleRoomSize);

            GenerateRoom(targetRoom.origineCoord, roomSamplesWithExit[targetRoom.GetSampleIndex]);

            makeRooms.Add(targetRoom);
        }


        // ������� ���� ��� ���� ���� Ȯ�� �Ŀ� ��� �����ϴ� ����
        foreach (var roomOne in makeRooms)
        {
            Room targetOne = roomOne.roomData;
            foreach (var roomTwo in targetOne.connectedRooms)
            {
                if(!targetOne.IsConnectedBuildRoom(roomTwo.Item1))
                {
                    targetOne.alreadyConnectPassWayRooms.Add(roomTwo.Item1);
                    roomTwo.Item1.alreadyConnectPassWayRooms.Add(targetOne);

                    if(TryGetMakeRoomByCoord(roomTwo.Item1.gridCoord, out MakeRoom result))
                    {
                        ConnectPassway(roomOne, result);
                    }
                }
            }
        }

        // ���� ��� ù���� ������(�׸��� �� ������ ���۹� ������ ���� ������ �������� ���� ����)
        ConnectPassway(makeRooms[0], makeRooms[1]);
    }

    /// <summary>
    /// �ⱸ ������ �����ϴ� ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="targetRoom">������ ��</param>
    /// <returns>���ǿ� �´� ���� ��</returns>
    int GetRandomRoom(Room targetRoom)
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
        return Random.Range(0, canBuildRoomList.Count);
    }

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
    }

    /// <summary>
    /// �ΰ��� ���� ���� �Ҷ� ��밡���� ���Ա��� Ȯ���ؼ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="temp1">������ ù��° ��</param>
    /// <param name="temp2">������ �ι�° ��</param>
    public void ConnectPassway(MakeRoom temp1, MakeRoom temp2)
    {
        float distance = float.MaxValue;
        PassWay one = temp1.passWays[0];
        PassWay two = temp2.passWays[0];

        // �� �ⱸ���� �ϳ� �� ������ �ִ� �Ÿ����� ��
        foreach (var exit1 in temp1.passWays)
        {
            if (exit1.isConnected) continue;    // �̹� ���� �Ǿ� ������ �н�
            foreach (var exit2 in temp2.passWays)
            {
                if (exit2.isConnected) continue; // �̹� ���� �Ǿ� ������ �н�

                float tempDistance = Vector3Int.Distance(exit1.Pos + temp1.origineCoord, exit2.Pos + temp2.origineCoord);
                if (tempDistance < distance)    // ���� ������ �Ÿ����� ª���� �ʱ�ȭ
                {
                    distance = tempDistance;
                    one = exit1;
                    two = exit2;
                }
            }
        }

        GeneratePassway(one, two);  // ��� ����
        one.isConnected = true;
        two.isConnected = true;
    }

    /// <summary>
    /// �ش� �׸��� ��ǥ�� �ִ� makeRoom �����Ͱ� �ִ��� Ȯ���ϰ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="gridPos">Ȯ���� �׸��� ��ǥ</param>
    /// <param name="resultRoom">��� ��</param>
    /// <returns>�ش� �׸��� ��ǥ�� �� ������ ������ ��, ������ ����</returns>
    bool TryGetMakeRoomByCoord(Vector2Int gridPos, out MakeRoom resultRoom)
    {
        bool result = false;
        resultRoom = null;

        foreach (var item in makeRooms)
        {
            if (item.gridCoord == gridPos)      // ����Ʈ �� �ش� �� ��ǥ�� ã�� ��ǥ�̸�
            {
                resultRoom = item;
                result = true;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// ��θ� �Ѱ��� �׸��� �Լ�
    /// </summary>
    /// <param name="passPos">��ġ, ���� ����</param>
    /// <param name="passWayType">�׷��� �� ���� ����</param>
    /// <param name="drawOverCount">�׷��� �� ĭ�� ��� ũ�⺸�� ���� �� ��ġ�� �� ��</param>
    /// <returns>��� �Ѱ��� �׸��� ���� ��ȯ�� ���� ��ġ</returns>
    Vector3Int GeneratePass(PassWay passPos, PassWayType passWayType, int drawOverCount = 0)
    {
        Vector3Int cursorPos = passPos.Pos;

        SampleRoomData targetData = passWaySamples[(int) passWayType];

        // �׷��� �� ������ �� ũ�⺸�� ������(passWaytype�� 0 �Ǵ� 1�ΰ�츸)
        // �� ������ ���� ��ŭ ���� �Ǵ� �ʺ� �ٿ��� �׸���
        Vector3Int decreaseFromMin = Vector3Int.zero;
        Vector3Int decreaseFromMax = Vector3Int.zero;

        // �׷��� �� ���⿡ ���� ���̴� ���� ���ϱ�
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

        // ��� �׸��� �κ�
        for (int i = decreaseFromMin.y; i < targetData.Height - decreaseFromMax.y; i++)    // �� ���� ��ŭ
        {
            for (int j = decreaseFromMin.x; j < targetData.Width - decreaseFromMax.x; j++)  // �� �ʺ� ��ŭ
            {
                // ******** ����ִ� ���̶� ���°��̶� ���� �ּ� ���� �������ϴ� �˰���¥�ߵ� �ٽ� ����� ��
                Vector3Int plusPos = new Vector3Int(j, i);
                Vector3Int targetDrawPos = targetData.min + plusPos;        // ���� ��ġ ���� ����� �׸� ��ġ

                // ���̾� ����
                int targetLayer = (int) MapLayer.Background;

                TileBase tempTile;

                if (targetData.mapLayers[(int) MapLayer.PlatForm].HasTile(targetDrawPos))
                {
                    if (!m_tileMaps[(int)MapLayer.HalfPlatForm].HasTile(cursorPos + targetDrawPos) && !m_tileMaps[(int)MapLayer.Background].HasTile(cursorPos + targetDrawPos))
                    {
                        // �÷��� ĭ�� ��(���÷����̰ų� �̹� ��� Ÿ���������� ����)
                        targetLayer = (int)MapLayer.PlatForm;
                        tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                        m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                    }
                    else
                    {
                        //Debug.Log("���÷��� Ÿ���� ����");
                    }
                }
                else if (targetData.mapLayers[(int)MapLayer.HalfPlatForm].HasTile(targetDrawPos))
                {
                    // ���÷��� ĭ�� �� ������ �÷��� ĭ ������ ����
                    targetLayer = (int)MapLayer.HalfPlatForm;
                    tempTile = targetData.mapLayers[targetLayer].GetTile(targetDrawPos);
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);

                    if (m_tileMaps[(int)MapLayer.PlatForm].HasTile(cursorPos + targetDrawPos))
                    {
                        m_tileMaps[(int)MapLayer.PlatForm].SetTile(cursorPos + targetDrawPos, null);
                    }
                }
                else
                {
                    // �÷����� ���÷����� ���� ��
                    // ��ĭ�� �� ������ ��ġ�� �÷����� ���÷��� �����
                    tempTile = null;
                    targetLayer = (int)MapLayer.PlatForm;
                    m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, tempTile);
                }

                targetLayer = (int)MapLayer.Background;

                // ��� �׸���
                m_tileMaps[targetLayer].SetTile(cursorPos + targetDrawPos, targetData.mapLayers[targetLayer].GetTile(targetDrawPos));
            }
        }

        // ���� Ŀ�� ��ġ�� ����ϱ����� �������� �� ��ġ ���� ��ȯ�ϴ� ����
        Vector3Int result = Vector3Int.zero;

        // ���� �׷��� ���⿡ ���� �ٸ� ��ġ ����
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
        result -= decreaseFromMax;
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
        for(int i = 0; i < targetRoomData.tilesPos.Count - 1; i++)          // ���̾� ���� ����(���������� �ⱸ ���̾�� ǥ�þ���
        {
            List<Vector3Int> poses = targetRoomData.tilesPos[i];
            foreach (Vector3Int pos in poses)                       // ���̾ �ִ� Ÿ�ϵ�
            {
                m_tileMaps[i].SetTile(pos + cursor, targetRoomData.mapLayers[i].GetTile(pos));
            }
        }
    }

    /// <summary>
    /// �׷��� �� ���� Ŭ����
    /// </summary>
    public class MakeRoom
    {
        int getSampleIndex = -1;

        /// <summary>
        /// �濡�ٰ� ��ġ�� ���� �� �ε���, ������ �Ǹ� �ش� ���� �ⱸ ������ �ڵ����� �����´�
        /// </summary>
        public int GetSampleIndex
        {
            get => getSampleIndex;
            set
            {
                getSampleIndex = value;
                if (getSampleIndex != -1)           // ���� ���� �� -1
                {
                    passWays = RoomGenerator.Ins.roomSamplesWithExit[getSampleIndex].exitPos.ToArray();
                }
                else
                {
                    // ���� ���� ��
                    passWays = RoomGenerator.Ins.startRoom.exitPos.ToArray();
                }
            }
        }

        /// <summary>
        /// �ⱸ ����
        /// </summary>
        public PassWay[] passWays;

        /// <summary>
        /// �� �׸��� ��ǥ(�� ��ĭ �� �ϳ�)
        /// </summary>
        public Vector2Int gridCoord;

        /// <summary>
        /// �ش� ���� ������ ���� Room������
        /// </summary>
        public Room roomData;

        /// <summary>
        /// �� ������ ���� ��ġ��(�׸��� ��ǥ�� �� �⺻ ũ�⿡ ���� �ٲ�)
        /// </summary>
        public Vector3Int origineCoord;

        public void GetRoomData(Room room)
        {
            gridCoord = room.gridCoord;
            roomData = room;
        }

        public void GetRoomData(Vector2Int pos)
        {
            gridCoord = pos;
            roomData = new Room(pos);
        }

        public void SetOrigineCoord(int roomSize)
        {
            origineCoord = new Vector3Int(gridCoord.x * roomSize, gridCoord.y * roomSize);
            for(int i =0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }

        // ���� ���� ���� �Լ�
        public void SetOrigineCoord(Vector3Int startSize, int roomSize)
        {
            int startRoomGab = 20;
            // ���ľߵ� !!
            // Debug.LogWarning("���� �ʿ�"); ���� �߸� ����
            origineCoord = new Vector3Int((gridCoord.x + 1) * roomSize - startSize.x - startRoomGab, gridCoord.y * roomSize);
            for (int i = 0; i < passWays.Length; i++)
            {
                passWays[i].Pos += origineCoord;
            }
        }
    }

    //public void Test()
    //{
    //    MakeRoom test1 = new MakeRoom();
    //    test1.gridCoord = new Vector2Int(0, 0);
    //    test1.GetSampleIndex = 0;
    //    test1.SetOrigineCoord(maxSingleRoomSize);
        
    //    MakeRoom test2 = new MakeRoom();
    //    test2.gridCoord = new Vector2Int(1, 1);
    //    test2.GetSampleIndex = 1;
    //    test2.SetOrigineCoord(maxSingleRoomSize);

    //    MakeRoom test3 = new MakeRoom();
    //    test3.gridCoord = new Vector2Int(2, 0);
    //    test3.GetSampleIndex = 2;
    //    test3.SetOrigineCoord(maxSingleRoomSize);

    //    MakeRoom test4 = new MakeRoom();
    //    test4.gridCoord = new Vector2Int(2, 1);
    //    test4.GetSampleIndex = 2;
    //    test4.SetOrigineCoord(maxSingleRoomSize);

    //    GenerateRoom(test1.origineCoord, roomSamplesWithExit[test1.GetSampleIndex]);
    //    GenerateRoom(test2.origineCoord, roomSamplesWithExit[test2.GetSampleIndex]);
    //    GenerateRoom(test3.origineCoord, roomSamplesWithExit[test3.GetSampleIndex]);
    //    GenerateRoom(test4.origineCoord, roomSamplesWithExit[test4.GetSampleIndex]);

    //    //Test_ConnectPassway(test1, test2);
    //    ConnectPassway(test2, test4);
    //    ConnectPassway(test3, test2);
    //}

    
}

