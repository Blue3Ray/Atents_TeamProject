using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBlock : MonoBehaviour
{
	public uint priceOfItem = 0;

	TextMeshProUGUI itemName;
	TextMeshProUGUI itemPrice;
	
	Image itemImage;
	
	ItemCode itemcode;

	private void Awake()
	{
		itemName = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		itemPrice = transform.GetChild(6).GetComponent<TextMeshProUGUI>();

		itemImage = transform.GetChild(4).GetComponent<Image>();
		itemImage.color = Color.clear;
	}

	public void Refresh(ItemCode code)
	{
		itemcode = code;
		itemName.text = GameManager.Ins.ItemData[code].itemName;
		priceOfItem = GameManager.Ins.ItemData[code].price;
		itemPrice.text = priceOfItem.ToString();
		itemImage.sprite = GameManager.Ins.ItemData[code].itemIcon;
		itemImage.color = Color.white;
	}

	public void Buy()
	{
		if(priceOfItem < GameManager.Ins.Player.inven.Money)
		{
			GameManager.Ins.Player.inven.Money -= (int)priceOfItem;
			GameManager.Ins.Player.inven.AddItemExeptQuickSlot(itemcode);
		}
	}
}
