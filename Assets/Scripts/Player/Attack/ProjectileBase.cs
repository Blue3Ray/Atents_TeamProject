using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 투사체에 들어갈 베이스 스크립트입니다.
/// </summary>
public class ProjectileBase : PooledObject
{
	public ElementalType elementalType;

	protected ElemantalStates elemantalStatus;

	/// <summary>
	/// 투사체가 날아가는 속도입니다.
	/// </summary>
	public float ProjectileSpeed = 1;

	public float ProjectileLife = 3.0f;

	/// <summary>
	/// 투사체가 날아갈 방향입니다.
	/// </summary>
	public Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// 투사체가 가진 스프라이트 렌더러입니다.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// 투사체와 character가 맞았을 때 외쳐질 델리게이트.
	/// </summary>
	public Action<CharacterBase, ElementalType> OnHit;

	public bool MoveOrStop = true;

	float status = 1f;

	public float PlayerStatus 
	{
		get => status; 
		set
		{ 
			status = value; 
		} 
	}
	
	protected Animator anim;


	protected float knockBackPower = 1;

	protected readonly int Hash_Collision = Animator.StringToHash("Collision");

	protected virtual void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();
		elemantalStatus = new ElemantalStates();
	}


	protected override void OnEnable()
	{
		base.OnEnable();
		MoveOrStop = true;
		StartCoroutine(DisableProjectile());
	}

	/// <summary>
	/// 투사체가 생성될 때 설정하는 함수(부모에서는 방향을 지정해 준다)
	/// </summary>
	/// <param name="dir">날라갈 방향</param>
	public virtual void OnInitialize(Vector2 dir)
	{
		dirProjectile = dir;
		transform.localScale *= new Vector2(dir.x, 1);
	}

	private void Update()
	{
		if (MoveOrStop)
		{
			transform.Translate(dirProjectile * ProjectileSpeed);
		}
	}

	/// <summary>
	/// 애니메이션의 이벤트키에서 실행될 함수입니다.
	/// </summary>
	public void EndAttack()
	{
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
	}



	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{

		CharacterBase characterTarget = collision.gameObject.GetComponent<CharacterBase>();
		
		if (characterTarget != null && !characterTarget.CompareTag("Player"))
		{
			OnAttack(characterTarget);
		}

	}

	protected virtual void OnAttack(CharacterBase characterTarget)
	{
		// characterTarget.knockBackDir = dirProjectile;
		characterTarget.Defence(status, dirProjectile * knockBackPower,elemantalStatus);
	}


	IEnumerator DisableProjectile()
	{
		yield return (new WaitForSeconds(ProjectileLife));
		EndAttack();
		StopAllCoroutines();
	}
}
