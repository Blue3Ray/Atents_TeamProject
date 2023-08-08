using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomMap;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OldRoom
{
    public List<Node> nodes = new List<Node>();
    public List<OldRoom> connectedRooms = new List<OldRoom>();
    public List<(OldRoom, ExitDirection)> connectedExit = new();

    public int index;

    public bool isAccessibleMainRoom;
    public bool isMainRoom;
    public bool isBuilt = false;

    public float CenterX => (maxX + minX) * 0.5f;
    public float CenterY => (maxY + minY) * 0.5f;

    // �� ��ġ�� ���� �ⱸ ��ġ�� ������ �����ϱ� ���� ����
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;

    public void SetXYData()
    {
        minX = minY = int.MaxValue;
        maxX = maxY = -1;
        if (nodes.Count <= 1)
        {
            minX = maxX = nodes[0].gridPos.x;
            minY = maxY = nodes[0].gridPos.y;
        }

        foreach (Node item in nodes)
        {
            if (item.gridPos.x < minX)
            {
                minX = item.gridPos.x;
            }
            else if (item.gridPos.x > maxX)
            {
                maxX = item.gridPos.x;
            }

            if (item.gridPos.y < minY)
            {
                minY = item.gridPos.y;
            }
            else if (item.gridPos.y > maxY)
            {
                maxY = item.gridPos.y;
            }
        }
    }

    public void ClearNodes()
    {
        foreach (Node node in nodes)
        {
            node.data = false;
        }
    }

    /// <summary>
    /// MainRoom�� ����� �ڽŰ� ����Ǿ� �ִ� �ٸ� ��鵵 ���� �Ӽ� �ο�
    /// </summary>
    public void SetAccessibleMainRoom()
    {
        if (!isAccessibleMainRoom)
        {
            isAccessibleMainRoom = true;
            foreach (OldRoom room in connectedRooms)
            {
                if (!room.isAccessibleMainRoom)
                {
                    room.SetAccessibleMainRoom();           // �ش� ��� ����Ǿ� �ִ� ��鵵 ���η�� ���� �������� �ٲ�
                }
            }
        }
    }

    public static void ConnectRooms(OldRoom rA, OldRoom rB)
    {
        // ������ �� �� �� �ϳ��� ���� ��� ������ �Ǿ� ������ �Ѵ� ���� ����
        if (rA.isAccessibleMainRoom)
        {
            rB.SetAccessibleMainRoom();
        }
        else if (rB.isAccessibleMainRoom)
        {
            rA.SetAccessibleMainRoom();
        }

        rA.connectedRooms.Add(rB);
        rB.connectedRooms.Add(rA);
    }

    /// <summary>
    /// target��� ���� �Ǿ� �ִ��� Ȯ���ϴ� �Լ�(������ Ȯ��)
    /// </summary>
    /// <param name="targetRoom">Ȯ���� ��</param>
    /// <returns>���̸� ���� �Ǿ� ����, �����̸� ���� �ȵǾ� ����</returns>
    public bool IsConnected(OldRoom targetRoom)
    {
        return connectedRooms.Contains(targetRoom);
    }

    /// <summary>
    /// �ڱ�� ����Ǿ� �ִ� �濡 ���ؼ� ��ġ �����ϱ�(Center ��������)
    /// </summary>
    /// <param name="width">���� �� ����(RandomMap ��� ����)</param>
    /// <param name="height">���� �� ����(RandomMap ��� ����</param>
    public void SetExitList(ref int width, ref int height)
    {
        ExitDirection exitDir = ExitDirection.Up;
        foreach (OldRoom room in connectedRooms)
        {
            if(room.CenterY > CenterY)      // ���� ���ʿ� �ְ�
            {
                if(room.CenterY - CenterY > Mathf.Abs(room.CenterX - CenterX))     // ���� ���� �� �������� X�ຸ�� Y�� ������ �ö� ���� ��
                {
                    exitDir = ExitDirection.Up;
                    height++;
                }
                else
                {
                    if (room.CenterX > CenterX)
                    {
                        exitDir = ExitDirection.Right;
                    }
                    else
                    {
                        exitDir = ExitDirection.Left;
                    }
                    width++;
                }
            }
            else                            // ���� �Ʒ��ʿ� �ְ�
            {
                if (CenterY - room.CenterY > Mathf.Abs(room.CenterX - CenterX))     // ���� ���� �� �������� Y���� X�ຸ�� �� �Ʒ� �ʿ� ���� ��
                {
                    exitDir = ExitDirection.Down;
                    height++;
                }
                else
                {
                    if (room.CenterX > CenterX)
                    {
                        exitDir = ExitDirection.Right;
                    }
                    else
                    {
                        exitDir = ExitDirection.Left;
                    }
                    width++;
                }
            }

            connectedExit.Add((room, exitDir));         // ����Ʈ ����(����Ǿ� �ִ� ��� index�� ���ƾߵ�)��� ����
        }
    }
}

// ���⼭���� ��� ����----------------------------------------------------------------------------------------------
// ���⼭���� ��� ����----------------------------------------------------------------------------------------------

public class RandomMap
{
    /// <summary>
    /// �� ������ �� ����� Node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// ��尡 ��ĭ���� �ƴ��� �Ǻ��ϴ� bool
        /// </summary>
        public bool data;

        /// <summary>
        /// ��尡 RoomList ������ �� �̹� �˻縦 �ߴ��� ���ߴ��� ����
        /// </summary>
        public bool isChecked;

        /// <summary>
        /// ��� ��ġ
        /// </summary>
        public Vector2Int gridPos;

        /// <summary>
        /// ��� Ŭ���� �ʱ�ȭ
        /// </summary>
        /// <param name="data">���ϰ�� ��ĭ, ������ ��� ��ĭ �ƴ�</param>
        /// <param name="pos">��� ��ġ</param>
        /// <param name="isChecked">�ʱ� ���� false</param>
        public Node(bool data, Vector2Int pos, bool isChecked = false)
        {
            this.data = data;
            this.gridPos = pos;
            this.isChecked = isChecked;
        }
    }

    public int width;
    public int Width
    {
        get => width;
        set 
        { 
            width = value; 
        }
    }
    public int height;
    public int Height
    {
        get => height;
        set 
        { 
            height = value;
        }
    }

    public int widthCount = 0;
    public int heightCount = 0;

    /// <summary>
    /// ����
    /// </summary>
    public Node[] mapNodes;
    
    /// <summary>
    /// �ʱ� ���� bool ä��� ���� (���� 0.45 ~ 0.47 ����)
    /// </summary>
    [Range(0,1)]
    public float mapFillRate = 0.46f;

    /// <summary>
    /// ����ȭ ��Ű�� Ƚ��
    /// </summary>
    public int collectBoxBoolCount = 3;

    /// <summary>
    /// Room(Node�� ����Ʈ)���� ������ �ִ� ����Ʈ
    /// </summary>
    public List<OldRoom> roomList = new List<OldRoom>();

    /// <summary>
    /// �� ��ġ�� ���� ��ų �׸���
    /// </summary>
    int[,] mapGrid;

    /// <summary>
    /// �׽�Ʈ �� �ӽ� ������(��¬�� �⺻���� �־ �ֱ� �����Ƽ� ����)
    /// </summary>
    public RandomMap()
    {

    }


    public RandomMap(int width, int height, float fillRate = 0.46f, int collecBoxBoolCount = 3)
    {
        Width = width;
        Height = height;
        mapFillRate = fillRate;
        collectBoxBoolCount = collecBoxBoolCount;
    }

    /// <summary>
    /// �׽�Ʈ�� ���� ����
    /// </summary>
    public void SetUpNodes()
    {
        RandomFillNodeInMap(Width, Height, mapFillRate);
    
        GatherData(collectBoxBoolCount);

        roomList = new List<OldRoom>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);
                OldRoom tempRoom = new();
                tempRoom.nodes = nodeList;
                tempRoom.SetXYData();      // room Ŭ������ �ּ� ��ǥ�� �ִ� ��ǥ���� ����ȭ �Ѵ�.

                if (tempRoom != null)       // ���������� �˻縦 ��ġ��
                {
                    roomList.Add(tempRoom);     // ����Ʈ�� �߰�
                    //Debug.Log($"room list : {roomList.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                    //    $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
                }
            }
        }
    }

    /// <summary>
    /// RoomList�� roomCount ������ ���� �����ϰ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="roomCount">������ �� ����</param>
    public void StartMapData(uint roomCount)
    {
        RandomFillNodeInMap(Width, Height, mapFillRate);

        GatherData(collectBoxBoolCount);

        roomList = new List<OldRoom>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);
                

                if (nodeList != null)       // ���������� �˻縦 ��ġ��
                {
                    OldRoom tempRoom = new();

                    tempRoom.nodes = nodeList;
                    tempRoom.SetXYData();      // room Ŭ������ �ּ� ��ǥ�� �ִ� ��ǥ���� ����ȭ �Ѵ�.

                    roomList.Add(tempRoom);     // ����Ʈ�� �߰�
                    //Debug.Log($"room list : {roomList.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                    //    $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
                }
            }
        }

        LimitRoomCount(roomCount);

        roomList.Sort((x, y) => x.CenterX < y.CenterX ? -1 : 1);      // ���� ���ʺ��� ���������� ����(���� �� ���� �ȵ�)

        roomList[0].isMainRoom = true;                          // ���� ���ʿ� �ִ� ���� ���� ������ ����(���� ��)
        roomList[0].isAccessibleMainRoom = true;

        ConnectNearRoom(roomList);              // �� ���� �ϱ�

        widthCount = 0;
        heightCount = 0;

        SortByAccessMainRoom(ref roomList);

        CheckExitDir(roomList);

        mapGrid = new int[widthCount, heightCount];
        Vector2Int targetGrid = Vector2Int.zero;
        Vector2Int stand = Vector2Int.zero;

        // ��� Grid�� -1���� �ʱ�ȭ
        for (int i = 0; i < widthCount; i++)
        {
            for (int j = 0; j < heightCount; j++)
            {
                mapGrid[i, j] = -1;
            }
        }

        //SetGridMap(mapGrid, targetGrid, stand);

        foreach (OldRoom tempRoom in roomList)     // ����׿�
        {
            //Debug.Log($"{tempRoom.connectedRooms.Count}");
            foreach (OldRoom tempBRoom in tempRoom.connectedRooms)
            {
                Debug.DrawLine(new Vector3(tempRoom.CenterX, tempRoom.CenterY), new Vector3(tempBRoom.CenterX, tempBRoom.CenterY), Color.red, 8f);
            }
        }
    }

    void SortByAccessMainRoom(ref List<OldRoom> rooms)
    {
        List<OldRoom> sortList = new();
        sortList.Add(rooms[0]);
        for(int i =0; i < rooms.Count; i++)
        {
            OldRoom target = rooms[i];
            for(int j = 0; j < target.connectedExit.Count; j++)
            {
                if (!sortList.Contains(target.connectedExit[j].Item1))
                {
                    sortList.Add(target.connectedExit[j].Item1);
                }
            }
        }

        rooms = sortList;
    }

    void SetGridMap(int[,] mapGrid, Vector2Int targetGrid, Vector2Int stand)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            mapGrid[targetGrid.x + stand.x, targetGrid.y + stand.y] = roomList[0].index;        // �� index
            for (int j = 0; j < roomList[i].connectedExit.Count; j++)       // ��� ����� ����� �˻�
            {
                switch (roomList[i].connectedExit[j].Item2)       // ����� ����� �ϳ��� ������
                {
                    case ExitDirection.Up:
                        targetGrid.x++;
                        break;
                    case ExitDirection.Right:
                        targetGrid.y++;
                        break;
                    case ExitDirection.Down:
                        // �׸��� ��ü�� �ö󰡾ߵ�
                        if (targetGrid.y == 0) MoveAllGridCell(true);

                        break;
                    case ExitDirection.Left:
                        // �׸��� ��ü�� ���������� �̵��ؾ� ��
                        if (targetGrid.x == 0) MoveAllGridCell(false);

                        break;
                }
            }
        }
    }

    class GridTest
    {
        public Vector2Int gridPos;
        public GridTest parent;
        public List<(GridTest, ExitDirection)> connectedGrid;
        public bool isAcessMain = false;

        public int index;

        public void Connect(GridTest test)
        {
            test.connectedGrid.Add((this, ExitDirection.Right));
            this.connectedGrid.Add((test, ExitDirection.Left));
            if(test.isAcessMain || this.isAcessMain)
            {
                test.isAcessMain = true;
                this.isAcessMain = true;
            }
        }
    }

    void Test()
    {
        GridTest[] romss = new GridTest[10];

        GridTest rom = new();
        rom.parent = null;
        rom.isAcessMain = true;

        romss[0] = rom;
        
        for(int i = 1; i < 10; i ++)
        {
            GridTest temp = new();
            temp.parent = romss[i - 1];
            romss[i - 1].Connect(temp);
        }
    }

    /// <summary>
    /// Grid���� ��� ��ǥ�� 1�� �����̴� �Լ�
    /// </summary>
    /// <param name="isY">Y�� �������� ����</param>
    void MoveAllGridCell(bool isY)
    {
        for(int y = 0; y < mapGrid.GetLength(0);y++)
        {
            for (int x = 0; x < mapGrid.GetLength(1); x++)
            {
                if (mapGrid[x, y] != -1)
                {
                    int temp = mapGrid[x, y];
                    if (isY)
                    {
                        mapGrid[x, y + 1] = temp;
                        mapGrid[x, y] = -1;
                    }
                    else
                    {
                        mapGrid[x + 1 , y] = temp;
                        mapGrid[x, y] = -1;
                    }
                }
            }
        }
    }



    /// <summary>
    /// ����Ʈ�� �ִ� �濡 ���ؼ� ����Ǿ� �ִ� ��鿡 ���� ������ ������, �߰��� ���⿡ ���� ���� ���� �� Count�� �����
    /// </summary>
    /// <param name="rooms">Ȯ���� room ����Ʈ��</param>
    public void CheckExitDir(List<OldRoom> rooms)
    {
        int i = 0;
        foreach(OldRoom room in rooms)
        {
            Debug.Log($"{i}��° ��");
            room.SetExitList(ref widthCount, ref heightCount);
            i++;
        }
    }

    /// <summary>
    /// ���η� ����(0��°�� ���� ���ʿ� �ִ� ���η�) ���� ����� ��� �����ϱ�(�����̸� �ϴ� ��ó 1����, ���̸� ���η�� ����Ȯ�� �� ����)
    /// </summary>
    public void ConnectNearRoom(List<OldRoom> allRooms, bool onceChecked = false)
    {
        OldRoom tempA = new();
        OldRoom tempB = new();

        List<OldRoom> roomListA = new();
        List<OldRoom> roomListB = new();

        bool isPossible = false;
        float nearDistance = float.MaxValue;

        if (onceChecked)     // �̹� �ѹ� üũ �Ѱ� Ȯ��
        {
            //foreach (Room temp in allRooms)       // bool isMainRoom���� Ȯ�� ���� ����
            //{
            //    if (mainRoom == null)
            //    {
            //        mainRoom = temp;
            //        continue;
            //    }

            //    if (mainRoom.minX > temp.minX)
            //    {
            //        mainRoom = temp;
            //    }
            //}

            foreach (OldRoom temp in allRooms)      // ��� ���� �˻��ϸ鼭 
            {
                if (temp.isAccessibleMainRoom)         // ���� ���̶� ���� �Ǿ� �ִ��� Ȯ�� �� ���� �Ǿ� ������ A����Ʈ �ƴϸ� B����Ʈ
                {
                    roomListA.Add(temp);
                }
                else
                {
                    roomListB.Add(temp);
                }
            }
        }
        else            // ó�� ���°Ÿ� A,B�� ��� ����
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        foreach (OldRoom roomA in roomListA)
        {
            if (!onceChecked) 
            {
                nearDistance = float.MaxValue;              // �⺻�� ����

                if (roomA.connectedRooms.Count > 0)         // ���� ���� ���� �Ǿ� ������
                continue;                                   // �н��ϱ�
            }

            foreach(OldRoom roomB in roomListB)
            {
                if (roomA == roomB) continue;   // A�� B�� ���� ���̸� ��ŵ(�ڱ��ڽ�)
                
                float distance = Mathf.Pow(roomA.CenterX - roomB.CenterX,2) + Mathf.Pow(roomA.CenterY - roomB.CenterY, 2);

                if(distance < nearDistance)         // �� ���� ���� ��ϵǾ� �ִ� �ִ� �Ÿ����� ������
                {
                    nearDistance = distance;
                    isPossible = true;
                    tempA = roomA;
                    tempB = roomB;
                }
            }

            if (tempA.IsConnected(tempB))       // ���� ����� �� ���� �̹� ���� ����Ǿ� ������ �Ѿ��
            {
                continue;
            }
            else if(isPossible && !onceChecked)                 // �� ���� ���� �����ϸ� �����ϱ�, ó�� ��� ���� ���� ��
            {
                OldRoom.ConnectRooms(tempA, tempB);
            }
        }

        if(isPossible && onceChecked)           // ���� �ٸ� Room����Ʈ���� ����� ���� �����ϱ�
        {
            OldRoom.ConnectRooms(tempA, tempB);
        }

        bool isAllRoomConnectedWithMainRoom = true;
        foreach(OldRoom temp in allRooms)
        {
            if(!temp.isAccessibleMainRoom)
            {
                isAllRoomConnectedWithMainRoom = false;
            }
        }

        if(!onceChecked)
        {         
            // ��� �� ����� �� �׸��� �����
            foreach (OldRoom tempRoom in allRooms)
            {
                //Debug.Log($"{tempRoom.connectedRooms.Count}");
                foreach (OldRoom tempBRoom in tempRoom.connectedRooms)
                {
                    Debug.DrawLine(new Vector3(tempRoom.CenterX - 1, tempRoom.CenterY - 1), new Vector3(tempBRoom.CenterX - 1, tempBRoom.CenterY - 1), Color.blue, 8f);
                }
            }
        }

        if (!isAllRoomConnectedWithMainRoom)           // �ϳ��� ������ �ȵǾ� ������
        {
            ConnectNearRoom(allRooms, true);
        }
    }

    /// <summary>
    /// ���ѵ� �� ������ ���̱�
    /// </summary>
    /// <param name="roomCount">������ �� ����</param>
    public void LimitRoomCount(uint roomCount)
    {
        if(roomCount < roomList.Count)
        {
            roomList.Sort((x, y) => x.nodes.Count > y.nodes.Count ? -1 : 1);        // ū ����� ���� ������ ����

            for (int i = 0; i < roomList.Count - roomCount; i++)                     // roomCount ��ŭ ������ �����ϰ� ���� ����� ����
            {
                roomList[(roomList.Count - 1) - i].ClearNodes();
            }
            roomList.RemoveRange((int)roomCount, (int) (roomList.Count - roomCount));
        }

        Debug.Log($" ���� Room List Count : {roomList.Count}");
    }

    

    /// <summary>
    /// Ư�� ��忡�� ����Ǿ� �ִ� ������ ã�� �� Ŭ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="x">����� X��ǥ</param>
    /// <param name="y">����� Y��ǥ</param>
    /// <returns>�ش� ��尡 ���Ե� Room Ŭ����(�̹� üũ�� ��쳪 �˻簡 �ȵǴ� ���(��)�� ��� null)</returns>
    List<Node> NearNodeList(int x, int y)
    {
        // �ش� ��尡 �̹� üũ�� �ǰų� üũ���ϴ� ����� ���
        if (mapNodes[GetIndex(x, y)].isChecked || !mapNodes[GetIndex(x, y)].data) return null;

        Stack<Node> stack = new();
        List<Node> list = new List<Node>();

        stack.Push(mapNodes[GetIndex(x, y)]);                  // ��带 ���ÿ� �ִ´�(���� 1)
        mapNodes[GetIndex(x, y)].isChecked = true;             // Ȯ�� ���� ǥ��

        while (stack.Count > 0)             // ���ÿ� �ƹ��͵� ���� ������ �ݺ�(�ش� ���� ����Ǿ� �ִ� ��� ��带 �˻� �ϸ� ������)
        {
            Node target = stack.Pop();      // ������ ������ ������
            list.Add(target);
            //room.nodes.Add(target);         // ���� ��带 ����Ʈ�� �߰��Ѵ�.

            // target ����� �����¿� ��带 �˻��Ѵ�.(+��� ��ġ��, �밢�� ����)
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // ���� �ٱ��� ��츦 �Ÿ���.
                    if (i * j == 0 && CheckInMap(target.gridPos.x + j, target.gridPos.y + i))
                    {
                        Node tempTarget = mapNodes[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
                        // �ֺ� ��� �� �˻縦 ���� ���ϰ� ��ĭ�� ��带 Ȯ���Ѵ�.
                        if (tempTarget.data && !tempTarget.isChecked)
                        {
                            stack.Push(tempTarget);                 // ��带 ���ÿ� �ִ´�
                            tempTarget.isChecked = true;            // Ȯ������ ǥ��
                        }
                    }
                }
            }
        }
        
        return list;
    }

    /// <summary>
    /// CheckNearNodesBool�� ��� ��带 ���� ��Ű�� �Լ�
    /// </summary>
    void GatherData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapNodes[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
                }
            }
        }
    }

    /// <summary>
    /// �ش� ��ǥ���� �ֺ� ������ data(bool)�� Ȯ���ؼ� �ڽ��� ���� �ֺ� bool �� ������ ���� �����ϴ� �޼���
    /// </summary>
    /// <param name="x">x ��ǥ��</param>
    /// <param name="y">y ��ǥ��</param>
    /// <returns>�ֺ� ��尡 true�� ���� �̻��̸� true, �ƴϸ� false</returns>
    bool CheckNearNodesBool(int x, int y)
    {
        int count = 0;
        int boolTCount = 0;
        for(int a = -1; a <= 1; a++)
        {
            for(int b = -1; b <= 1; b++)
            {
                if(CheckInMap(x + b, y + a))    // �� �ٱ��� �� false�� ó��
                {
                    // ��ó Ÿ�� ��尡 true �϶� ture ī��Ʈ ����
                    if (mapNodes[GetIndex(x + b, y + a)].data) boolTCount++;
                }
                count++;
            }
        }
        //boolTCount�� count�� ���ݺ��� ũ�� true, ������ false
        return (boolTCount - (count * 0.5f)) > 0f;
    }


    /// <summary>
    /// mapFillRate ������ ���� ����(Width) ����(Height)ũ�� ��ŭ ��ĭ Ȥ�� ����ĭ�� �����ؼ� nodes����Ʈ�� �����ϴ� �޼���
    /// </summary>
    void RandomFillNodeInMap(int mapWidth, int mapHeight, float fillRate)
    {   
        mapNodes = new Node[mapWidth * mapHeight];

        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = new Node(
                (Random.Range(0.0f, 1.0f) < fillRate),       // fiilrate���� ������ true(��ĭ) �ƴϸ� false(����ĭ)
                new Vector2Int(i % mapWidth, i / mapWidth));          // ��� ��ġ�� 
        }
    }

    /// <summary>
    /// �� ������ ��ǥ���� �����ϴ� �޼���
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns></returns>
    bool CheckInMap(int x, int y)
    {
        return !((x < 0 || x >= width) || (y < 0 || y >= height));
    }

    /// <summary>
    /// x, y���� �޾� nodes�� index�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns></returns>
    int GetIndex(int x, int y)
    {
        return x + y * width;
    }

    int roomIndex = 0;

    /// <summary>
    /// ���η�[0]���κ��� ����� ������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortRoom">�ϳ��� �����ؼ� ���� ��ȯ�� ����Ʈ</param>
    /// <param name="targetRoom">���� �� ���(�ߺ��̸� �߰� ����)</param>
    public void SortingRoomList(List<OldRoom> sortRoom, OldRoom targetRoom)
    {
        targetRoom.index = roomIndex;
        sortRoom.Add(targetRoom);

        foreach (OldRoom room in targetRoom.connectedRooms)
        {
            if (!sortRoom.Contains(room))
            {
                roomIndex++;
                SortingRoomList(sortRoom, room);
            }
        }
    }


#if UNITY_EDITOR
    /// <summary>
    /// �׽�Ʈ�� ����Ʈ Ȯ�� �Լ�
    /// </summary>
    public void PrintRoomlist()
    {
        Debug.Log($"�� {roomList.Count}�� ��");
        int i = 0;
        foreach(OldRoom room in roomList)
        {
            Debug.Log($"{i}��° {room.nodes.Count}�� ��");
        }
    }

    public List<Vector3Int> GetRoomPosList()
    {
        List<Vector3Int> result = new();

        foreach(OldRoom room in roomList)
        {
            result.Add(new Vector3Int((int) room.CenterX, (int) room.CenterY));
        }
        return result;  
    }



    //private void OnDrawGizmos()
    //{
    //    if (mapNodes != null && mapNodes.Length > 0 && roomList.Count > 0)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int x = 0; x < width; x++)
    //            {

    //                Color temp = new Color(255, 255, 255, 0.2f);
    //                Gizmos.color = temp;
    //                // false�� ����ĭ, ���� Ȯ�εȰŸ� ����, true�� ��ĭ
    //                if (!mapNodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
    //                else if (mapNodes[GetIndex(x, y)].isChecked)
    //                {
    //                    Gizmos.color = new Color(255,0,0,0.2f);
    //                    Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
    //                }



    //                //Gizmos.color = Color.blue;
    //                //foreach (var roomA in roomList)
    //                //{
    //                //    foreach(Room roomB in roomA.connectedRooms)
    //                //    {
    //                //        Gizmos.DrawCube(new Vector3(roomA.minX, roomA.minY), Vector3.one);
    //                //        Gizmos.DrawCube(new Vector3(roomB.minX, roomB.minY), Vector3.one);
    //                //    }
    //                //}

    //            }
    //        }
    //    }
    //}
#endif
}
