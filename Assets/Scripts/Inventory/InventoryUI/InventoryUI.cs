using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	Inventory inventory;
	public InvenSlotUI[] UISlots;
	public InvenSlotUI this[int index] => UISlots[index];
	//InvenSlotUI tempSlotUI;



	private void Awake()
	{
		Transform child = transform.GetChild(2);
		UISlots = GetComponentsInChildren<InvenSlotUI>();
	}

	private void Start()
	{
		inventory = GameManager.Ins.inven;
		ConnetingSlots();
	}

	public void ConnetingSlots()
	{
		for(int i = 0; i < inventory.SlotCount; i++)
		{
			UISlots[i].invenSlot = inventory[(uint)i];
			//UISlots[i].onDragEnter += OnDragEnter;
			//UISlots[i].onDragExit += OnDragExit;
			//UISlots[i].onDragging += OnDragging;
		}
		//tempSlotUI.invenSlot = inventory.tempSlot;


	}

	private void OnDragging()
	{
		throw new NotImplementedException();
	}

	private void OnDragExit(uint obj)
	{
		throw new NotImplementedException();
	}

	private void OnDragEnter(uint obj)
	{
		throw new NotImplementedException();
	}
}
