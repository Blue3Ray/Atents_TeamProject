using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomSingleTiles : MonoBehaviour
{
    /// <summary>
    /// Ÿ���� �ִ� ���͵��� ����Ʈ
    /// </summary>
    List<Vector2Int> tilesPos;

    /// <summary>
    /// �ڱ� �ڽŷ��̾��� Ÿ�ϸ�
    /// </summary>
    public Tilemap m_tileMap;       

    private void Awake()
    {
        m_tileMap = GetComponent<Tilemap>();
        tilesPos = new();
    }

    private void Start()
    {
        CheckTileVectorInt();
    }

    public List<Vector2Int> CheckTileVectorInt()
    {
        tilesPos.Clear();
        for (int y = m_tileMap.cellBounds.yMin; y <= m_tileMap.cellBounds.yMax; y++)
        {
            for (int x = m_tileMap.cellBounds.xMin; x <= m_tileMap.cellBounds.xMax; x++)
            {
                if (m_tileMap.HasTile(new Vector3Int(x, y)))
                {
                    tilesPos.Add(new Vector2Int(x, y));
                    Debug.Log(m_tileMap.GetTile(new Vector3Int(x, y)));
                }
            }
        }
        return tilesPos;
    }
}
