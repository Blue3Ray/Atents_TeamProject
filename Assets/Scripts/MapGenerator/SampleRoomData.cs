using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

enum RoomLayer
{
    Background = 0,
    Platform,
    Exit
}

enum ExitDirection
{
    Up = 0,
    Left,
    Right,
    Down
}

struct Exit
{
    public Vector3Int Pos;
    public ExitDirection Direction;

    public Exit(Vector3Int pos, ExitDirection dir)
    {
        Pos = pos;
        Direction = dir;
    }
}

public class SampleRoomData : MonoBehaviour
{
    List<Tilemap> mapLayers;
    List<Exit> exitPos;

    public Vector3Int min;
    public Vector3Int max;

    public int height;
    public int width;

    public void OnInitialize()
    {
        mapLayers.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            mapLayers.Add(transform.GetChild(i).GetComponent<Tilemap>());
        }

        min = new Vector3Int(mapLayers[1].cellBounds.xMin, mapLayers[1].cellBounds.yMin);
        max = new Vector3Int(mapLayers[1].cellBounds.xMax, mapLayers[1].cellBounds.yMax);

        height = max.y - min.y;
        width = max.x - min.x;

        CheckExitPos(mapLayers[2]);
    }

    void CheckExitPos(Tilemap targetTileMap)
    {
        exitPos = new List<Exit>();
        for (int y = targetTileMap.cellBounds.yMin; y < targetTileMap.cellBounds.yMax; y++)
        {
            for (int x = targetTileMap.cellBounds.xMin; x < targetTileMap.cellBounds.xMax; x++)
            {
                if (targetTileMap.HasTile(new Vector3Int(x, y)))
                {
                    ExitDirection tempDir = new();
                    if (x == min.x) tempDir = ExitDirection.Left;
                    else if (x == max.x - 1) tempDir = ExitDirection.Right;
                    else if (y == min.y) tempDir = ExitDirection.Down;
                    else if (y == max.y - 1) tempDir = ExitDirection.Up;
                    else
                    {
                        Debug.LogWarning($"({x}, {y}) 출구 위치가 이상합니다. 샘플 데이터({min} ~ {max}) 확인 필요");
                    }
                    Debug.Log($"출구 위치 : {x}, {y}, {tempDir}");
                    Exit tempExit = new Exit(new Vector3Int(x, y), tempDir);
                    exitPos.Add(tempExit);
                }
            }
        }
    }
}
