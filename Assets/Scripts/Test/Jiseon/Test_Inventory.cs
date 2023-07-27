using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Inventory : TestBase
{
	InventoryUI inventoryUI;
	Inventory inven;

	private void Start()
	{
		inventoryUI = FindAnyObjectByType<InventoryUI>();
		inven = GameManager.Ins.inven;
	}

	protected override void Test1(InputAction.CallbackContext context)
	{
		inven.AddItem(ItemCode.Jewerly);
	}

	protected override void Test2(InputAction.CallbackContext context)
	{
		inventoryUI.UISlots[0].refresh();
		
	}

	protected override void Test3(InputAction.CallbackContext context)
	{
		GameObject gameObject = GameObject.Find("Image");
		Image image1 = gameObject.GetComponent<Image>();
		//Image image = gameObject.GetComponent<Image>();
		image1.sprite = GameManager.Ins.ItemData.itemDatas[0].itemIcon;
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
