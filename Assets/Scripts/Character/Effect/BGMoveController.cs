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
		//플레이어가 이동한 만큼의 역방향 벡터
		Vector2 moveDir = currentPlayerPosition - (Vector2)player.position;

		//이동한 만큼 반대로 배경을 옮긴 다음
		Vector2 positionNow = transform.localPosition;
		positionNow += moveDir * movingRatio;
		transform.localPosition = positionNow;

		//현재 배경의 로컬 포지션을 클램프해준다.
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
