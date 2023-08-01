using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomGenerate : TestBase
{
    RoomGenerator roomGenerator;

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        
    }
}
