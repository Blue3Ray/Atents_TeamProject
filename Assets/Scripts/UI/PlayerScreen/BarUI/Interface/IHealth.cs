using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    /// <summary>
    /// HP Ȯ�� �� ������ ������Ƽ
    /// </summary>
    float HP { get; set; }

    /// <summary>
    /// �ִ� HP Ȯ�ο� ������Ƽ
    /// </summary>
    float MaxHP { get; }

    /// <summary>
    /// HP�� ����� ������ ����� ��������Ʈ(���� ��, �ִ� ��) �� ������Ƽ
    /// </summary>
    Action<float, float> onHealthChange { get; set; }

    /// <summary>
    /// ��� ó���� �Լ�
    /// </summary>
    void Die();

    /// <summary>
    /// ����� �˸��� ���� ��������Ʈ �� ������Ƽ
    /// </summary>
    Action onDie { get; set; }

    /// <summary>
    /// ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// ü���� ���������� �������� �ִ� �Լ�. �ʴ� totalRegen/duration��ŭ ȸ��
    /// </summary>
    /// <param name="totalRegen">��ü ȸ����</param>
    /// <param name="duration">��ü ȸ�� �ð�</param>
    void HealthRegenetate(float totalRegen, float duration);

    /// <summary>
    /// ü���� ƽ������ �������� �ִ� �Լ�. ��ü ȸ���� = tickRegen * totalTickCount
    /// </summary>
    /// <param name="tickRegen">ƽ �� ȸ����</param>
    /// <param name="tickTime">��ƽ���� �ð� ����</param>
    /// <param name="totalTickCount">ȸ���� ������ ��ü ƽ��</param>
    void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount);

}
