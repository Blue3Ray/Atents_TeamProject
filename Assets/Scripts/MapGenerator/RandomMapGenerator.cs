using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public enum ExitDirection
{
    None = -1,
    Up = 0,
    Left,
    Right,
    Down
}

class NodeMap
{
    class Node
    {
        public bool data;
        public bool isChecked;
        public Vector2Int gridPos;
        public Node(bool data, Vector2Int pos, bool isChecked = false)
        {
            this.data = data;
            this.isChecked = isChecked;
            gridPos = pos;
        }
    }

    Node[] mapNode;
    List<List<Node>> nodesList;

    int width;
    int height;

    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="w">���α���</param>
    /// <param name="h">���α���</param>
    public NodeMap(int w, int h)
    {
        width = w;
        height = h;
        mapNode = new Node[w * h];
    }

    /// <summary>
    /// ���� �����ϰ� ä��� �Լ�
    /// </summary>
    /// <param name="fillRate">ä��� ����</param>
    public void RandomFillNodeInMap(float fillRate)
    {
        for (int i = 0; i < mapNode.Length; i++)
        {
            mapNode[i] = new Node(
                (Random.Range(0.0f, 1.0f) < fillRate),       // fiilrate���� ������ true(��ĭ) �ƴϸ� false(����ĭ)
                new Vector2Int(i % width, i / height));          // ��� ��ġ�� 
        }
    }

    /// <summary>
    /// ������ ��ġ�� ����� �Լ�
    /// </summary>
    /// <param name="count">�۾� ������ Ƚ��</param>
    public void GatherData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapNode[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
                }
            }
        }
    }

    /// <summary>
    /// ��� ��ó�� ������ ���� ���� �ڽ� ����� ���� �����ǰ� �ϴ� �Լ�
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns>�����̻��� ���̸� ��, �ƴϸ� ����</returns>
    bool CheckNearNodesBool(int x, int y)
    {
        int count = 0;
        int boolTCount = 0;
        for (int a = -1; a <= 1; a++)
        {
            for (int b = -1; b <= 1; b++)
            {
                if (CheckInMap(x + b, y + a))    // �� �ٱ��� �� false�� ó��
                {
                    // ��ó Ÿ�� ��尡 true �϶� ture ī��Ʈ ����
                    if (mapNode[GetIndex(x + b, y + a)].data) boolTCount++;
                }
                count++;
            }
        }
        //boolTCount�� count�� ���ݺ��� ũ�� true, ������ false
        return (boolTCount - (count * 0.5f)) > 0f;
    }

    /// <summary>
    /// ���� �����ϴ� ��� ����Ʈ �����
    /// </summary>
    public void GetNodesList()
    {
        nodesList = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Node> nodeList = NearNodeList(x, y);

                if (nodeList != null)       // ���������� �˻縦 ��ġ��
                {
                    nodesList.Add(nodeList);     // ����Ʈ�� �߰�
                }
            }
        }
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
        if (mapNode[GetIndex(x, y)].isChecked || !mapNode[GetIndex(x, y)].data) return null;

        Stack<Node> stack = new();
        List<Node> list = new List<Node>();

        stack.Push(mapNode[GetIndex(x, y)]);                  // ��带 ���ÿ� �ִ´�(���� 1)
        mapNode[GetIndex(x, y)].isChecked = true;             // Ȯ�� ���� ǥ��

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
                        Node tempTarget = mapNode[GetIndex(target.gridPos.x + j, target.gridPos.y + i)];
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
    /// ���ѵ� �氳������ ������ ���� ����� �����ϱ�
    /// </summary>
    /// <param name="listCount"></param>
    public void LimitRoomCount(int listCount)
    {
        if (listCount < nodesList.Count)
        {
            nodesList.Sort((x, y) => x.Count > y.Count ? -1 : 1);        // ū ����� ���� ������ ����

            for (int i = 0; i < nodesList.Count - listCount; i++)                     // roomCount ��ŭ ������ �����ϰ� ���� ����� ����
            {
                foreach (Node node in nodesList[(nodesList.Count - 1) - i])
                {
                    node.data = false;
                }
            }
            nodesList.RemoveRange((int)listCount, (int)(nodesList.Count - listCount));
        }
    }
    
    /// <summary>
    /// ��� ������ ����Ʈ�� ���ؼ� center��ǥ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns>Center��ǥ�� ����Ʈ</returns>
    public List<Vector2Int> GetRoomCoord()
    {
        List<Vector2Int> list = new();
        foreach(List<Node> temp in nodesList)
        {
            list.Add(GetCenterCoord(temp));
        }
        return list;
    }

    // ������ ��� �κ�===============================================
    /// <summary>
    /// center ��ǥ���� �޾ƿ��� �Լ�
    /// </summary>
    /// <param name="nodes">center ��ǥ�� �˰� ���� ����</param>
    /// <returns></returns>
    Vector2Int GetCenterCoord(List<Node> nodes)
    {
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        
        foreach(Node node in nodes)
        {
            if (node.gridPos.x < minX)
            {
                minX = node.gridPos.x;
            }
            else if (node.gridPos.x > maxX)
            {
                maxX = node.gridPos.x;
            }

            if (node.gridPos.y < minY)
            {
                minY = node.gridPos.y;
            }
            else if (node.gridPos.y > maxY)
            {
                maxY = node.gridPos.y;
            }
        }

        int centerX = (int)((maxX + minX) * 0.5f);
        int centerY = (int)((maxY + minY) * 0.5f);

        return new Vector2Int(centerX, centerY);
    }


    /// <summary>
    /// ������ �� �������� Ȯ���ϴ� �κ�
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns></returns>
    bool CheckInMap(int x, int y)
    {
        return !((x < 0 || x >= width) || (y < 0 || y >= height));
    }

    /// <summary>
    /// x,y������ �� �迭 �ε��� ã��
    /// </summary>
    /// <param name="x">x ��ǥ</param>
    /// <param name="y">y ��ǥ</param>
    /// <returns>index</returns>
    int GetIndex(int x, int y)
    {
        return x + y * width;
    }
}


public class RoomMap
{
    public List<Room> roomList;

    public RoomMap(List<Vector2Int> map)
    {
        roomList = GetRoomList(map);
    }

    /// <summary>
    /// ���η� ����(0��°�� ���� ���ʿ� �ִ� ���η�) ���� ����� ��� �����ϱ�(�����̸� �ϴ� ��ó 1����, ���̸� ���η�� ����Ȯ�� �� ����)
    /// </summary>
    public void ConnectNearRoom(List<Room> allRooms, bool onceChecked = false)
    {
        Room tempA = new();
        Room tempB = new();

        List<Room> roomListA = new();
        List<Room> roomListB = new();

        bool isPossible = false;
        float nearDistance = float.MaxValue;

        if (onceChecked)     // �̹� �ѹ� üũ �Ѱ� Ȯ��
        {

            foreach (Room temp in allRooms)      // ��� ���� �˻��ϸ鼭 
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

        foreach (Room roomA in roomListA)
        {
            if (!onceChecked)
            {
                nearDistance = float.MaxValue;              // �⺻�� ����

                if (roomA.connectedRooms.Count > 0)         // ���� ���� ���� �Ǿ� ������
                    continue;                                   // �н��ϱ�
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB) continue;   // A�� B�� ���� ���̸� ��ŵ(�ڱ��ڽ�)

                float distance = Mathf.Pow(roomA.roomCoord.x - roomB.roomCoord.x, 2) + Mathf.Pow(roomA.roomCoord.y - roomB.roomCoord.y, 2);

                if (distance < nearDistance)         // �� ���� ���� ��ϵǾ� �ִ� �ִ� �Ÿ����� ������
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
            else if (isPossible && !onceChecked)                 // �� ���� ���� �����ϸ� �����ϱ�, ó�� ��� ���� ���� ��
            {
                Room.ConnectRooms(tempA, tempB);
            }
        }

        if (isPossible && onceChecked)           // ���� �ٸ� Room����Ʈ���� ����� ���� �����ϱ�
        {
            Room.ConnectRooms(tempA, tempB);
        }

        bool isAllRoomConnectedWithMainRoom = true;     // ��� ���� ����Ǿ� �ִ��� ���� Ȯ��
        foreach (Room temp in allRooms)
        {
            if (!temp.isAccessibleMainRoom)             // �ϳ��� ���� �ȵǾ� ������ false
            {
                isAllRoomConnectedWithMainRoom = false;
            }
        }

        if (!isAllRoomConnectedWithMainRoom)           // �ϳ��� ������ �ȵǾ� ������
        {
            ConnectNearRoom(allRooms, true);            // �Լ� �����(���������� ����Ǿ� �ִ°� �ȵǾ� �ִ°� �˻���)
        }
    }

    /// <summary>
    /// �������� ���� ��ġ ����Ʈ�� Room����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="map">����</param>
    /// <returns>RoomList</returns>
    List<Room> GetRoomList(List<Vector2Int> map)
    {
        List<Room> result = new List<Room>();
        foreach (var roomCoord in map)
        {
            result.Add(new Room(roomCoord));
        }

        return result;
    }

    /// <summary>
    /// X ��ǥ �������� ����
    /// </summary>
    /// <param name="roomList">������ ����Ʈ</param>
    public void SortByCoordX()
    {
        roomList.Sort((a, b) => a.roomCoord.x < b.roomCoord.x ? -1 : 1);      // ���� ���ʺ��� ���������� ����(���� �� ���� �ȵ�)

        roomList[0].isMainRoom = true;                          // ���� ���ʿ� �ִ� ���� ���� ������ ����(���� ��)
        roomList[0].isAccessibleMainRoom = true;
    }


}

public class Room
{
    /// <summary>
    /// ����Ǿ� �ִ� ��� �ش���� ����
    /// </summary>
    public List<(Room, ExitDirection)> connectedRooms;

    public bool isMainRoom = false;
    public bool isAccessibleMainRoom = false;

    public Vector2Int roomCoord;

    public Room() { }

    public Room(Vector2Int coord)
    {
        roomCoord = coord;
        connectedRooms = new();
    }

    public void SetAccessibleMainRoom()
    {
        isAccessibleMainRoom = true;            // �ش� ���� true�� ����

        for(int i = 0; i < connectedRooms.Count; i++)       // ����Ǿ� �ִ� ��鿡 ���ؼ� �˻�
        {
            Room room = connectedRooms[i].Item1;
            if (!room.isAccessibleMainRoom)             // ����Ǿ� �ִ� ��� �� ture�� �ƴ� ���� ������
            {
                room.SetAccessibleMainRoom();           // �ش� ��� ����Ǿ� �ִ� ��鵵 ���η�� ���� �������� �ٲ�
            }
        }
    }

    /// <summary>
    /// �̹��� target�� ���� ����(�ٷ� ��) �Ǿ� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="targetRoom">Ȯ���� ��</param>
    /// <returns>���̸� ���� �Ǿ� ����, �����̸� ���� �ȵǾ� ����</returns>
    public bool IsConnected(Room targetRoom)
    {
        foreach((Room, ExitDirection) room in connectedRooms)
        {
            if(room.Equals(targetRoom))
            {
                return true;
            }
        }
        return false;
    }

    public static void ConnectRooms(Room room1, Room room2)
    {
        // ������ �� �� �� �ϳ��� ���� ��� ������ �Ǿ� ������ �Ѵ� ���� ����
        if (room1.isAccessibleMainRoom)
        {
            room2.SetAccessibleMainRoom();
        }
        else if (room2.isAccessibleMainRoom)
        {
            room1.SetAccessibleMainRoom();
        }

        room1.SetExitDirAndAddConnectList(room2);
        room2.SetExitDirAndAddConnectList(room1);
    }

    void SetExitDirAndAddConnectList(Room targetRoom)
    {
        ExitDirection result = ExitDirection.None;

        // target�� ���� �뺸�� ���� ���� ��
        if(targetRoom.roomCoord.y > roomCoord.y)
        {
            // target�� ���� �� �������� X�ຸ�� Y�� ������ �ö� ���� ��
            if (targetRoom.roomCoord.y - roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Up;
            }
            else
            {
                // target�� �����ʿ� ���� ��
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }
        else
        {
            // target�� ���� �� �������� X�ຸ�� Y�� ������ ������ ���� ��
            if (roomCoord.y - targetRoom.roomCoord.y > Mathf.Abs(targetRoom.roomCoord.x - roomCoord.x))
            {
                result = ExitDirection.Down;
            }
            else
            {
                // target�� �����ʿ� ���� ��
                if (targetRoom.roomCoord.x > roomCoord.x)
                {
                    result = ExitDirection.Right;
                }
                else
                {
                    result = ExitDirection.Left;
                }
            }
        }

        if (result != ExitDirection.None)
        {
            connectedRooms.Add((targetRoom, result));
        }
        else
        {
            Debug.LogWarning("�� ������ �ȵ˴ϴ�!");
        }
    }
    
}


public class GridMap
{
    public Room[,] mapGrid;
    int width;
    public int Width => width;

    int height;
    public int Height => height;
    public static Vector2Int cursor = Vector2Int.zero;

    public GridMap(Vector2Int size)
    {
        width = size.x; height = size.y;
        mapGrid = new Room[size.x, size.y];
        // ��� Grid�� null���� �ʱ�ȭ
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                mapGrid[i, j] = null;
            }
        }
        cursor = Vector2Int.zero;
    }

    

    public void MakeGridMap(Room room, ExitDirection exitDir)
    {
        Room tempRoom = room;

        if (cursor.y >= mapGrid.GetLength(1) || cursor.x >= mapGrid.GetLength(0) || cursor.x < 0 || cursor.y < 0)
        {
            int aaa = 0;
        }

        mapGrid[cursor.x, cursor.y] = tempRoom;

        for(int i = 0; i < tempRoom.connectedRooms.Count; i++)
        {
            if (!IsContain(tempRoom.connectedRooms[i].Item1))       // ���� �׸��� �ʿ� �������� ������
            {
                int a = 100;
                while (a > 0)
                {
                    bool isXDir = false;
                    switch (tempRoom.connectedRooms[i].Item2)           // ���⿡ ���� Ŀ�� ������ ��
                    {
                        case ExitDirection.Up:
                            cursor.y++;
                            isXDir = false;
                            break;
                        case ExitDirection.Right:
                            cursor.x++;
                            isXDir = true;
                            break;
                        case ExitDirection.Left:
                            if (cursor.x == 0) MoveAllGridCell(false);
                            else cursor.x--;
                            isXDir = true;
                            break;
                        case ExitDirection.Down:
                            if (cursor.y == 0) MoveAllGridCell(true);
                            else cursor.y--;
                            isXDir = false;
                            break;
                        default:
                            Debug.LogWarning("�������� ���� �����Դϴ�.");
                            break;
                    }

                    bool canDeploy = true;

                    if (isXDir) // ���η� ���� ���� ��
                    {
                        for (int y = 0; y < mapGrid.GetLength(1); y++)      
                        {
                            if (mapGrid[cursor.x, y] != null)   // ���� ���ο� ���� ������
                            {
                                canDeploy = false;
                                break;
                            }
                        }
                    }
                    else    // ���η� ���� ���� ��
                    {
                        for (int x = 0; x < mapGrid.GetLength(0); x++)
                        {
                            if (mapGrid[x, cursor.y] != null)   // ���� ���ο� ���� ������
                            {
                                canDeploy = false;
                                break;
                            }
                        }
                    }

                    if (canDeploy)
                    {
                        break;
                    }
                    a--;
                }

                MakeGridMap(tempRoom.connectedRooms[i].Item1, tempRoom.connectedRooms[i].Item2);
            }
        }

        switch (exitDir)
        {
            case ExitDirection.Up:
                cursor.y--;
                break;
            case ExitDirection.Left:
                cursor.x++;
                break;
            case ExitDirection.Right:
                cursor.x--;
                break;
            case ExitDirection.Down:
                cursor.y++;
                break;
        }
    }

    public Vector2Int GetRoomGrid(Room room)
    {
        Vector2Int temp = -Vector2Int.one;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapGrid[x, y] != null && mapGrid[x, y].Equals(room))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return temp;
    }


    bool IsContain(Room room)
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(mapGrid[x, y] != null && mapGrid[x, y].Equals(room))
                {
                    return true;
                }
            }
        }
        return false;
    }


    void MoveAllGridCell(bool isY)
    {
        for (int y = mapGrid.GetLength(1) - 2; y >= 0 ; y--)
        {
            for (int x = mapGrid.GetLength(0) - 2; x >= 0 ; x--)
            {
                if (mapGrid[x, y] != null)
                {
                    Room temp = mapGrid[x, y];
                    if (isY)
                    {
                        mapGrid[x, y + 1] = temp;
                        mapGrid[x, y] = null;
                    }
                    else
                    {
                        mapGrid[x + 1, y] = temp;
                        mapGrid[x, y] = null;
                    }
                }
            }
        }
    }
}


public class RandomMapGenerator
{
    /// <summary>
    /// ����(�� ����Ʈ ���� �������� ���)
    /// </summary>
    NodeMap nodeMap;

    /// <summary>
    /// ���
    /// </summary>
    RoomMap roomMap;

    public GridMap gridMap;

    public List<Room> roomList;


    public void SetUp(int roomCount = 8, int width = 100, int height = 100, float mapFillRate = 0.46f, int collectNodeCount = 3)
    {
        nodeMap = new NodeMap(width, height);

        nodeMap.RandomFillNodeInMap(mapFillRate);           // ���� �����ϰ� �Ѹ���

        nodeMap.GatherData(collectNodeCount);               // ������ ����ȭ ��Ű��

        nodeMap.GetNodesList();                             // ������ ������ ����Ʈ�� �����

        nodeMap.LimitRoomCount(roomCount);                  // ���ѵ� �� ������ ����(����Ʈ ū�� -> ������ ������ ����)

        roomMap = new RoomMap(nodeMap.GetRoomCoord());      // nodemap�� ��ȯ

        roomMap.SortByCoordX();                             // Room�� X��ǥ �������� ������ �� ���η� ����

        roomMap.ConnectNearRoom(roomMap.roomList);                          // �� �����ϱ�

        roomList = roomMap.roomList;        // �׽�Ʈ�� �ӽ�

        Vector2Int size = GetGridMapSize(roomMap.roomList);

        gridMap = new GridMap(size);

        Debug.Log($"�� ũ�� : {size}");

        gridMap.MakeGridMap(roomMap.roomList[0], ExitDirection.None);


    }


    public Vector2Int GetGridMapSize(List<Room> roomList)
    {
        int x = 0;
        int y = 0;
        foreach (Room room in roomList)
        {
            foreach ((Room, ExitDirection) connectedRoom in room.connectedRooms)
            {
                if (connectedRoom.Item2 == ExitDirection.Left || connectedRoom.Item2 == ExitDirection.Right)
                {
                    x++;
                }
                else if (connectedRoom.Item2 == ExitDirection.Up || connectedRoom.Item2 == ExitDirection.Down)
                {
                    y++;
                }
            }
        }
        return new Vector2Int((int)(x * 0.5f) + 1, (int)(y * 0.5f) + 1);
    }










}
