using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    readonly int zDistance = -10;
	PlayerJM player;

	private void Start()
	{
		player = GameManager.Ins.playerTest;
		if(player == null)
		{
			Debug.Log("플레이어를 못찾았음 (카메라)");
		}
	}

	private void Update()
	{
		if(player != null)
		{
			transform.position = new Vector3(player.transform.position.x,
				player.transform.position.y, zDistance);
		}
	}
}
