using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Switch : TestBase
{
    IInteractable target;

    private void Start()
    {
        target = FindObjectOfType<Switch>().GetComponent<IInteractable>();  
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        target.Use();
    }
}
