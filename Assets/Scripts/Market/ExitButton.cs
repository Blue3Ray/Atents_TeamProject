using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
	public Action IsExitMarket;

	Button buyButton;

	private void Awake()
	{
		buyButton = transform.GetComponent<Button>();
		buyButton.onClick.AddListener(ExitMarket);
	}

	private void ExitMarket()
	{
		IsExitMarket?.Invoke();
		Debug.Log("´­¸²");
	}
}
