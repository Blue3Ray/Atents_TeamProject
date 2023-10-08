using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{

	Material merchantShader;

	private void Awake()
	{
		merchantShader = transform.GetComponent<Renderer>().sharedMaterial;
	}

	private void Start()
	{
		transform.GetChild(0).GetComponent<Merchant_Sensor>().playerCloseToMerchant += (isIn) => CreatOutLine(isIn);
	}

	void CreatOutLine(bool isIn)
	{
		if (isIn)
		{
			merchantShader.SetFloat("_line", 0.002f);
		}
		else
		{
			merchantShader.SetFloat("_line", 0);
		}
	}


}
