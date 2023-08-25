using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour 
{

	PlayerJS player;

	private void Start()
	{
		// player = GameManager.Ins.playerTest1;
		player = transform.parent.GetComponent<PlayerJS>();
	}

	GameObject currentGround;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("충돌감지");
		if (collision.gameObject.CompareTag("Ground"))
		{
			currentGround = collision.gameObject;
			player.IsGrounded = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject == currentGround)
		{
			player.IsGrounded = false;

		}
	}

}
