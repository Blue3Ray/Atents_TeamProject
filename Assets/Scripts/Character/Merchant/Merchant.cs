using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{

	public Action OnMarket;
	bool marketReady = false;
	Material merchantShader;

	private void Awake()
	{
		merchantShader = transform.GetComponent<Renderer>().sharedMaterial;
	}

	private void Start()
	{
		transform.GetChild(0).GetComponent<Merchant_Sensor>().playerCloseToMerchant += (isIn) => CreatOutLine(isIn);
		GameManager.Ins.Player.onUsePerformed += MarketOn;
		OnMarket += FindAnyObjectByType<Canvas>().GetComponentInChildren<MarketControle>().MarketOn;
	}

	private void MarketOn()
	{
		if (marketReady) 
		{
			OnMarket?.Invoke();
		}
	}

	void CreatOutLine(bool isIn)
	{
		if (isIn)
		{
			merchantShader.SetFloat("_line", 0.002f);
			marketReady = true;
		}
		else
		{
			merchantShader.SetFloat("_line", 0);
			marketReady = false;
		}
	}


}
