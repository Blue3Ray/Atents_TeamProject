using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
	bool IsAttackAreaValid;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("TriggerIn");
	}

	//private void OnTriggerStay2D(Collider2D collision)
	//{
	//	Debug.Log("TriggerIn");
	//}

	//private void OnTriggerExit2D(Collider2D collision)
	//{
	//	Debug.Log("TriggerOut");
	//}
}
