using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ������ ���� ��� �Ϳ� ���� �׽�Ʈ Ŭ������ �θ�Ŭ����
/// </summary>
public class  Test_PlayerAttack: TestBase
{
	PlayerJS player;

    private void Start()
    {
		player = GameManager.Ins.player;
    }

    protected override void Test1(InputAction.CallbackContext context)
	{
		player.ChangeActivateAttack(ElementalType.Fire);
		//player.PlayerElementalStatus = 
	}

	protected override void Test2(InputAction.CallbackContext context)
	{
		player.ChangeActivateAttack(ElementalType.Water);
	}

	protected override void Test3(InputAction.CallbackContext context)
	{
		player.ChangeActivateAttack(ElementalType.Thunder);
	}

	protected override void Test4(InputAction.CallbackContext context)
	{
		player.ChangeActivateAttack(ElementalType.Wind);
	}

	protected override void Test5(InputAction.CallbackContext context)
	{
		player.inven.Money += 100;
	}
}
