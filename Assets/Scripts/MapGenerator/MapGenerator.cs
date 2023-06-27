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
    /// 방 데이터 불러올 샘플들(첫생성할때 0은 항상 시작 방)
    /// </summary>
    public Tilemap[] roomSamples;

    /// <summary>
    /// 복도 데이터 불러올 샘플들(첫생성할때 0은 항상 시작 방)
    /// </summary>
    public Tilemap[] passRoomSamples;

    /// <summary>
    /// 타일 맵들(0 배경, 1 플랫폼)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// 타일 그리는 위치
    /// </summary>
    Vector3Int cursor;

    /// <summary>
    /// 통로로 나가는 위치(맵 하나 생성할때마다 임시로 지정)
    /// </summary>
    List<Vector3Int> passwayPos;

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

        passwayPos = new();

        GeneratingMap(roomSamples[0]);

        Debug.Log(passwayPos.Count);


        //for (int i = 0; i < 3; i++)
        //{
        //    cursor += new Vector3Int(roomSamples[0].cellBounds.size.x, 0);

        //    GeneratingMap(roomSamples[0]);
        //}

    }

    /// <summary>
    /// 받아온 타일맵을 커서 위치 기준으로 그리기(배경도 함께)
    /// </summary>
    /// <param name="targetTileMap">샘플 타일맵</param>
    void GeneratingMap(Tilemap targetTileMap)
    {
        passwayPos.Clear();

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
                else if(x == targetTileMap.cellBounds.xMin || x == targetTileMap.cellBounds.xMax || y == targetTileMap.cellBounds.yMin || y == targetTileMap.cellBounds.yMax)
                {
                    if(CheckIsPassway(new Vector3Int(x,y), targetTileMap))
                    {
                        passwayPos.Add(targetTile);
                        Debug.Log($"{x}, {y}");
                    }
                }
                m_tileMaps[0].SetTile(targetTile, backgroundTile);      // 배경 타일맵에 배경을
            }
        }
    }

    bool CheckIsPassway(Vector3Int targetPos, Tilemap targetMap)
    {
        int check = 5;
        for(int y = targetPos.y - 1; y <= targetPos.y + 1; y++)
        {
            for(int x = targetPos.x -1; x <= targetPos.x +1; x++)
            {
                if(targetMap.HasTile(new Vector3Int(x,y)))
                {
                    check--;
                }
            }
        }

        return check == 0;
    }
}
