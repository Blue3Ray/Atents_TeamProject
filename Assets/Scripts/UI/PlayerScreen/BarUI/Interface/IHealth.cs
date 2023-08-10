using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    // HP Ȯ�� �� ������ ������Ƽ
    float HP {  get; set; }

    // �ִ� HP Ȯ�ο� ������Ƽ
    float MaxHP { get;}

    bool IsAlive { get; }

    System.Action<float> onHealthChange { get; set; }

    void Die();


}
