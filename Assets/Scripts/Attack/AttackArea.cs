using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	bool IsAttackAreaValid;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("트리거 들어옴");
		//IBattle battle = collision.GetComponent<IBattle>();
		//if(battle != null)
		//{

		//}
	}
}
