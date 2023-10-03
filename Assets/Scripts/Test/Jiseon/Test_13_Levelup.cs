using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_13_Levelup : TestBase
{

	PlayerJS player;
	private void Start()
	{
		player = GameManager.Ins.Player;
	}

	protected override void Test1(InputAction.CallbackContext context)
	{
		base.Test1(context);
		player.Experience += 100;
	}
}
