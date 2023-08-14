using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Action<float> onHealthChange { get; set; }

    public Action onDie { get; set; }

    public bool IsAlive => hp > 0;

    float IHealth.HP { get; set; }

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
}
