using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Trap : TestBase
{

    Icicle[] targets;

    private void Start()
    {
        targets = FindObjectsOfType<Icicle>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        foreach (var target in targets)
        {
            target.DropObject();
        }
        
    }

}
