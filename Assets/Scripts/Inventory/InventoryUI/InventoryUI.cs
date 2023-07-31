using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	Inventory inventory;
	//인벤토리의 각 UI슬롯들
	public InvenSlotUI[] UISlots;

	//그 슬롯 배열로부터 index를 입력하면 UIslot을 얻어 올 수 있는 배열 프로퍼티
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
	//	Debug.Log("드래깅 중");
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
	private void OnSplitItems(uint slotIndex, uint SplitCount)
	{
		//임시 슬롯이 비어져 있고 나누려는 갯수가 ItemCount보다 작을 때만 실행
		//애초에 SplitCount를 유효한 숫자로 보내자.
		if (inventory.TempSlot.IsEmpty && SplitCount < inventory[slotIndex].ItemCount)
		{
			inventory.SplitItem(slotIndex, SplitCount);
		}
	
			
	}
}
