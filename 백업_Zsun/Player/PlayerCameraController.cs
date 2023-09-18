using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    //readonly int zDistance = -10;
	PlayerJM player;
	public float CameraSpeed = 5.0f;
	Vector3 CameraZ;
	public float MinMapX;
	public float MaxMapX;
	public float MinMapY;
	public float MaxMapY;


	float cameraX;
	float cameraY;

	private void Awake()
	{
		//높이 고정을 위해 Z값을 저장
		CameraZ = new Vector3(0, 0, -10);

		//카메라의 높이
		cameraY = Camera.main.orthographicSize;

		//카메라의 넓이
		cameraX = cameraY * Screen.width / Screen.height;
	}

	private void Start()
	{
		player = GameManager.Ins.playerTest;
	}

	private void FixedUpdate()
	{
		CameraMove();

	}
	private void CameraMove()
	{
		transform.position =
		Vector3.Lerp(transform.position, player.transform.position + CameraZ, Time.fixedDeltaTime * CameraSpeed);
		float ClampX = Mathf.Clamp(transform.position.x, MinMapX + cameraX, MaxMapX - cameraX);
		float ClampY = Mathf.Clamp(transform.position.y, MinMapY + cameraY, MaxMapY - cameraY);
		transform.position = new Vector3(ClampX, ClampY, -10);

	}
}
