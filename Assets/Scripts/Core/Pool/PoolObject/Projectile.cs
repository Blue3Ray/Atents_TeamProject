using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    public ElementalType elementalType;

    /// <summary>
    /// ����ü�� ���ư��� �ӵ��Դϴ�.
    /// </summary>
    public float ProjectileSpeed = 1;

    /// <summary>
    /// ����ü�� ���ư� �����Դϴ�.
    /// </summary>
    Vector3 dirProjectile = Vector3.right;

    /// <summary>
    /// ����ü�� character�� �¾��� �� ������ ��������Ʈ.
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
    /// �ִϸ��̼��� �̺�ƮŰ���� ����� �Լ��Դϴ�.
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
        //    // ����ü�� ��ȣ�ۿ��ϴ� �Ϲ� �������� ������ ��
        //    gameObject.SetActive(false);
        //}
    }

    protected virtual void InvokeOnHit(CharacterBase targetHit)
    {
        OnHit?.Invoke(targetHit, elementalType);
    }
}
