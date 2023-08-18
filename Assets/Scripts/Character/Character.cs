
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


// �Ӽ� ���� enum


public class Character : MonoBehaviour, IHealth
{
  //  [SerializeField]

    //HP 
    protected  float hp  = 1.0f;
     public float HP
    {
        get => hp;
        set 
        { 
            if(IsAlive)
            {
                hp = value;
                if (hp <= 0)
                {     
                    Die();
                }
                hp = Mathf.Clamp(HP, 0, MaxHP);
                onHealthChange?.Invoke(HP / MaxHP);
            }
            
        }
    }

    protected float maxHP;
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
        }
    }
    

    public System.Action<float> onHealthChange { get; set; }

    public System.Action onDie { get; set; }

    public bool IsAlive => hp > 0;

    

    // ���ݷ�
    protected float attack;
    public float Attack    // ���ݷ� ������Ƽ
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
    public float Defence  // ���� ������Ƽ
    {
        get { return defence; } 
        set
        {
            defence = value;
        }
    }
    public System.Action<float> onDefenceChange { get; set; }
   


    // ElemantalStatus Ŭ���� ȣ��
    ElemantalStatus elemantalStatus;

    // �Ӽ� ���ݷ�
    public  float elemantalAttack;
    // �Ӽ� ����
    public float elemantalDefence;

    private void Awake()
    {
        elemantalStatus= new ElemantalStatus();
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



    // �Ӽ� ���� �Լ�
    public void ElemantalSelect(Elemantal elemantal)
    {
        switch(elemantal)
        {
            case Elemantal.Fire:
                elemantalStatus.elemantal = Elemantal.Fire; 
                break;
            case Elemantal.Water:
                elemantalStatus.elemantal = Elemantal.Water;
                break;
            case Elemantal.Wind:
                elemantalStatus.elemantal = Elemantal.Wind;
                break;
            case Elemantal.Thunder:
                elemantalStatus.elemantal = Elemantal.Thunder;
                break;
             default:
                elemantalStatus.elemantal = Elemantal.None;
                break;
        }
    }

    // �Ӽ� ���׷��̵� �Լ�
    public void ElemantalUpgrade(int elemantalLevel)
    {

    }

    
}
