
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomGenerate : TestBase
{
    RandomMapGenerator rmg;
    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public uint roomCount = 8;

    public bool useSeed = false;
    public string seed = "asd";
    List<Room> list;

    private void Start()
    {
        rmg = new();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        if (!useSeed) seed = null;
        rmg.SetUp((int)roomCount, width, height, fillRate, collecBoxBoolCount, seed);
    }

    private void OnDrawGizmos()
    {
        if (rmg != null && rmg.gridMap != null)
        {
            for(int x = 0; x < rmg.gridMap.Width; x++)
            {
                for(int y = 0; y < rmg.gridMap.Height; y++)
                {
                    if (rmg.gridMap.mapGrid[x, y] != null)
                    {
                        Gizmos.color = new Color(200, 0, 0, 0.5f);
                        Gizmos.DrawCube(new Vector3(x,y), Vector3.one * 1.0f);
                        Gizmos.color = new Color(0, 200, 0, 0.5f);
                        foreach (var temp in rmg.gridMap.mapGrid[x,y].connectedRooms)
                        {
                            Gizmos.DrawLine(new Vector3(x, y), (Vector2) rmg.gridMap.GetRoomGrid(temp.Item1));
                        }
                    }
                }
            }
            //foreach (Room item in list)
            //{
            //    Vector3 temp = ((Vector3Int)item.roomCoord);
            //    Gizmos.color = new Color(0, 200, 0, 0.5f);
            //    Gizmos.DrawCube(temp, Vector3.one * 1.0f);

            //    Gizmos.color = new Color(0, 0, 200, 0.5f);
            //    foreach ((Room, ExitDirection) connectedRoom in item.connectedRooms)
            //    {
            //        Gizmos.DrawLine((Vector3Int)item.roomCoord, (Vector3Int)connectedRoom.Item1.roomCoord);
            //    }

            //}
        }
    }
}
