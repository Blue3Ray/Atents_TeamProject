using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 투사체에 들어갈 베이스 스크립트입니다.
/// </summary>
public class ProjectileBase : MonoBehaviour
{
	public ElementalType elementalType;

	/// <summary>
	/// 투사체가 날아가는 속도입니다.
	/// </summary>
	public float ProjectileSpeed = 1;

	/// <summary>
	/// 투사체가 날아갈 방향입니다.
	/// </summary>
	Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// 투사체가 가진 스프라이트 렌더러입니다.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// 투사체와 character가 맞았을 때 외쳐질 델리게이트.
	/// </summary>
	public Action<Character, ElementalType> OnHit;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Debug.Log($"{spriteRenderer.sprite.bounds.size.x}");
	}

	private void Start()
	{
		transform.SetParent(null);					//부모의 영향을 받지 않기 위해
		if (GameManager.Ins.IsRight)				//오른쪽을 보고 있는지 왼쪽을 보고 있는지 판단한 후 그에 걸맞는 방향으로 쏜다.
		{
			dirProjectile = transform.right;
		}
		else
		{
			spriteRenderer.flipY = true;
			dirProjectile = -(transform.right);
		}
	}
	private void Update()
	{
		transform.Translate(dirProjectile * ProjectileSpeed);
	}

	/// <summary>
	/// 애니메이션의 이벤트키에서 실행될 함수입니다.
	/// </summary>
	public void EndAttack()
	{
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character characterTarget = collision.gameObject.GetComponent<Character>();
		if(characterTarget != null && !characterTarget.CompareTag("Player"))
		{
			InvokeOnHit(characterTarget);
		}
	}

	protected virtual void InvokeOnHit(Character targetHit)
	{
		OnHit?.Invoke(targetHit, elementalType);
	}
}
