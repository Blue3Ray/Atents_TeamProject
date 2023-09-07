using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Child_Trigger : MonoBehaviour
{
	public Action<Character> childTriggerOn;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character characterTarget = collision.gameObject.GetComponent<Character>();

		if (characterTarget != null && !characterTarget.CompareTag("Player"))
		{
			childTriggerOn?.Invoke(characterTarget);
		}
	}
}
