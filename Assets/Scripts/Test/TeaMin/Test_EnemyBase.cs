using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyBase : TestBase
{
    public float knockBackPwr;

    EnemyBase enemy;


    private void Start()
    {
        enemy = FindObjectOfType<EnemyBase>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        //enemy.Defence(20, 10);
    }
}
