using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SpawnEnemy : TestBase
{
    public Spawner spawner;
    protected override void Test1(InputAction.CallbackContext context)
    {
        spawner.SpawnBoneMonster();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        spawner.SpawnArcherMonster();
    }
}
