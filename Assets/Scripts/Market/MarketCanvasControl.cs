using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketCanvasControl : MonoBehaviour
{
	GameObject marketWindow;

	private void Awake()
	{
		Transform transformM = transform.GetChild(1);
		marketWindow = transformM.gameObject;
		
	}


}
