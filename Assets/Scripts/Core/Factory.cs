using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Projectile = 0,       // ����ü
    Projectile_Fire,
    Projectile_Water,
    Projectile_Thunder,
    Projectile_Wind,
    BoneEnemy
}

public class Factory : Singleton<Factory>
{
    //public GameObject playerBullet;
    ProjectilePool projectilePool;
    ProjectilePool_Fire projectilePool_Fire;
    ProjectilePool_Water projectilePool_Water;
    ProjectilePool_Thunder projectilePool_Thunder;
    ProjectilePool_Wind projectilePool_Wind;
    EnemyPool_BoneEnemy enemyPool_BoneEnemy;

    protected override void OnInitalize()
    {
        base.OnInitalize();

        projectilePool = transform.GetChild(0).GetComponent<ProjectilePool>();
        projectilePool?.Initialize();
		projectilePool_Fire = transform.GetChild(1).GetComponent<ProjectilePool_Fire>();
        projectilePool_Fire?.Initialize();
        projectilePool_Water = transform.GetChild(2).GetComponent<ProjectilePool_Water>();
        projectilePool_Water?.Initialize();
        projectilePool_Thunder = transform.GetChild(3).GetComponent<ProjectilePool_Thunder>();
        projectilePool_Thunder?.Initialize();
        projectilePool_Wind = transform.GetChild(4).GetComponent<ProjectilePool_Wind>();
        projectilePool_Wind?.Initialize();

        enemyPool_BoneEnemy = transform.GetChild(5).GetComponent<EnemyPool_BoneEnemy>();
        enemyPool_BoneEnemy?.Initialize();


    }


    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Projectile:
                result = projectilePool.GetObject()?.gameObject;
                break;
            case PoolObjectType.Projectile_Fire:
                result = projectilePool_Fire.GetObject()?.gameObject;
                break;
            case PoolObjectType.Projectile_Water:
                result = projectilePool_Water.GetObject()?.gameObject;
                break;
            case PoolObjectType.Projectile_Thunder:
                result = projectilePool_Thunder.GetObject()?.gameObject;
                break;
            case PoolObjectType.Projectile_Wind:
                result = projectilePool_Wind.GetObject()?.gameObject;
                break;
            case PoolObjectType.BoneEnemy:
                result = enemyPool_BoneEnemy.GetObject()?.gameObject;
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
  //      ProjectileBase projectile = obj.GetComponent<ProjectileBase>();
  //      projectile.dirProjectile = Vector3.right;
  //      if(angle == 180)
  //      {
		//	if (projectile != null)
		//	{
  //              projectile.dirProjectile = Vector3.left;
		//	}
		//}
        return obj;
    }
}
