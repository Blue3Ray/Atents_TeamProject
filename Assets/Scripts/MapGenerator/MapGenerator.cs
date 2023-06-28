using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

struct RoomData
{
    Tilemap map;
    Tilemap exit;

    public RoomData(Tilemap map, Tilemap exit)
    {
        this.map = map;
        this.exit = exit;
    }
}

/// <summary>
/// �� ������(������)�� �����ͼ� �� �����ϴ� Ŭ����
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public Tile backgroundTile;
    public Tile exitTile;

    /// <summary>
    /// �� ������ �ҷ��� ���õ�(ù�����Ҷ� 0�� �׻� ���� ��)
    /// </summary>
    public Tilemap[] roomSamples;

    /// <summary>
    /// ���� ������ �ҷ��� ���õ�(0 ����, 1 ����)
    /// </summary>
    public Tilemap[] passRoomSamples;

    /// <summary>
    /// Ÿ�� �ʵ�(0 ���, 1 �÷���, 2 ���Ա�)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
    /// </summary>
    Vector3Int cursor;

    /// <summary>
    /// ��η� ������ ��ġ(�� �ϳ� �����Ҷ����� �ӽ÷� ����)
    /// </summary>
    List<Vector3Int> passwayPos;

    private void Awake()
    {
        // Ÿ���� �׸� ���̾� �ҷ����� ����
        m_tileMaps = new Tilemap[transform.childCount];     
        for (int i = 0; i < transform.childCount; i++)
        {
            m_tileMaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }
    }

    private void Start()
    {
        cursor = new Vector3Int(0,0);

        passwayPos = new();

        GeneratingMap(roomSamples[0]);

        //List<Vector3Int> temp = new();
        //Tile[] test = new Tile[4];
        //for (int i = 0; i < 4; i++)
        //{
        //    temp.Add(new Vector3Int(i, i));
        //    test[i] = backgroundTile;
        //    Debug.Log($"{i}, {i}");
        //}
        
        
        //m_tileMaps[0].SetTiles(temp.ToArray(),test);


        //for (int i = 0; i < 3; i++)
        //{
        //    cursor += new Vector3Int(roomSamples[0].cellBounds.size.x, 0);

        //    GeneratingMap(roomSamples[0]);
        //}

    }

    void GeneratingPassway(int passSize, Vector3Int passStartPos)
    {

    }


    /// <summary>
    /// �޾ƿ� Ÿ�ϸ��� Ŀ�� ��ġ �������� �׸���(��浵 �Բ�)
    /// </summary>
    /// <param name="targetTileMap">���� Ÿ�ϸ�</param>
    void GeneratingMap(Tilemap targetTileMap)
    {
        passwayPos.Clear();

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
                else if(x == targetTileMap.cellBounds.xMin || x == targetTileMap.cellBounds.xMax - 1 || y == targetTileMap.cellBounds.yMin || y == targetTileMap.cellBounds.yMax - 1)
                {
                    if(CheckIsPassway(new Vector3Int(x,y), targetTileMap))
                    {
                        passwayPos.Add(targetTile);
                        Debug.Log($"{x}, {y} is passway!");
                    }
                }
                m_tileMaps[0].SetTile(targetTile, backgroundTile);      // ��� Ÿ�ϸʿ� �����
            }
        }
    }

    bool CheckIsPassway(Vector3Int targetPos, Tilemap targetMap)
    {
        int check = 9;
        for(int y = targetPos.y - 1; y <= targetPos.y + 1; y++)
        {
            for(int x = targetPos.x -1; x <= targetPos.x + 1; x++)
            {
                if(!targetMap.HasTile(new Vector3Int(x,y)))
                {
                    check--;   
                }
            }
        }

        return check == 0;
    }
}
