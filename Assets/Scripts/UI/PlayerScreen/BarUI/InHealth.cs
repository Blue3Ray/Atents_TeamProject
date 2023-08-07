using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InHealth
{
    // HP 확인 밑 설정용 프로퍼티
    float HP {  get; set; }

    // 최대 HP 확인용 프로퍼티
    float MaxHP { get;}

    // MP 확인 밑 설정용 프로퍼티
    float MP { get; set; }

    // 최대 MP 확인용 프로퍼티
    float MaxMP { get; }

    bool IsAlive { get; }

    Action<float> onHealthChange { get; set; }

    void Die();


}
