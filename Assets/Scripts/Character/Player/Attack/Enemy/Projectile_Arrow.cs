using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arrow : ProjectileBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);

        }

        CharacterBase characterTarget = collision.gameObject.GetComponent<CharacterBase>();

        if (characterTarget != null )
        {
            OnAttack(characterTarget);
            gameObject.SetActive(false);
        }
    }
}
