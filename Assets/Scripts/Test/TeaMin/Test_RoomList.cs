using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomList : TestBase
{
    RoomGenerator roomGenerator;
    RandomMap randomMap;
    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public uint roomCount = 8;

    List<Vector3Int> list;
    

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
        randomMap = new(width, height, fillRate , collecBoxBoolCount);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        randomMap.StartMapData(roomCount);
        list = randomMap.GetRoomPosList();

        List<Room> testList = new List<Room>();
        randomMap.SortingRoomList(testList, randomMap.roomList[0]);
        int i = 0;
        foreach(Room room in testList)
        {
            Debug.Log($"{i}¹øÂ° : {room.CenterX}, {room.CenterY}");
            i++;
        }

        Debug.Log($"Count : {randomMap.widthCount}, {randomMap.heightCount}");
    }

    private void OnDrawGizmos()
    {
        if (list != null)
        {
            foreach (var item in list)
            {
                Gizmos.color = new Color(0, 200, 0, 0.5f);
                Gizmos.DrawCube(item, Vector3.one * 3.0f);
            }
        }
    }
}
