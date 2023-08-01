using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_NodeGenerate : TestBase
{
    RandomMap randomMap;

    private void Start()
    {
        randomMap = FindObjectOfType<RandomMap>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        randomMap.SetUpNodes();
    }
}
