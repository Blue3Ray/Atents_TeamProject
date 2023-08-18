using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PassRoomGenerate : TestBase
{
    public RoomGenerator roomGenerator;

    protected override void Test1(InputAction.CallbackContext context)
    {
        roomGenerator.SetUpRooms();
    }
}
