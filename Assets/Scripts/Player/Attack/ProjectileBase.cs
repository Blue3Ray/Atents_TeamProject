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

	protected ElemantalStatus elemantalStatus;

	/// <summary>
	/// ����ü�� ���ư��� �ӵ��Դϴ�.
	/// </summary>
	public float ProjectileSpeed = 1;

	public float ProjectileLife = 3.0f;

	/// <summary>
	/// ����ü�� ���ư� �����Դϴ�.
	/// </summary>
	protected Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// ����ü�� ���� ��������Ʈ �������Դϴ�.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// ����ü�� character�� �¾��� �� ������ ��������Ʈ.
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
			if (!GameManager.Ins.IsRight)                //�������� ���� �ִ��� ������ ���� �ִ��� �Ǵ��� �� �׿� �ɸ´� �������� ���.
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
	/// �ִϸ��̼��� �̺�ƮŰ���� ����� �Լ��Դϴ�.
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
