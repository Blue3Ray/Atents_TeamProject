using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana 
{
    /// <summary>
    /// MP Ȯ�� �� ������ ������Ƽ
    /// </summary>
    float MP { get; set; }

    /// <summary>
    /// �ִ� MP Ȯ�ο� ������Ƽ
    /// </summary>
    float MaxMP { get; }

    /// <summary>
    /// MP�� ����� ������ ����� ��������Ʈ(�Ķ���ʹ� ����) �� ������Ƽ
    /// </summary>
    Action<float> onManaChange { get; set; }

    /// <summary>
    /// ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// MP�� ���������� �������� �ִ� �Լ�. �ʴ� totalRegen/duration��ŭ ȸ��
    /// </summary>
    /// <param name="totalRegen">��ü ȸ����</param>
    /// <param name="duration">��ü ȸ�� �ð�</param>
    void ManaRegenetate(float totalRegen, float duration);

    /// <summary>
    /// MP�� ƽ������ �������� �ִ� �Լ�. ��ü ȸ���� = tickRegen * totalTickCount
    /// </summary>
    /// <param name="tickRegen">ƽ �� ȸ����</param>
    /// <param name="tickTime">��ƽ���� �ð� ����</param>
    /// <param name="totalTickCount">ȸ���� ������ ��ü ƽ��</param>
    void ManaRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount);
}
