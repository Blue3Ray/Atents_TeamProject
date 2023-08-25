using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ü�� �� ���̽� ��ũ��Ʈ�Դϴ�.
/// </summary>
public class ProjectileBase : MonoBehaviour
{
	public ElementalType elementalType;

	/// <summary>
	/// ����ü�� ���ư��� �ӵ��Դϴ�.
	/// </summary>
	public float ProjectileSpeed = 1;

	/// <summary>
	/// ����ü�� ���ư� �����Դϴ�.
	/// </summary>
	Vector3 dirProjectile = Vector3.zero;
	
	/// <summary>
	/// ����ü�� ���� ��������Ʈ �������Դϴ�.
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// ����ü�� character�� �¾��� �� ������ ��������Ʈ.
	/// </summary>
	public Action<Character, ElementalType> OnHit;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Debug.Log($"{spriteRenderer.sprite.bounds.size.x}");
	}

	private void Start()
	{
		transform.SetParent(null);					//�θ��� ������ ���� �ʱ� ����
		if (GameManager.Ins.IsRight)				//�������� ���� �ִ��� ������ ���� �ִ��� �Ǵ��� �� �׿� �ɸ´� �������� ���.
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
	/// �ִϸ��̼��� �̺�ƮŰ���� ����� �Լ��Դϴ�.
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
