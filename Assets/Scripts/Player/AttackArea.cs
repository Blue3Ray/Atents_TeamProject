using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		IBattle battle = collision.GetComponent<IBattle>();
		if(battle != null)
		{

			Debug.Log("트리거 들어옴");
		}
	}
}
