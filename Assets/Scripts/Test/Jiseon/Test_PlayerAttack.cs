using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ������ ���� ��� �Ϳ� ���� �׽�Ʈ Ŭ������ �θ�Ŭ����
/// </summary>
public class  Test_PlayerAttack: TestBase
{
	protected override void Test1(InputAction.CallbackContext context)
	{
		PlayerJS player = GameManager.Ins.playerTest1;
		player.PlayerElementalStatusChange(ElementalType.Fire);
		//player.PlayerElementalStatus = 
	}
}
