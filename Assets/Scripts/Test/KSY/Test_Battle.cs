using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : MonoBehaviour
{
   Test_PlayerCharater player;
   Test_EnemyCharater enemy;

    ActionControl inputAction;

    private void Awake()
    {
        player = FindObjectOfType<Test_PlayerCharater>();
        enemy =FindObjectOfType<Test_EnemyCharater>();

        inputAction = new ActionControl();  
    }

    private void OnEnable()
    {
        inputAction.Test.Enable();
        inputAction.Test.Test1.performed += Test1;
       // inputAction.Test.Test2.performed += Test2;
       // inputAction.Test.Test3.performed += Test3;
       // inputAction.Test.Test4.performed += Test4;
       // inputAction.Test.Test5.performed += Test5;
       // inputAction.Test.TestClick.performed += TestClick;
    }

   

    private void OnDisable()
    {
        // inputAction.Test.TestClick.performed -= TestClick;
        // inputAction.Test.Test1.performed -= Test5;
        // inputAction.Test.Test1.performed -= Test4;
        // inputAction.Test.Test1.performed -= Test3;
        // inputAction.Test.Test1.performed -= Test2;
        inputAction.Test.Test1.performed -= Test1;
        inputAction.Test.Disable();
    }

    private void Test1(InputAction.CallbackContext _)
    {
        player.HP -= 10.0f;
    }

}
