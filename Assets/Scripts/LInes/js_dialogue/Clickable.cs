using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾�κ��� Ŭ���� �����ؾ� �ϴ� �������̽�
/// </summary>
public interface IClickable
{
    public void OnClicking(IClickable tmp);
}
