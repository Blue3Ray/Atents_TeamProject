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
    Projectile_Arrow,
    Explosion,          // ���� ����Ʈ
    BoneEnemy,
    ArcherEnemy
}

public class Factory : Singleton<Factory>
{
    //public GameObject playerBullet;
    
    ProjectilePool_Fire projectilePool_Fire;
    ProjectilePool_Water projectilePool_Water;
    ProjectilePool_Thunder projectilePool_Thunder;
    ProjectilePool_Wind projectilePool_Wind;
    ProjectilePool_Arrow projectilePool_Arrow;
    ExplosionPool_Explosion explosionPool;
    EnemyPool_BoneEnemy enemyPool_BoneEnemy;
    EnemyPool_ArcherEnemy enemyPool_ArcherEnemy;

    protected override void OnInitalize()
    {
        base.OnInitalize();

		projectilePool_Fire = transform.GetChild(0).GetComponent<ProjectilePool_Fire>();
        projectilePool_Fire?.Initialize();
        projectilePool_Water = transform.GetChild(1).GetComponent<ProjectilePool_Water>();
        projectilePool_Water?.Initialize();
        projectilePool_Thunder = transform.GetChild(2).GetComponent<ProjectilePool_Thunder>();
        projectilePool_Thunder?.Initialize();
        projectilePool_Wind = transform.GetChild(3).GetComponent<ProjectilePool_Wind>();
        projectilePool_Wind?.Initialize();
        projectilePool_Arrow = transform.GetChild(4).GetComponent<ProjectilePool_Arrow>();
        projectilePool_Arrow?.Initialize();

        explosionPool = transform.GetChild(5).GetComponent<ExplosionPool_Explosion>();
        explosionPool?.Initialize();

        enemyPool_BoneEnemy = transform.GetChild(6).GetComponent<EnemyPool_BoneEnemy>();
        enemyPool_BoneEnemy?.Initialize();
        enemyPool_ArcherEnemy = transform.GetChild(7).GetComponent<EnemyPool_ArcherEnemy>();
        enemyPool_ArcherEnemy?.Initialize();
    }


    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)
        {
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
            case PoolObjectType.Projectile_Arrow:
                result = projectilePool_Arrow.GetObject()?.gameObject;
                break;
            case PoolObjectType.Explosion:
                result = explosionPool.GetObject()?.gameObject;
                break;
            case PoolObjectType.BoneEnemy:
                result = enemyPool_BoneEnemy.GetObject()?.gameObject;
                break;
            case PoolObjectType.ArcherEnemy:
                result = enemyPool_ArcherEnemy.GetObject()?.gameObject;
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
