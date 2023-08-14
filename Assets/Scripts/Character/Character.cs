
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


// �Ӽ� ���� enum
enum ESelct
{
    None,
    Fire,
    Water,
    Wind,
    Thunder
}

public class Character : MonoBehaviour, IHealth
{
    [SerializeField]
    protected float hp;
    public float HP
    {
        get { return hp; }
        set 
        { 
            hp = value; 
            if(hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }

    protected float maxHP;
    public float MaxHP => maxHP;

    public System.Action<float> onHealthChange { get; set; }

    public System.Action onDie { get; set; }

    public bool IsAlive => hp > 0;

    float IHealth.HP { get; set; }

    // ���ݷ�
    protected float attack;
    public float Attack
    {
        get { return attack; }
        set
        {
            attack = value;
        }
    }
    public System.Action<float> onAttackChange { get; set; }

    // ����
    protected float defence;
    public float Defence 
    {
        get { return defence; } 
        set
        {
            defence = value;
        }
    }
    public System.Action<float> onDefenceChange { get; set; }

    private void Awake()
    {
        ESelct eSelct = ESelct.None;
    }

    // ��� ó���� �Լ�
    public void Die()
    {
        if (hp <= 0)
        {
            onDie?.Invoke();
            gameObject.SetActive(false);
        }
    }

    // ü������ ƽ ������ ���� ��Ű�� �Լ�
    public void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(HealthRegenerateByTickCoroutine(tickRegen, tickTime, totalTickCount));
    }

    IEnumerator HealthRegenerateByTickCoroutine(float tickRegen, float tickTime, uint totalTickCount)
    {
        WaitForSeconds wait = new WaitForSeconds(tickTime);
        for (uint tickCount = 0; tickCount < totalTickCount; tickCount++)
        {
            HP += tickRegen;
            yield return wait;
        }
    }

    // ü���� ���������� ȸ����Ű�� �Լ�
    public void HealthRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator HealthRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // �ʴ� ȸ���� ���
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // �ð� ī����
            HP += Time.deltaTime * regenPerSec;     // �ʴ� ȸ������ŭ ����
            yield return null;
        }
    }


    // index : �÷��� ��� ���� ���� �޴� ����
    // index : Enemy ��� ���̵� ���� �޴� ����

    // ���ݷ� ���� �Լ�
    protected virtual void AttackIncrease(int index)
    {

    }

    // ���� ���� �Լ�
    protected virtual void DefenceIncrease(int index)
    {

    }

    // �Ӽ� ���� �Լ�
    public void ElemantalSelect()
    {
        
    }
}
