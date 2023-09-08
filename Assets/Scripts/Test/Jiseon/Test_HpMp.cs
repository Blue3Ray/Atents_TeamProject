using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_HpMp : TestBase
{
    PlayerJS player;

    void Start()
    {
        player = GameManager.Ins.player;
    }

	protected override void Test1(InputAction.CallbackContext context)
	{
        player.MP -= 10;
        //player.HP -= 10;
	}

	protected override void Test2(InputAction.CallbackContext context)
	{
		player.MP += 10;
		//player.HP += 10;
	}


}
