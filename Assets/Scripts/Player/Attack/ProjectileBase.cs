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

	protected ElemantalStatus elemantalStatus;

	/// <summary>
	/// 투사체가 날아가는 속도입니다.
	/// </summary>
	public float ProjectileSpeed = 1;

	public float ProjectileLife = 3.0f;

	/// <summary>
	/// 투사체가 날아갈 방향입니다.
	/// </summary>
	protected Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// 투사체가 가진 스프라이트 렌더러입니다.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// 투사체와 character가 맞았을 때 외쳐질 델리게이트.
	/// </summary>
	public Action<Character, ElementalType> OnHit;

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




	protected readonly int Hash_Collision = Animator.StringToHash("Collision");

	protected virtual void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponent<Animator>();
		elemantalStatus = new ElemantalStatus();
	}


	protected override void OnEnable()
	{
		base.OnEnable();
		dirProjectile = transform.right;

		if (GameManager.Ins.playerTest1 != null)
		{
			if (!GameManager.Ins.IsRight)                //오른쪽을 보고 있는지 왼쪽을 보고 있는지 판단한 후 그에 걸맞는 방향으로 쏜다.
			{
				spriteRenderer.flipY = true;
			}
		}
		StartCoroutine(DisableProjectile());
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
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
	}



	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{

		Character characterTarget = collision.gameObject.GetComponent<Character>();
		
		if (characterTarget != null && !characterTarget.CompareTag("Player"))
		{
			OnAttack(characterTarget);
		}

	}

	protected virtual void OnAttack(Character characterTarget)
	{

		
		characterTarget.Defance(status, elemantalStatus);
	}


	IEnumerator DisableProjectile()
	{
		yield return (new WaitForSeconds(ProjectileLife));
		EndAttack();
		StopAllCoroutines();
	}
}
