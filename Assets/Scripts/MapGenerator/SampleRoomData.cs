using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum RoomLayer
{
    Background = 0,
    Platform,
    PlatformHalf,
    Exit
}

public struct PassWay
{
    public Vector3Int Pos;
    public ExitDirection Direction;
    public bool isConnected;

    public PassWay(Vector3Int pos, ExitDirection dir, bool isConnected = false)
    {
        Pos = pos;
        Direction = dir;
        this.isConnected = isConnected;
    }
}

public class SampleRoomData : MonoBehaviour
{
    public List<Tilemap> mapLayers;

    public List<List<Vector3Int>> tilesPos;

    public List<PassWay> exitPos;

    public Vector3Int min;
    public Vector3Int max;

    public int Height => max.y - min.y;
    public int Width => max.x - min.x;

    public int OnInitialize()
    {
        int maxSize = 0;

        mapLayers.Clear();
        tilesPos = new List<List<Vector3Int>>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Tilemap temp = transform.GetChild(i).GetComponent<Tilemap>();
            tilesPos.Add(GetTileVector(temp));
            mapLayers.Add(temp);
        }

        // 전체 크기가 될 플랫폼을(자식 1번째) 기준으로 설정
        min = new Vector3Int(mapLayers[1].cellBounds.xMin, mapLayers[1].cellBounds.yMin);       
        max = new Vector3Int(mapLayers[1].cellBounds.xMax, mapLayers[1].cellBounds.yMax);

        //height = mapLayers[1].size.y;
        //width = mapLayers[1].size.x;

        if(mapLayers.Count > 3) CheckExitPos(mapLayers[mapLayers.Count - 1]);

        maxSize = Height > Width ? Height:Width;

        return maxSize;
    }

    void CheckExitPos(Tilemap targetTileMap)
    {
        exitPos = new List<PassWay>();
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
                        Debug.LogWarning($"({x}, {y}) 출구 위치가 이상합니다. 샘플 데이터({this.gameObject.name}) 확인 필요");
                    }
                    //Debug.Log($"출구 위치 : {x}, {y}, {tempDir}");
                    PassWay tempExit = new PassWay(new Vector3Int(x, y), tempDir);
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

    public List<PassWay> GetExitList(ExitDirection dir)
    {
        List<PassWay> result = new();
        
        foreach(PassWay exit in exitPos)
        {
            if(exit.Direction == dir)
            {
                result.Add(exit);
            }
        }

        return result;
    }
}
