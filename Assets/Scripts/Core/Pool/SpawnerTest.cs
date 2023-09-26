using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnerTest : MonoBehaviour
{
    public Vector3Int[] spawnPositions;

    private void Awake()
    {
        //spawnPositions = new Transform[transform.childCount - 1];

        //for(int i = 1; i < transform.childCount; i++)
        //{
        //    spawnPositions[i - 1] = transform.GetChild(i);
        //    Debug.Log($"{i - 1}번째에 들어간 오브젝트 이름은 {spawnPositions[i - 1].name}");
        //}
    }

    public void SpawnEnemyAtAllPos()
    {
        
        for(int i = 1; i < spawnPositions.Length; i++)
        { 
            Vector3 pos = spawnPositions[i];
            if (Random.value > 0.4f)
            {
                SpawnBoneMonster(pos);
            }
            else
            {
                SpawnArcherMonster(pos);
            }
        }
    }

    public void SetPlayerPos()
    {
        GameManager.Ins.player.transform.position = spawnPositions[0];
    }

    public void GetSpawnPoses(Vector3Int[] positions)
    {
        spawnPositions = positions;
    }

    public void GetSpawnPoses(List<Vector3Int> positions)
    {
        GetSpawnPoses(positions.ToArray());
    }


    public void SpawnBoneMonster(Vector3 pos)
    { 
        Factory.Ins.GetObject(PoolObjectType.BoneEnemy, pos);
    }

    public void SpawnArcherMonster(Vector3 pos)
    {
        Factory.Ins.GetObject(PoolObjectType.ArcherEnemy, pos);
    }
}
