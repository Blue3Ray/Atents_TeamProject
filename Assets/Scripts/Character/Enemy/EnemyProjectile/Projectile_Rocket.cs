using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rocket : ProjectileBase
{
    Transform target;

    protected override void OnEnable()
    {
        base.OnEnable();
        projectileSpeed = 5;
    }

    //원랜 타겟을 발사한 오브젝트에서 지정해 줘야 하는데... 그냥 플레이어를 죽이자
    public override void OnInitialize(Vector2 dir, ElementalType type)
    {
        base.OnInitialize(dir, type);
        target = GameManager.Ins.Player.transform;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Factory.Ins.GetObject(PoolObjectType.Explosion, transform.position + dirProjectile, Random.Range(0f, 360f));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.TryGetComponent<CharacterBase>(out var characterTarget))
        {
            OnAttack(characterTarget);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {        
        transform.Translate(dirProjectile * projectileSpeed * Time.deltaTime);
        Vector3 dir = target.position - transform.position;
        transform.right += Vector3.Lerp(transform.right * dirProjectile.x, dir, Time.deltaTime * 0.3f) * dirProjectile.x;
    }
}
