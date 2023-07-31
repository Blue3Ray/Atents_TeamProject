using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomList : TestBase
{
    public RandomMap randomMap;
    protected override void Test1(InputAction.CallbackContext context)
    {
        randomMap.PrintRoomlist();
    }
}
