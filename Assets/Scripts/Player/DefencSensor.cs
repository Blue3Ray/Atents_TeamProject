using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencSensor : MonoBehaviour
{
    public Action<float> OnDefence;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CharacterBase characterBase = collision.gameObject.GetComponent<CharacterBase>();
		if(characterBase != null)
		{
			OnDefence?.Invoke(10);					//플레이어의 데미지 플롯을 보냄
		}
	}
}
    
