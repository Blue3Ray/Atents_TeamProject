using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    /// <summary>
    /// �� ������Ʈ�� Ʈ������(������ �ʿ� ����)
    /// </summary>
    Transform transform { get; }

    /// <summary>
    /// ���ݷ� Ȯ�ο� ������Ƽ
    /// </summary>
    float AttackPower { get; }

    /// <summary>
    /// ���� Ȯ�ο� ������Ƽ
    /// </summary>
    float DefencePower { get; }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    /// <param name="target">������ ���</param>
    void Attack(IBattle target);

    /// <summary>
    /// ��� �Լ�
    /// </summary>
    /// <param name="damage">���� ���� ������</param>
    void Defence(ElemantalStatus elemantal , float damage);
}
