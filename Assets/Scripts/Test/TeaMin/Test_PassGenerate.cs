using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_PassGenerate : TestBase
{
    public RoomGenerator roomGenerator;

    Vector3Int[] exitPos = new Vector3Int[2];

    private void Start()
    {

    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        PassWay exit = new PassWay(new Vector3Int(3, 0), ExitDirection.Right);
        PassWay exit2 = new PassWay(new Vector3Int(2, 14), ExitDirection.Left);
        roomGenerator.GeneratePassway(exit, exit2);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {

    }


}
