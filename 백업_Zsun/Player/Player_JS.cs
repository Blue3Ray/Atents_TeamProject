using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_JS : Character
{
	Animator animator;
	readonly int Hash_Grounded = Animator.StringToHash("Grounded");
	readonly int Hash_Attack = Animator.StringToHash("Attack");
	ActionControl playerInputAction;
	public float moveSpeed;
	Vector2 dir;
	Collider2D GroundSensor;
	bool isGround = false;
	public float jumpForce = 20.0f;
	Rigidbody2D rg;
	SpriteRenderer spriteRenderer;
	Transform AttackPivot;
	Collider2D attackArea;
	

	public bool IsGround
	{
		get => IsGround;
		set
		{
			if(value != isGround)
			{
				isGround = value;
				//Debug.Log($"{isGround}");
				animator.SetBool(Hash_Grounded, isGround);
			}
		}
	}


	protected override void Awake()
	{
		base.Awake();
		playerInputAction = new();
		animator = GetComponent<Animator>();
		rg = transform.GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		AttackPivot = transform.GetChild(1);
		attackArea = AttackPivot.GetChild(0).GetComponent<Collider2D>();

	}
	//private void Awake()
	//{
	//	playerInputAction = new();
	//	animator = GetComponent<Animator>();
	//	rg = transform.GetComponent<Rigidbody2D>();
	//	spriteRenderer = GetComponent<SpriteRenderer>();
	//	AttackPivot = transform.GetChild(1);
	//	attackArea = AttackPivot.GetChild(0).GetComponent<Collider2D>();

	//}
	private void Start()
	{
		GroundSensor = transform.GetChild(0).GetComponent<Collider2D>();
		OffAttackArea();
	}

	private void OnEnable()
	{
		playerInputAction.PlayerJM.Enable();
		playerInputAction.PlayerJM.Attack.performed += OnAttack;
		playerInputAction.PlayerJM.Move.performed += this.OnMove;
		playerInputAction.PlayerJM.Move.canceled += this.OnMove;
		playerInputAction.PlayerJM.Jump.performed += OnJump;
		playerInputAction.PlayerJM.Down.performed += OnDown;
	}


	private void OnDisable()
	{
		playerInputAction.PlayerJM.Down.performed -= OnDown;
		playerInputAction.PlayerJM.Jump.performed -= OnJump;
		playerInputAction.PlayerJM.Move.canceled -= this.OnMove;
		playerInputAction.PlayerJM.Move.performed -= this.OnMove;
		playerInputAction.PlayerJM.Attack.performed -= OnAttack;
		playerInputAction.PlayerJM.Disable();
	}
	private void FixedUpdate()
	{
		transform.Translate(Time.deltaTime * moveSpeed * dir);
	}


	private void OnDown(InputAction.CallbackContext obj)
	{
	}

	private void OnJump(InputAction.CallbackContext obj)
	{
		rg.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
	}

	private void OnMove(InputAction.CallbackContext obj)
	{
		//Debug.Log("´­¸²");
		dir = obj.ReadValue<Vector2>();
		if(dir.x < 0)
		{
			AttackPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
			spriteRenderer.flipX = true;
		}
		else if(dir.x != 0)
		{
			AttackPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			spriteRenderer.flipX = false;
		}

	}

	private void OnAttack(InputAction.CallbackContext obj)
	{
		attackArea.enabled = true;
		animator.SetTrigger(Hash_Attack);
	}

	public void OffAttackArea()
	{
		attackArea.enabled = false;
	}

}
