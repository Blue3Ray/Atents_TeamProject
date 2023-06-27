using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �� ������(������)�� �����ͼ� �� �����ϴ� Ŭ����
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public Tile backgroundTile;

    /// <summary>
    /// �ʵ����� �ҷ��� �迭��
    /// </summary>
    public Tilemap[] tilemapSamples;

    /// <summary>
    /// Ÿ�� �ʵ�(0 ���, 1 �÷���)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
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
    /// �޾ƿ� Ÿ�ϸ��� Ŀ�� ��ġ �������� �׸���(��浵 �Բ�)
    /// </summary>
    /// <param name="targetTileMap">���� Ÿ�ϸ�</param>
    void GeneratingMap(Tilemap targetTileMap)
    {
        for (int y = targetTileMap.cellBounds.yMin; y < targetTileMap.cellBounds.yMax; y++)
        {
            for (int x = targetTileMap.cellBounds.xMin; x < targetTileMap.cellBounds.xMax; x++)
            {
                Vector3Int targetTile = cursor + new Vector3Int(x, y);

                if (targetTileMap.HasTile(new Vector3Int(x, y)))        // ������ ��ġ�� Ÿ���� �ִٸ�
                {
                    m_tileMaps[1].SetTile(targetTile, targetTileMap.GetTile(new Vector3Int(x,y)));  // �ش� Ÿ�ϰ� ������ Ÿ���� ����

                    //Debug.Log(targetTileMap.GetTile(new Vector3Int(x, y)));
                }
                m_tileMaps[0].SetTile(targetTile, backgroundTile);      // ��� Ÿ�ϸʿ� �����
            }
        }
    }
}
