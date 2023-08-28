using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Projectile = 0,       // ����ü
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
    /// ������Ʈ�� Ǯ���� �������鼭 ��ġ�� ������ �����ϴ� �����ε� �Լ�
    /// </summary>
    /// <param name="type">������ ������Ʈ Ÿ��</param>
    /// <param name="position">������ ��ġ(����)</param>
    /// <param name="angle">ȸ�� ��ų ����(�⺻�� 0)</param>
    /// <returns>������ ������Ʈ</returns>
    public GameObject GetObject(PoolObjectType type, Vector3 position, float angle = 0.0f)
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.Rotate(angle * Vector3.forward);

        return obj;
    }
}
