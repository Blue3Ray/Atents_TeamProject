using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Boss : TestBase
{
    Boss boss;

    private void Start()
    {
        boss = FindObjectOfType<Boss>();
    }

    int index = 1;

    protected override void Test1(InputAction.CallbackContext context)
    {
        //index++;
        //// Debug.Log(Enum.GetNames(typeof(BossPos)).Length);
        //if (index > Enum.GetNames(typeof(BossPos)).Length) index = 1;
        //boss.Teleport((BossPos)index);
        boss.Blink();
    }
}
