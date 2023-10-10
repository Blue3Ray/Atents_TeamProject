using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BGMoveController : MonoBehaviour
{
	public float movingRatio;

	float minX;
	float maxX;
	float minY;
	float maxY;

	Sprite BackGroundSprite;

	Vector2 currentPlayerPosition;

	Transform player;

	private void Awake()
	{
		float height = 2 * Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		height *= 0.5f;
		width *= 0.5f;

		Debug.Log($"{width}, {height}");

		BackGroundSprite = GetComponent<SpriteRenderer>().sprite;
		maxX = BackGroundSprite.bounds.extents.x - width;
		minX = -maxX;
		maxY = BackGroundSprite.bounds.extents.y - height;
		minY = -maxY;
		Debug.Log($"Bounds : {BackGroundSprite.bounds} / Border : {BackGroundSprite.border}");
		Debug.Log($"{maxX}, {minX}, {maxY}, {minY}");
	}

	private void Start()
	{
		player = transform.parent;
		currentPlayerPosition = player.localPosition;
	}

	private void Update()
	{
		StartCoroutine(BackGroundMoving());
	}

	IEnumerator BackGroundMoving()
	{
		//�÷��̾ �̵��� ��ŭ�� ������ ����
		Vector2 moveDir = currentPlayerPosition - (Vector2)player.position;

		//�̵��� ��ŭ �ݴ�� ����� �ű� ����
		Vector2 positionNow = transform.localPosition;
		positionNow += moveDir * movingRatio;
		transform.localPosition = positionNow;

		//���� ����� ���� �������� Ŭ�������ش�.
		positionNow = transform.localPosition;
		positionNow.x = Mathf.Clamp(positionNow.x, minX, maxX);
		positionNow.y = Mathf.Clamp(positionNow.y, minY, maxY);
		transform.localPosition = positionNow;


		currentPlayerPosition = player.position;


		//moveDir.y = Mathf.Clamp(moveDir.y, minY, maxY);
		//moveDir.x = Mathf.Clamp(moveDir.x, minX, maxX);

		yield return null;
	}
}
