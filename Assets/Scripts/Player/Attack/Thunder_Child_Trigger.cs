using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Child_Trigger : MonoBehaviour
{
	public Action<CharacterBase> childTriggerOn;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CharacterBase characterTarget = collision.gameObject.GetComponent<CharacterBase>();

		if (characterTarget != null && !characterTarget.CompareTag("Player"))
		{
			childTriggerOn?.Invoke(characterTarget);
		}
	}
}
