
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


// 속성 선택 enum


public class Character : MonoBehaviour, IHealth
{
    [SerializeField]

    //HP 
    protected float hp;
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
                hp = Mathf.Clamp(hp, 0, MaxHP);
                onHealthChange?.Invoke(hp / MaxHP);
            }
            
        }
    }

    protected float maxHP;
    public float MaxHP => maxHP;

    public System.Action<float> onHealthChange { get; set; }

    public System.Action onDie { get; set; }

    public bool IsAlive => hp > 0;

    

    // 공격력
    protected float attack;
    public float Attack    // 공격력 프로퍼티
    {
        get { return attack; }
        set
        {
            attack = value;
        }
    }
    public System.Action<float> onAttackChange { get; set; }

    // 방어력
    protected float defence;
    public float Defence  // 방어력 프로퍼티
    {
        get { return defence; } 
        set
        {
            defence = value;
        }
    }
    public System.Action<float> onDefenceChange { get; set; }
    float IHealth.HP { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    // ElemantalStatus 클래스 호출
    ElemantalStatus elemantalStatus;

    // 속성 공격력
    public  float elemantalAttack;
    // 속성 방어력
    public float elemantalDefence;

    private void Awake()
    {
        elemantalStatus= new ElemantalStatus();
    }

    // 사망 처리용 함수
    public void Die()
    {
        if (hp <= 0)
        {
            onDie?.Invoke();
            gameObject.SetActive(false);
        }
    }

    // 체력으로 틱 단위로 증가 시키는 함수
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

    // 체력을 지속적으로 회복시키는 함수
    public void HealthRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator HealthRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // 초당 회복량 계산
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // 시간 카운팅
            HP += Time.deltaTime * regenPerSec;     // 초당 회복량만큼 증가
            yield return null;
        }
    }



    // 속성 선택 함수
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

    // 속성 업그레이드 함수
    public void ElemantalUpgrade(int elemantalLevel)
    {

    }

    
}
