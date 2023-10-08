using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Sensor : MonoBehaviour
{
	public Action<bool> playerCloseToMerchant;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerCloseToMerchant?.Invoke(true);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerCloseToMerchant?.Invoke(false);
		}
	}
}
