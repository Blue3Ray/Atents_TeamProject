using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencSensor : MonoBehaviour
{
    public Action<float> OnDefence;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		OnDefence?.Invoke(10);					//�÷��̾��� ������ �÷��� ����
	}
}
    
