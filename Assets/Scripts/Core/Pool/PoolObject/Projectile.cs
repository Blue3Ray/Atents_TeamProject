using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    public ElementalType elementalType;

    /// <summary>
    /// 투사체가 날아가는 속도입니다.
    /// </summary>
    public float ProjectileSpeed = 1;

    /// <summary>
    /// 투사체가 날아갈 방향입니다.
    /// </summary>
    Vector3 dirProjectile = Vector3.right;

    /// <summary>
    /// 투사체와 character가 맞았을 때 외쳐질 델리게이트.
    /// </summary>
    public Action<CharacterBase, ElementalType> OnHit;


    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(LifeOver(2.0f));
    }

    private void Update()
    {
        transform.Translate(ProjectileSpeed * Time.deltaTime * dirProjectile);
    }

    /// <summary>
    /// 애니메이션의 이벤트키에서 실행될 함수입니다.
    /// </summary>
    public void EndAttack()
    {
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        //Character characterTarget = collision.gameObject.GetComponent<Character>();
        //if (characterTarget != null && !characterTarget.CompareTag("Player"))
        //{
        //    InvokeOnHit(characterTarget);
        //}
        //else
        //{
        //    // 투사체와 상호작용하는 일반 벽같은데 접촉할 때
        //    gameObject.SetActive(false);
        //}
    }

    protected virtual void InvokeOnHit(CharacterBase targetHit)
    {
        OnHit?.Invoke(targetHit, elementalType);
    }
}
