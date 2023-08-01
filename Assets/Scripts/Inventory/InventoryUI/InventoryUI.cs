using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	Inventory inventory;
	//�κ��丮�� �� UI���Ե�
	public InvenSlotUI[] UISlots;

	//�� ���� �迭�κ��� index�� �Է��ϸ� UIslot�� ��� �� �� �ִ� �迭 ������Ƽ
	public InvenSlotUI this[int index] => UISlots[index];

	TempSlotUI tempSlotUI = null;

	TrashCan trashCan;

	

	private void Awake()
	{
		
		Transform tempSlot = transform.GetChild(2);
		UISlots = GetComponentsInChildren<InvenSlotUI>();
		Transform tempTempSlot = transform.GetChild(3);
		this.tempSlotUI = tempTempSlot.GetComponent<TempSlotUI>();
		Transform tempTrashCan = transform.GetChild(4);
		trashCan = tempTrashCan.GetComponent<TrashCan>();

	}

	private void Start()
	{
		inventory = GameManager.Ins.inven;
		ConnetingSlots();
		trashCan.ClickTrashCan += OnTrashCan;
	}

	public void ConnetingSlots()
	{
		for(int i = 0; i < inventory.SlotCount; i++)
		{
			UISlots[i].invenSlot = inventory[(uint)i];
			UISlots[i].InitializeSlot();
			UISlots[i].onDragEnter += OnDragEnter;
			UISlots[i].onDragExit += OnDragExit;
			UISlots[i].onClick += OnClick;
			UISlots[i].onTrashcan += OnTrashCan;
			UISlots[i].onSplit += OnSplitItems;

			//UISlots[i].onDragging += OnDragging;
		}
		tempSlotUI.invenSlot = inventory.TempSlot;
		tempSlotUI.InitializeSlot();


	}


	private void OnTrashCan()
	{
		inventory.TempSlot.ClearSlotItem();
		
	}

	private void OnClick(uint obj)
	{
		if(tempSlotUI.invenSlot.ItemData != null)
		{
			inventory.MoveItem(tempSlotUI.invenSlot.Index, obj);
		}
	}

	//private void OnDragging()
	//{
	//	int i = 0;
	//	Debug.Log("�巡�� ��");
	//}

	private void OnDragExit(uint obj)
	{
		inventory.MoveItem(tempSlotUI.invenSlot.Index, obj);
	}

	private void OnDragEnter(uint obj)
	{
		inventory.MoveItem(obj, tempSlotUI.invenSlot.Index);
	}

	public void InventoryExit()
	{
		this.gameObject.SetActive(false);
	}
	private void OnSplitItems(uint slotIndex)
	{
		//��� ���� ���� �׳� �Ű�
		if (tempSlotUI.invenSlot.IsEmpty)
		{
			inventory.SplitItemToTemp(slotIndex);
		}
		//temp�� ���� ���� ���� itemData�� ���� ����
		else if(inventory[slotIndex].ItemData == tempSlotUI.invenSlot.ItemData)
		{
			if(!(tempSlotUI.invenSlot.ItemCount > tempSlotUI.invenSlot.ItemData.maxStackCount))
			{
				inventory.SplitItemToTemp(slotIndex);
			}

		}

	}
}
