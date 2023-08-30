using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerFactory : TestBase
{
	protected override void Test1(InputAction.CallbackContext context)
	{
		Factory.Ins.GetObject(PoolObjectType.Projectile);
	}
}
