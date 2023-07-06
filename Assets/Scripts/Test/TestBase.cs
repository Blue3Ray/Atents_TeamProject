using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 앞으로 만들 모든 것에 대한 테스트 클래스의 부모클래스
/// </summary>
public class TestBase : MonoBehaviour
{
    ActionControl inputAction;
    private void Awake()
    {
        inputAction = new ActionControl();
    }

    private void OnEnable()
    {
        inputAction.Test.Enable();
        inputAction.Test.Test1.performed += Test1;
        inputAction.Test.Test2.performed += Test2;
        inputAction.Test.Test3.performed += Test3;
        inputAction.Test.Test4.performed += Test4;
        inputAction.Test.Test5.performed += Test5;
        inputAction.Test.TestClick.performed += TestClick;
    }

    private void OnDisable()
    {
        inputAction.Test.TestClick.performed -= TestClick;
        inputAction.Test.Test1.performed -= Test5;
        inputAction.Test.Test1.performed -= Test4;
        inputAction.Test.Test1.performed -= Test3;
        inputAction.Test.Test1.performed -= Test2;
        inputAction.Test.Test1.performed -= Test1;
        inputAction.Test.Disable();
    }

    protected virtual void Test1(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
    protected virtual void Test2(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
    protected virtual void Test3(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
    protected virtual void Test4(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
    protected virtual void Test5(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
    protected virtual void TestClick(UnityEngine.InputSystem.InputAction.CallbackContext context)    {    }
}
