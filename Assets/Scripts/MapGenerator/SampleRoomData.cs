using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public enum RoomLayer
{
    Background = 0,
    Platform,
    PlatformHalf,
    Exit
}

public enum ExitDirection
{
    Up = 0,
    Left,
    Right,
    Down
}

public struct Exit
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
    public List<Tilemap> mapLayers;

    public List<List<Vector3Int>> tilesPos;
    

    public List<Exit> exitPos;

    public Vector3Int min;
    public Vector3Int max;

    public int height;
    public int width;

    public void OnInitialize()
    {
        mapLayers.Clear();
        tilesPos = new List<List<Vector3Int>>();


        for (int i = 0; i < transform.childCount; i++)
        {
            Tilemap temp = transform.GetChild(i).GetComponent<Tilemap>();
            tilesPos.Add(GetTileVector(temp));
            mapLayers.Add(temp);
        }

        min = new Vector3Int(mapLayers[1].cellBounds.xMin, mapLayers[1].cellBounds.yMin);
        max = new Vector3Int(mapLayers[1].cellBounds.xMax, mapLayers[1].cellBounds.yMax);

        height = max.y - min.y;
        width = max.x - min.x;

        CheckExitPos(mapLayers[mapLayers.Count - 1]);
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
                    //Debug.Log($"출구 위치 : {x}, {y}, {tempDir}");
                    Exit tempExit = new Exit(new Vector3Int(x, y), tempDir);
                    exitPos.Add(tempExit);
                }
            }
        }
    }

    List<Vector3Int> GetTileVector(Tilemap targetTileMap)
    {
        int count = GetTileAmount(targetTileMap);
        List<Vector3Int> tempVector = new List<Vector3Int>();

        for (int y = targetTileMap.cellBounds.yMin; y < targetTileMap.cellBounds.yMax; y++)
        {
            for (int x = targetTileMap.cellBounds.xMin; x < targetTileMap.cellBounds.xMax; x++)
            {
                if (targetTileMap.HasTile(new Vector3Int(x, y)))
                {
                    tempVector.Add(new Vector3Int(x, y));
                }
            }
        }

        return tempVector;
    }

    public int GetTileAmount(Tilemap tilemap)
    {
        int amount = 0;

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                amount += 1;
            }
        }

        //Debug.Log($"타일을 {amount}개 가지고 있음");
        return amount;
    }

    
}
