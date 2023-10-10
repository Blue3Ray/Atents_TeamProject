using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_20_BackGroundMoving : TestBase
{

	protected override void Test1(InputAction.CallbackContext context)
	{
		base.Test1(context);
		GameManager.Ins.Player.inven.Money += 20000;
	}
}
