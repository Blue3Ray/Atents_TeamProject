using System.Collections;
using System.Collections.Generic;
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
