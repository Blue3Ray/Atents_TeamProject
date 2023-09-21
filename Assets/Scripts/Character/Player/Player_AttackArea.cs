using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackArea : MonoBehaviour
{
	public System.Action<CharacterBase> onCharacterEnter;
	public System.Action<CharacterBase> onCharacterExit;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.TryGetComponent<CharacterBase>(out CharacterBase temp))
		{
			onCharacterEnter?.Invoke(temp);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<CharacterBase>(out CharacterBase temp))
		{
			onCharacterExit?.Invoke(temp);
		}
	}

}
