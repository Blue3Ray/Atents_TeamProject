using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Projectile = 0,       // 투사체
}

public class Factory : Singleton<Factory>
{
    //public GameObject playerBullet;
    ProjectilePool projectilePool;


    protected override void OnInitalize()
    {
        base.OnInitalize();

        projectilePool = GetComponentInChildren<ProjectilePool>();

        projectilePool?.Initialize();
    }


    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Projectile:
                result = projectilePool.GetObject()?.gameObject;
                break;
            default:
                result = new GameObject();
                break;
        }

        return result;
    }

    /// <summary>
    /// 오브젝트를 풀에서 가져오면서 위치와 각도를 설정하는 오버로딩 함수
    /// </summary>
    /// <param name="type">생성할 오브젝트 타입</param>
    /// <param name="position">생성할 위치(월드)</param>
    /// <param name="angle">회전 시킬 각도(기본값 0)</param>
    /// <returns>생성한 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position, float angle = 0.0f)
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.Rotate(angle * Vector3.forward);

        return obj;
    }
}
