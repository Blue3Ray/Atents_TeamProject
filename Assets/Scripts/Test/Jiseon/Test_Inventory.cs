using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickSlotTest : TestBase
{
	InventoryUI inventoryUI;
	Inventory inven;

	private void Start()
	{
		inventoryUI = FindAnyObjectByType<InventoryUI>();
	}

	protected override void Test1(InputAction.CallbackContext context)
	{
		base.Test1(context);
		inven = GameManager.Ins.player.inven;
		inven.AddItem(ItemCode.Jewerly);
	}

	protected override void Test2(InputAction.CallbackContext context)
	{
		base.Test2(context);
		inven.AddItem(ItemCode.Potion, 3);
		
	}

	protected override void Test3(InputAction.CallbackContext context)
	{
		base.Test3(context);
	}

	protected override void Test4(InputAction.CallbackContext context)
	{
		base.Test4(context);
	}

	protected override void Test5(InputAction.CallbackContext context)
	{
		base.Test5(context);
	}
}
