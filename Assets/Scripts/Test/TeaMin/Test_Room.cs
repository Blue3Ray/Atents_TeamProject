using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Room : TestBase
{
    RoomGenerator roomGenerator;
    // Start is called before the first frame update
    void Start()
    {
        roomGenerator = FindAnyObjectByType<RoomGenerator>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        roomGenerator.ClearData();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        roomGenerator.MakeLevel();
    }
}
