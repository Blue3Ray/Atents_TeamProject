using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Laser : ProjectileBase
{
    [Header("State")]
    public float laserDamage = 5.0f;
    
    public float tickMaxTime = 0.2f;
    float tickTime = -1f;

    bool canDamage = false;

    // 공격 범위 안에 들어온 타겟(Player으로 유일)
    [SerializeField]
    CharacterBase target;

    protected override void OnEnable()
    {
        base.OnEnable();
        projectileSpeed = 0;
    }

    public override void OnInitialize(Vector2 dir, ElementalType type)
    {
        base.OnInitialize(dir, type);
        Damage = laserDamage;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject;
        if(temp.CompareTag("Player"))
        {
            CharacterBase player = temp.GetComponent<CharacterBase>();
            target = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target != null && tickTime < 0 && canDamage)
        {
            OnAttack(target);
            StartCoroutine(tickCoolTime());
        }
    }

    IEnumerator tickCoolTime()
    {
        tickTime = tickMaxTime;
        while (tickTime > 0)
        {
            yield return null;
            tickTime -= Time.deltaTime;
        }
        tickTime = -1;
    }

    public void SetDamageOn()
    {
        canDamage = true;
    }

    public void SetDamageOff()
    {
        canDamage = false;
    }

}
