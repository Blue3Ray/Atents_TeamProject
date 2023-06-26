using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �� ������(������)�� �����ͼ� �� �����ϴ� Ŭ����
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// �ʵ����� �ҷ��� �迭��
    /// </summary>
    public Tilemap[] tilemaps;

    /// <summary>
    /// �ڱ� �ڽ��� Ÿ�� ��
    /// </summary>
    Tilemap m_tileMap;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
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
