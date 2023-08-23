using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 앞으로 만들 모든 것에 대한 테스트 클래스의 부모클래스
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
