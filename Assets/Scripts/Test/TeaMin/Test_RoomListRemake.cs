using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RoomListRemake : TestBase
{
    RoomGenerator roomGenerator;
    RandomMapGenerator rnmg;
    
    public int width = 100;
    public int height = 100;
    public float fillRate = 0.46f;
    public int collecBoxBoolCount = 3;

    public uint roomCount = 8;

    public string seed = "asd";
    List<Room> list;
    

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
        rnmg = new RandomMapGenerator();
        rnmg.SetUp((int) roomCount, width, height, fillRate, collecBoxBoolCount, seed);
        list = rnmg.roomList;
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        rnmg.SetUp((int)roomCount, width, height, fillRate, collecBoxBoolCount);
        list = rnmg.roomList;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        for(int i = 0; i < roomCount; i++)
        {
            Debug.Log($"{i}¹øÂ° ¹æ");
            foreach((Room, ExitDirection) r in list[i].connectedRooms)
            {
                Debug.Log(r.Item2);
            }
        }
    }



    private void OnDrawGizmos()
    {
        if (list != null)
        {
            foreach (Room item in list)
            {
                Vector3 temp = ((Vector3Int)item.roomCoord);
                Gizmos.color = new Color(0, 200, 0, 0.5f);
                Gizmos.DrawCube(temp, Vector3.one * 1.0f);

                Gizmos.color = new Color(0, 0, 200, 0.5f);
                foreach((Room, ExitDirection) connectedRoom in item.connectedRooms)
                {
                    Gizmos.DrawLine((Vector3Int)item.roomCoord, (Vector3Int)connectedRoom.Item1.roomCoord);
                }
                
            }
        }
    }
}
