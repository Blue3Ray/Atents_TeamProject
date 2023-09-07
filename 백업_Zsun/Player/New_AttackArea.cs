using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_AttackArea : MonoBehaviour
{
	public System.Action<Character> onCharacterEnter;
	public System.Action<Character> onCharacterExit;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.TryGetComponent<Character>(out Character temp))
		{
			onCharacterEnter?.Invoke(temp);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Character>(out Character temp))
		{
			onCharacterExit?.Invoke(temp);
		}
	}

}
