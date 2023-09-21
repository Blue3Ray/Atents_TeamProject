using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform pool;
    public Transform[] spawnPositions;


    private void Awake()
    {
        pool = transform.GetChild(0);

        spawnPositions = new Transform[transform.childCount - 1];

        for(int i = 1; i < transform.childCount; i++)
        {
            spawnPositions[i - 1] = transform.GetChild(i);
            Debug.Log($"{i - 1}번째에 들어간 오브젝트 이름은 {spawnPositions[i - 1].name}");
        }
    }


    public void SpawnBoneMonster()
    { 
        Factory.Ins.GetObject(PoolObjectType.BoneEnemy, spawnPositions[0].position);
    }

    public void SpawnArcherMonster()
    {
        Factory.Ins.GetObject(PoolObjectType.ArcherEnemy, spawnPositions[0].position);
    }
}
