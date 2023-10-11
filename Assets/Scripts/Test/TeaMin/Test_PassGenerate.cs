using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_PassGenerate : TestBase
{
    public RoomGenerator roomGenerator;

    PassWay[] exitPos = new PassWay[2];

    private void Start()
    {

    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        PassWay exit = new PassWay(new Vector3Int(-1, 10), ExitDirection.Down);
        PassWay exit2 = new PassWay(new Vector3Int(1, -10), ExitDirection.Up);
        roomGenerator.GeneratePassway(exit, exit2);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {

    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        if (exitPos[0].Pos == Vector3Int.zero)
        {
            exitPos[0] = new PassWay(WorldToGrid(worldPos), ExitDirection.None);
        }
        else if (exitPos[1].Pos == Vector3Int.zero)
        {
            exitPos[1] = new PassWay(WorldToGrid(worldPos), ExitDirection.None);

            Vector3 minu = exitPos[1].Pos - exitPos[0].Pos;
            if (Mathf.Abs(minu.x) > Mathf.Abs(minu.y))
            {
                if (minu.x > 0)
                {
                    exitPos[0].Direction = ExitDirection.Right;
                    exitPos[1].Direction = ExitDirection.Left;
                }
                else
                {
                    exitPos[0].Direction = ExitDirection.Left;
                    exitPos[1].Direction = ExitDirection.Right;
                }
            }
            else
            {
                if (minu.y > 0)
                {
                    exitPos[0].Direction = ExitDirection.Up;
                    exitPos[1].Direction = ExitDirection.Down;
                }
                else
                {
                    exitPos[0].Direction = ExitDirection.Down;
                    exitPos[1].Direction = ExitDirection.Up;
                }
            }


            roomGenerator.GeneratePassway(exitPos[0], exitPos[1]);
        }
        else
        {
            roomGenerator.ClearData();
            exitPos = new PassWay[2];
        }
    }

    Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int((int)worldPos.x, (int)worldPos.y);
    }


}
