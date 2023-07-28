using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RandomMap : MonoBehaviour 
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

    public class Room
    {   
        public List<Node> nodes = new List<Node>();
        public List<Room> connectedRooms = new List<Room>();

        public int minX;
        public int minY;
        public int maxX;
        public int maxY;

        public void SetXYData()
        {
            minX = minY = int.MaxValue;
            maxX = maxY = -1;
            if(nodes.Count <= 1)
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

        public static void ConnectRooms(Room rA, Room rB)
        {
            rA.connectedRooms.Add(rB);
            rB.connectedRooms.Add(rA);
        }

        public bool isConnected(Room targetRoom)
        {
            return connectedRooms.Contains(targetRoom);
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
    List<Room> roomList = new List<Room>();


    public RandomMap(int width, int height, float fillRate = 0.46f, int collecBoxBoolCount = 3)
    {
        Width = width;
        Height = height;
        mapFillRate = fillRate;
        collectBoxBoolCount = collecBoxBoolCount;
    }

    /// <summary>
    /// ���� ����� ��� �����ϱ�(1����)
    /// </summary>
    public void ConnectNearRoom()
    {
        List<Room> allRooms = roomList;
        float nearDistance = float.MaxValue;
        bool isPossible = false;
        Room tempA = new();
        Room tempB = new();

        foreach (Room roomA in allRooms)
        {
            //isPossible = false; 
            nearDistance = float.MaxValue;

            foreach(Room roomB in allRooms)
            {
                if (roomA == roomB) continue;   // ���� ���̸� BǮ �Ѿ��
                if (roomA.isConnected(roomB))
                {
                    //isPossible = false;         // �̹� ����Ǿ� ������ AǮ �Ѿ��
                    break;
                }
                float distanceRoom = Mathf.Pow(roomA.minX - roomB.minX,2) + Mathf.Pow(roomA.minY - roomB.minY, 2);
                if(distanceRoom < nearDistance)
                {
                    nearDistance = distanceRoom;
                    //isPossible = true;
                    tempA = roomA;
                    tempB = roomB;
                }
            }
            if(isPossible)
            {
                Room.ConnectRooms(tempA, tempB);
                
            }
        }
    }

    /// <summary>
    /// ���ѵ� �� ������ ���̱�
    /// </summary>
    /// <param name="roomCount">������ �� ����</param>
    public void LimitRoomCount(int roomCount)
    {
        if(roomCount < roomList.Count)
        {
            roomList.Sort((x, y) => x.nodes.Count > y.nodes.Count ? -1 : 1);        // ū ����� ���� ������ ����

            for (int i = 0; i < roomList.Count - roomCount; i++)                     // roomCount ��ŭ ������ �����ϰ� ���� ����� ����
            {
                roomList[(roomList.Count - 1) - i].ClearNodes();
            }
            roomList.RemoveRange(roomCount, roomList.Count - roomCount);
        }

        Debug.Log($" ���� Room List Count : {roomList.Count}");
    }

    /// <summary>
    /// RoomList�� �����ϰ� �����ϴ� �Լ�
    /// </summary>
    public void StartMapData()
    {
        ResetMap();

        for (int i = 0; i < collectBoxBoolCount; i++)
        {
            GatherData();
        }

        roomList = new List<Room>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Room tempRoom = CheckRoomList(x, y);

                if (tempRoom != null)       // ���������� �˻縦 ��ġ��
                {
                    roomList.Add(tempRoom);     // ����Ʈ�� �߰�
                    Debug.Log($"room list : {roomList.Count} => ({x}, {y}), Count : {tempRoom.nodes.Count}\n" +
                        $"Min : ({tempRoom.minX}, {tempRoom.minY}), Max : ({tempRoom.maxX}, {tempRoom.maxY})");
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
    Room CheckRoomList(int x, int y)
    {
        // �ش� ��尡 �̹� üũ�� �ǰų� üũ���ϴ� ����� ���
        if (mapNodes[GetIndex(x, y)].isChecked || !mapNodes[GetIndex(x, y)].data) return null;

        Room room = new();
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
        room.nodes = list;
        room.SetXYData();      // room Ŭ������ �ּ� ��ǥ�� �ִ� ��ǥ���� ����ȭ �Ѵ�.
        return room;
    }

    /// <summary>
    /// CheckNearNodesBool�� ��� ��带 ���� ��Ű�� �Լ�
    /// </summary>
    void GatherData()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapNodes[GetIndex(x, y)].data = CheckNearNodesBool(x, y);
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
    void ResetMap()
    {   
        mapNodes = new Node[Width * Height];

        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = new Node(
                (Random.Range(0.0f, 1.0f) < mapFillRate),       // fiilrate���� ������ true(��ĭ) �ƴϸ� false(����ĭ)
                new Vector2Int(i % Width, i / Width));          // ��� ��ġ�� 
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

#if UNITY_EDITOR
    /// <summary>
    /// �׽�Ʈ�� ����Ʈ Ȯ�� �Լ�
    /// </summary>
    public void PrintRoomlist()
    {
        Debug.Log($"�� {roomList.Count}�� ��");
        int i = 0;
        foreach(Room room in roomList)
        {
            Debug.Log($"{i}��° {room.nodes.Count}�� ��");
        }
    }



    private void OnDrawGizmos()
    {
        if (mapNodes != null && mapNodes.Length > 0 && roomList.Count > 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    //Gizmos.color = Color.white;

                    //foreach(Room room in roomlist)
                    //{
                    //    //Vector3[] tempsVec = new Vector3[4];
                    //    //tempsVec[0] = new Vector3(room.minX, room.minY);
                    //    //tempsVec[1] = new Vector3(room.minX, room.maxY);
                    //    //tempsVec[2] = new Vector3(room.maxX, room.maxY);
                    //    //tempsVec[3] = new Vector3(room.maxX, room.minY);
                    //    //Handles.DrawLines(tempsVec);
                    //    //Gizmos.DrawLine(new Vector3(room.minX, room.minY), new Vector3(room.minX, room.maxY));
                    //    //Gizmos.DrawLine(new Vector3(room.minX, room.maxY), new Vector3(room.maxX, room.maxY));
                    //    //Gizmos.DrawLine(new Vector3(room.maxX, room.maxY), new Vector3(room.maxX, room.minY));
                    //    //Gizmos.DrawLine(new Vector3(room.maxX, room.minY), new Vector3(room.minX, room.minY));
                    //    Gizmos.DrawCube(new Vector3((room.minX + room.maxX) * 0.5f, (room.minY + room.maxY) * 0.5f), new Vector3(room.maxX - room.minX, room.maxY - room.minY));
                    //}

                    //Gizmos.color = Color.black;
                    //// false�� ����ĭ, ���� Ȯ�εȰŸ� ����, true�� ��ĭ
                    //if (!mapNodes[GetIndex(x, y)].data) Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    //else if (mapNodes[GetIndex(x, y)].isChecked)
                    //{
                    //    Gizmos.color = Color.red;
                    //    Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                    //}

                    Gizmos.color = Color.blue;
                    foreach (var roomA in roomList)
                    {
                        foreach(Room roomB in roomA.connectedRooms)
                        {
                            Gizmos.DrawLine(new Vector3(roomA.minX, roomA.minY), new Vector3(roomB.minX, roomB.minY));
                        }
                    }
                    
                }
            }
        }
    }
#endif
}
