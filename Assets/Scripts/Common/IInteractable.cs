using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어떤 클래스가 반드시 특정 기능을 가지고 있어야 한다고 명시해 놓은 것
// 인터페이스는 기본적으로 public, 안에 있는 멤버도 public
// 인터페이스는 상속하는데 갯수의 제한이 없음
// 인터페이스에는 선언만 있음(구현은 없음)
// 인터페이스에는 변수가 들어갈 수 없음
// 

public interface IInteractable
{
    /// <summary>
    /// 상호작용한 대상을 직접 사용하는지, 간접 사용하는지 확인하는 프로퍼티
    /// </summary>
    bool IsDirectUse
    {
        get;
    }

    /// <summary>
    /// 사용가능한 기능
    /// </summary>
    void Use();
}

