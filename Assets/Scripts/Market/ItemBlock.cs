using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBlock : MonoBehaviour
{
	TextMeshProUGUI itemName;
	TextMeshProUGUI itemPrice;
	ItemCode itemcode;

	Image itemImage;
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
		itemPrice.text = GameManager.Ins.ItemData[code].price.ToString();
		itemImage.sprite = GameManager.Ins.ItemData[code].itemIcon;
		itemImage.color = Color.white;
	}

	public void Buy()
	{
		GameManager.Ins.playerTest.inven.AddItem(itemcode);
	}
}
