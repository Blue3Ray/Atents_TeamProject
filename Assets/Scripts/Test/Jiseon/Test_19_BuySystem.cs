using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_19_BuySystem : TestBase
{
	protected override void Test1(InputAction.CallbackContext context)
	{
		GameManager.Ins.Player.inven.Money = 10000;
	}

}
