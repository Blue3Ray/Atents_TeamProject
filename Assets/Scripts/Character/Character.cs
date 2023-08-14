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
}
