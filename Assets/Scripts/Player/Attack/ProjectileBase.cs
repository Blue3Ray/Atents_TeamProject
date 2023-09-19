using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ü�� �� ���̽� ��ũ��Ʈ�Դϴ�.
/// </summary>
public class ProjectileBase : PooledObject
{
	public ElementalType elementalType;

	protected ElemantalStates elemantalStatus;

	/// <summary>
	/// ����ü�� ���ư��� �ӵ��Դϴ�.
	/// </summary>
	public float ProjectileSpeed = 1;

	public float ProjectileLife = 3.0f;

	/// <summary>
	/// ����ü�� ���ư� �����Դϴ�.
	/// </summary>
	public Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// ����ü�� ���� ��������Ʈ �������Դϴ�.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// ����ü�� character�� �¾��� �� ������ ��������Ʈ.
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
	/// ����ü�� ������ �� �����ϴ� �Լ�(�θ𿡼��� ������ ������ �ش�)
	/// </summary>
	/// <param name="dir">���� ����</param>
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
	/// �ִϸ��̼��� �̺�ƮŰ���� ����� �Լ��Դϴ�.
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
