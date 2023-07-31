using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketScroll : MonoBehaviour
{
	Scrollbar scrollbar;
	Transform Handle;
	float viewPortX;
	float contentX;
	float DivideContentX;

	private void Awake()
	{
		Transform scrollT = transform.GetChild(1);
		scrollbar = scrollT.GetComponent<Scrollbar>();
		Transform viewT = transform.GetChild(0);
		viewPortX = viewT.position.x;
		Transform contentT = viewT.GetChild(0);
		contentX = contentT.position.x;
		DivideContentX = 1 / contentX;
		Handle = scrollT.GetChild(0).GetChild(0);
	}

	private void Update()
	{

		Handle.transform.localScale = new Vector3(3.96f*(viewPortX * DivideContentX), 0.7f, 1);
		//Handle.transform.localscale = new Vector3(viewportx * dividecontentx, 1, 1);
	}
}
