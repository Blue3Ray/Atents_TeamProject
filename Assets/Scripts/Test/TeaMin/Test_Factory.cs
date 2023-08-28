using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Test_Factory : TestBase
{
    public Transform spwanPos;
    protected override void Test1(InputAction.CallbackContext context)
    {
        if (spwanPos != null) Factory.Ins.GetObject(PoolObjectType.Projectile, spwanPos.position);
        else Factory.Ins.GetObject(PoolObjectType.Projectile);
    }
}
