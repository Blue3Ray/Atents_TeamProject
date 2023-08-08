using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_PassGenerate : TestBase
{
    public RoomGenerator roomGenerator;

    public Tilemap exitTileMap;

    Vector3Int[] exitPos = new Vector3Int[2];

    private void Start()
    {
        Vector2Int tileMapSize = (Vector2Int)exitTileMap.size;
        Vector2Int startPos = new Vector2Int((int)exitTileMap.origin.x, (int)exitTileMap.origin.y);

        Debug.Log(tileMapSize);

        List<Vector2Int> poss = new();
        for(int x = startPos.x; x < startPos.x + tileMapSize.x; x++)
        {
            for(int y = startPos.y; y < startPos.y + tileMapSize.y; y++)
            {
                if(exitTileMap.GetTile(new Vector3Int(x,y)) != null)
                {
                    poss.Add(new Vector2Int(x, y));
                }
            }
        }

        Exit exit = new Exit(exitPos[0], ExitDirection.Right);
        Exit exit2 = new Exit(new Vector3Int(0,14), ExitDirection.Left);

        roomGenerator.GeneratePassway(exit, exit2);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        
    }


}
