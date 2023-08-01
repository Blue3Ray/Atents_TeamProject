using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomList : TestBase
{
    RandomMap randomMap;
    public uint roomCount = 8;

    private void Start()
    {
        randomMap = FindObjectOfType<RandomMap>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        randomMap.StartMapData(roomCount);
    }
}
