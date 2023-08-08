using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana 
{
    // MP 확인 밑 설정용 프로퍼티
    float MP { get; set; }

    // 최대 HP 확인용 프로퍼티
    float MaxMP { get; }
}
