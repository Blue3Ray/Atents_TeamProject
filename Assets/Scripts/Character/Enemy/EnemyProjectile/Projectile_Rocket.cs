using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rocket : ProjectileBase
{
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

        Factory.Ins.GetObject(PoolObjectType.Explosion, transform.position + dirProjectile, Random.Range(0f, 360f));
    }
}
