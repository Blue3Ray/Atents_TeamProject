using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// � Ŭ������ �ݵ�� Ư�� ����� ������ �־�� �Ѵٰ� ����� ���� ��
// �������̽��� �⺻������ public, �ȿ� �ִ� ����� public
// �������̽��� ����ϴµ� ������ ������ ����
// �������̽����� ���� ����(������ ����)
// �������̽����� ������ �� �� ����
// 

public interface IInteractable
{
    /// <summary>
    /// ��ȣ�ۿ��� ����� ���� ����ϴ���, ���� ����ϴ��� Ȯ���ϴ� ������Ƽ
    /// </summary>
    bool IsDirectUse
    {
        get;
    }

    /// <summary>
    /// ��밡���� ���
    /// </summary>
    void Use();
}

