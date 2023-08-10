using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    // HP 확인 밑 설정용 프로퍼티
    float HP {  get; set; }

    // 최대 HP 확인용 프로퍼티
    float MaxHP { get;}

    bool IsAlive { get; }

    System.Action<float> onHealthChange { get; set; }

    void Die();


}
