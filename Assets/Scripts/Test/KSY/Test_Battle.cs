using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : TestBase
{
    public Test_PlayerCharacter player;
    public CharacterBase enemy;

    private void Start()
    {

    }


    protected override void Test1(InputAction.CallbackContext context)
    {
        player.Attack(enemy);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        enemy.Attack(player);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        IExperience target = player.GetComponent<IExperience>();
        if(target != null)
        {
            target.Experience += 10;
        }
    }

}
