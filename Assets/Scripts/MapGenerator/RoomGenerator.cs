using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// ����� �׸� Ÿ��
    /// </summary>
    public Tile backgroundTile;

    /// <summary>
    /// �ⱸ�� �����ϱ����� ���� Ÿ��
    /// </summary>
    public Tile exitTile;

    /// <summary>
    /// �� ������ �ҷ��� ���õ�(ù�����Ҷ� 0�� �׻� ���� ��)
    /// </summary>
    public SampleRoomData[] roomSamplesWithExit;

    /// <summary>
    /// Ÿ���� �׸� �ʵ�(0 ���, 1 �÷���, 2 ���Ա�)
    /// </summary>
    Tilemap[] m_tileMaps;

    /// <summary>
    /// Ÿ�� �׸��� ��ġ
    /// </summary>
    Vector3Int cursor;

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
        cursor = new Vector3Int(0, 0);

        foreach(SampleRoomData temp in roomSamplesWithExit)
        {
            temp.OnInitialize();
        }

        // ����
    }


}
