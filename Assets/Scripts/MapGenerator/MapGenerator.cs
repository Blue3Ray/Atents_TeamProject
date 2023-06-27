using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 데이터(프리펩)을 가져와서 맵 생성하는 클래스
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public Tile backgroundTile;

    /// <summary>
    /// 맵데이터 불러올 배열들
    /// </summary>
    public Tilemap[] tilemapSamples;

    /// <summary>
    /// 타일 맵들(0 배경, 1 플랫폼)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    private void Awake()
    {
        m_tileMaps = new Tilemap[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }
    }

    private void Start()
    {
        cursor = new Vector3Int(0,0);

        GeneratingMap(tilemapSamples[0]);
        for (int i = 0; i < 10; i++)
        {
            cursor += new Vector3Int(tilemapSamples[0].cellBounds.size.x, 0);

            GeneratingMap(tilemapSamples[0]);
        }

    }

    /// <summary>
    /// 받아온 타일맵을 커서 위치 기준으로 그리기(배경도 함께)
    /// </summary>
    /// <param name="targetTileMap">샘플 타일맵</param>
    void GeneratingMap(Tilemap targetTileMap)
    {
        for (int y = targetTileMap.cellBounds.yMin; y < targetTileMap.cellBounds.yMax; y++)
        {
            for (int x = targetTileMap.cellBounds.xMin; x < targetTileMap.cellBounds.xMax; x++)
            {
                Vector3Int targetTile = cursor + new Vector3Int(x, y);

                if (targetTileMap.HasTile(new Vector3Int(x, y)))        // 복제할 위치에 타일이 있다면
                {
                    m_tileMaps[1].SetTile(targetTile, targetTileMap.GetTile(new Vector3Int(x,y)));  // 해당 타일과 동일한 타일을 설정

                    //Debug.Log(targetTileMap.GetTile(new Vector3Int(x, y)));
                }
                m_tileMaps[0].SetTile(targetTile, backgroundTile);      // 배경 타일맵에 배경을
            }
        }
    }
}
