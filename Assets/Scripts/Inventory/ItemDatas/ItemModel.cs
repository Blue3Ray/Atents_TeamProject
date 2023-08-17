using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : MonoBehaviour
{
	float timeElapsed= 0.0f;
	float y = 0.0f;
	SpriteRenderer spriteImage;

	private void Awake()
	{
		spriteImage = GetComponent<SpriteRenderer>();	
	}
	private void Update()
	{
		timeElapsed += Time.deltaTime;
		y = Mathf.Sign(timeElapsed)*0.5f+0.5f;
		Debug.Log($"{y}");
		spriteImage.color = new Color(1, 1, 1, y);
	}
}
