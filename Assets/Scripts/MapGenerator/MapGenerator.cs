using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 데이터(프리펩)을 가져와서 맵 생성하는 클래스
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// 맵데이터 불러올 배열들
    /// </summary>
    public Tilemap[] tilemaps;

    /// <summary>
    /// 자기 자신의 타일 맵
    /// </summary>
    Tilemap m_tileMap;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    private void Awake()
    {
        m_tileMap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        cursor = new Vector3Int(0,0);
        GeneratingMap(tilemaps[0]);
        for (int i = 0; i < 10; i++)
        {
            cursor += new Vector3Int(tilemaps[0].cellBounds.size.x, 0);
            GeneratingMap(tilemaps[0]);
        }

    }

    void GeneratingMap(Tilemap targetTileMap)
    {
        for (int y = targetTileMap.cellBounds.yMin; y <= targetTileMap.cellBounds.yMax; y++)
        {
            for (int x = targetTileMap.cellBounds.xMin; x <= targetTileMap.cellBounds.xMax; x++)
            {
                if (targetTileMap.HasTile(new Vector3Int(x, y)))
                {
                    Vector3Int targetTile = cursor + new Vector3Int(x, y);
                    m_tileMap.SetTile(targetTile, targetTileMap.GetTile(new Vector3Int(x,y)));

                    //Debug.Log(targetTileMap.GetTile(new Vector3Int(x, y)));
                }
            }
        }
    }
}
