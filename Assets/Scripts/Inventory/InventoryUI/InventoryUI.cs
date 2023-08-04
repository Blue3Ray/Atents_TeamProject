using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
	Inventory inventory;

	public List<InvenSlotUI> UISlotsList;
	//�κ��丮�� �� UI���Ե�
	public InvenSlotUI[] UISlots;

	//�� ���� �迭�κ��� index�� �Է��ϸ� UIslot�� ��� �� �� �ִ� �迭 ������Ƽ
	public InvenSlotUI this[int index] => UISlots[index];

	TempSlotUI tempSlotUI = null;

	TrashCan trashCan;

	CanvasGroup canvasGroup;

	ActionControl inputActions;


	private void Awake()
	{
		inputActions = new();
		UISlotsList = new List<InvenSlotUI>();
		canvasGroup = transform.GetComponent<CanvasGroup>();
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0;
		Transform tempSlot = transform.GetChild(2);


		var invenUISlots = GetComponentsInChildren<InvenSlotUI>();

		foreach(var slots in invenUISlots)
		{
			UISlotsList.Add(slots);
		}
			
		Transform tempTempSlot = transform.GetChild(3);
		this.tempSlotUI = tempTempSlot.GetComponent<TempSlotUI>();
		Transform tempTrashCan = transform.GetChild(4);
		trashCan = tempTrashCan.GetComponent<TrashCan>();
	}

	private void OnEnable()
	{
		inputActions.Inventory.Inventory.Enable();
		inputActions.Inventory.Inventory.performed += OnOffInventory;
	}


	private void OnDisable()
	{
		inputActions.Inventory.Inventory.performed -= OnOffInventory;
		inputActions.Inventory.Inventory.Disable();
		
	}

	private void Start()
	{
		var QuickSlotUIs = GameObject.FindGameObjectsWithTag("QuickSlot");
		foreach(var QuickSlot in QuickSlotUIs)
		{
			InvenSlotUI tmpInvenSlotOfQuickSlot = QuickSlot.transform.GetComponent<InvenSlotUI>();
			UISlotsList.Add(tmpInvenSlotOfQuickSlot);
		}
		UISlots = UISlotsList.ToArray();
		ConnetingSlots();
		trashCan.ClickTrashCan += OnTrashCan;
	}



	public void ConnetingSlots()
	{
		inventory = GameManager.Ins.playerTest.inven;
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

	private void OnSplitItems(uint slotIndex)
	{
		
		//��� ���� ���� �׳� �Ű�
		if (tempSlotUI.invenSlot.IsEmpty)
		{
			inventory.SplitItemToTemp(slotIndex);
		}
		//temp�� ���� ���� ���� itemData�� ���� ����
		else if (inventory[slotIndex].ItemData == tempSlotUI.invenSlot.ItemData)
		{
			if (!(tempSlotUI.invenSlot.ItemCount > tempSlotUI.invenSlot.ItemData.maxStackCount))
			{
				inventory.SplitItemToTemp(slotIndex);
			}

		}
		
	}

	private void OnOffInventory(InputAction.CallbackContext obj)
	{
		if (canvasGroup.interactable)
		{

			CloseInventory();
		}
		else
		{
			OpenInventory();
		}
	}
	private void OpenInventory()
	{
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.alpha = 1;
	}
	public void CloseInventory()
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0;
	}
}
