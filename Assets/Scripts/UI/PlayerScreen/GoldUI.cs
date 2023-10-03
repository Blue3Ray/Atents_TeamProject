using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    TextMeshProUGUI goldIndex;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        goldIndex = child.GetComponent<TextMeshProUGUI>();
    }

	private void Start()
	{
        GameManager.Ins.Player.inven.OnMoneyChange = (money) => RefreshData(money);
	}

	public void RefreshData(int gold)
    {
        
        goldIndex.text = gold.ToString();
    }
}
