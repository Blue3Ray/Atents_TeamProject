using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour 
{

	PlayerJS player;

	private void Start()
	{
		player = GameManager.Ins.playerTest1;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("충돌감지");
		if (collision.gameObject.CompareTag("Ground"))
		{
			player.IsGrounded = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			player.IsGrounded = false;

		}
	}

}
