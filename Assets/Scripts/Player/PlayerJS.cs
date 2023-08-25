using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJS : Character
{

	public GameObject fireBall;
	public GameObject waterBall;
	public GameObject thunderBall;
	public GameObject windBall;

	/// <summary>
	/// inventory�� �÷��̾ ���� �� �ֵ��� �߰�
	/// </summary>
	public Inventory inven;

	/// <summary>
	/// Input System �޾ƿ� ����
	/// </summary>
	private ActionControl inputActions;

	/// <summary>
	/// ������ �ٵ�� rb�� ����
	/// </summary>
	private Rigidbody2D rb;

	/// <summary>
	/// �÷��̾��� ĸ�� �ݶ��̴�
	/// </summary>
	private Collider2D playerCollider;

	/// <summary>
	/// �¿� ������ ���� ��������Ʈ �������� ĳ���� ����
	/// </summary>
	public SpriteRenderer spriteRenderer;

	/// <summary>
	/// ���� ��Ҵ��� Ȯ���ϴ� ����
	/// </summary>
	private bool isGrounded;

	/// <summary>
	/// ������ٵ� ���ٰ� Ű�� �̳��� ����������
	/// </summary>
	bool isTriggerSwitch = false;

	/// <summary>
	/// �Ʒ�Ű�� ������ ������ �ϵ�, ������ ������ �Ʒ�Ű�� ������
	/// �� �� �Ǿ�� �ϴϱ� �����̽��� ���ȴ����� Ȯ���ؾ� �Ѵ�.
	/// </summary>
	bool isSpaceBarOn = false;

	/// <summary>
	/// Ground�� ��Ҵ��� ������ ����
	/// �׿� �ɸ´� �ִϸ��̼��� �����ϴ� ������Ƽ
	/// </summary>
	public bool IsGrounded
	{
		get => isGrounded;
		set
		{
			if (isGrounded != value)
			{
				isGrounded = value;
				//Debug.Log($"isGround = {isGrounded}");
				anim.SetBool(Hash_Grounded, isGrounded);
			}
		}
	}

	/// <summary>
	/// �޸��� ���ǵ�
	/// </summary>
	public float moveSpeed = 10f;

	/// <summary>
	/// ���� ���̿� ������ ��
	/// </summary>
	public float jumpForce = 10f;

	/// <summary>
	/// ���ݹ���
	/// </summary>
	public float attackRange = 1f;

	/// <summary>
	/// anim�� airSpeed set�� ������ �س��� ������ ���� �ִϸ��̼ǿ���
	/// Ʈ�������� idle���� transition�� ������ �ʾƼ�
	/// �̸� ������ ����
	/// </summary>
	public float fallSpeed = -1.0f;

	/// <summary>
	/// wŰ�� �����ִ����� Ȯ���Ѵ�.
	/// </summary>
	bool OnDownArrow = false;

	/// <summary>
	/// ���� ����ƮŰ�� ������ �� � ũ���
	/// addforce������ �ν����Ϳ��� �޴� ����
	/// </summary>
	public float dashForce = 2.0f;

	/// <summary>
	/// ������ Ŭ���Ǿ������� �޴´�.
	/// ���� ĳ���� ���� �̾߱�� Ŭ���� �Է� �޾ƾ� �ϴµ�
	/// �׶� player�� ��ǲ�׼��� Ȯ���ϰ� �ִ�.
	/// </summary>
	public Action MouseJustclick_Left;

	/// <summary>
	/// ���÷����� ��Ҵ��� �ȴ�Ҵ���
	/// </summary>
	bool IsHalfPlatform = false;

	/// <summary>
	/// �� ��������Ʈ�� Elemental�� �Ӽ��� �´� �����Լ��� ����ȴ�.
	/// player������ ��ɸ� ���� �Ű�
	/// �������� character�� attack�� ����Ѵ�.
	/// </summary>
	public Action ActiveElementalAttack;


	/// <summary>
	/// ĳ���� ��ũ��Ʈ�� �ִ� elemantalStatus�� ������ �� �ִ�
	/// public ������Ƽ�μ�
	/// elemantalStatus�� set�� ������ ���� Ÿ���� ì�ܼ� ����Ǵ� �Լ��� �ٲ۴�.
	/// </summary>
	public ElemantalStatus PlayerElementalStatus
	{
		get => elemantalStatus;
		set
		{
			switch (value.Elemantal)
			{

				case ElementalType.Fire:
					ActiveElementalAttack = FireAttack;
					break;

				case ElementalType.Thunder:
					ActiveElementalAttack = ThunderAttack;
					break;

				case ElementalType.Water:
					ActiveElementalAttack = WaterAttack;
					break;

				case ElementalType.Wind:
					ActiveElementalAttack = WindAttack;
					break;

				default:
					ActiveElementalAttack = NoneAttack;
					break;
			}
		}
	}

	/// <summary>
	/// ������Ƽ�� public�� �� ������ private�̾ ������
	/// Ÿ���� public�̾�� �ϱ� ������ public���� ��.
	/// </summary>
	public enum TouchedWall
	{
		None = 0,
		RightWall,
		LeftWall
	}

	TouchedWall playerTouchedWall = TouchedWall.None;

	public TouchedWall PlayerTouchedWall
	{
		get => playerTouchedWall;
		set
		{
			playerTouchedWall = value;
			if(value == TouchedWall.RightWall)
			{
				spriteRenderer.flipX = false;
			}
			else if(value == TouchedWall.LeftWall)
			{
				spriteRenderer.flipX = true;
			}
		}
	}

	Transform pivotTransform;

	/// <summary>
	/// Move �׼Ǹʿ� ���ε� �� Ű���� ���Ͱ��� ����
	/// </summary>
	Vector2 dir;

	/// <summary>
	/// �ִϸ����� ������Ʈ�� �޾ƿ� ����
	/// </summary>
	Animator anim;

	/// <summary>
	/// attack�� �ݶ��̴��� �θ� transform
	/// </summary>
	Transform attackAreaPivot;

	/// <summary>
	/// attackarea �ݶ��̴�
	/// </summary>
	GameObject attackArea;

	/// <summary>
	/// ���� ��Ŀ����� ������ ������� ����
	/// ���ο� attackCollider
	/// ���� ���� attakArea�� ������ ����
	/// </summary>
	New_AttackArea attackCollider;

	/// <summary>
	/// ���ʰ� ������ ������ �ݶ��̴��� �ް� �ִ�.
	/// [0] = ������
	/// [1] = ����
	/// </summary>
	WallSensor[] wallsensor;

	/// <summary>
	/// �ִϸ����� �Ķ���� �ؽ� ������
	/// </summary>
	readonly int Hash_Grounded = Animator.StringToHash("Grounded");
	readonly int Hash_AnimState = Animator.StringToHash("AnimState");
	readonly int Hash_IsLeft = Animator.StringToHash("IsLeft");
	readonly int Hash_Jump = Animator.StringToHash("Jump");
	readonly int Hash_AirSpeedY = Animator.StringToHash("AirSpeedY");
	readonly int Hash_Attack1 = Animator.StringToHash("Attack1");
	readonly int Hash_Attack2 = Animator.StringToHash("Attack2");
	readonly int Hash_Attack3 = Animator.StringToHash("Attack3");
	readonly int Hash_WallSlide = Animator.StringToHash("WallSlide");
	readonly int Hash_Roll = Animator.StringToHash("Roll");
	readonly int Hash_Hurt = Animator.StringToHash("Hurt");
	readonly int Hash_Death = Animator.StringToHash("Death");

	/// <summary>
	/// �׼� �ִϸ��̼� �� �� �� �ϳ��� �������� setTrigger�ϱ� ����
	/// �ؽ� �� ���� �迭�� ����
	/// </summary>
	int[] AttackHashes;

	public float wallJumpForce = 10.0f;

	/// <summary>
	/// attack area �ȿ� ���� character ����Ʈ
	/// </summary>
	List<Character> targetChars = new();

	/// <summary>
	/// ������ �밢�� ����
	/// </summary>
	Vector3 Cross;

	/// <summary>
	/// 
	/// </summary>
	Vector3 LeftCross;

	private void OnEnable()
	{
		//���� inputAction�� Ȱ��ȭ��Ű�� �ʾƵ� �� �� ���Ƽ� �ּ� ó���߽��ϴ�!
		//inputActions.Enable();
		inputActions.PlayerJM.Enable();
		inputActions.PlayerJM.Move.performed += OnMove;
		inputActions.PlayerJM.Move.canceled += OnMove;
		inputActions.PlayerJM.Jump.performed += OnJump;
		inputActions.PlayerJM.Jump.canceled += OffSpaceBar;
		inputActions.PlayerJM.Attack.performed += OnAttack;
		inputActions.PlayerJM.Click.performed += OnClickMouse_Left;
		inputActions.PlayerJM.Down.performed += OnDown;
		inputActions.PlayerJM.Down.canceled += OnDown;
		inputActions.PlayerJM.Dash.performed += OnDash;
	}
	private void OnDisable()
	{
		inputActions.PlayerJM.Dash.performed -= OnDash;
		inputActions.PlayerJM.Down.performed -= OnDown;
		inputActions.PlayerJM.Down.canceled -= OnDown;
		inputActions.PlayerJM.Click.performed -= OnClickMouse_Left;
		inputActions.PlayerJM.Attack.performed -= OnAttack;
		inputActions.PlayerJM.Jump.canceled -= OffSpaceBar;
		inputActions.PlayerJM.Jump.performed -= OnJump;
		inputActions.PlayerJM.Move.performed -= OnMove;
		inputActions.PlayerJM.Move.canceled -= OnMove;
		inputActions.PlayerJM.Disable();
	}

	protected override void Awake()
	{
		base.Awake();
		inputActions = new ActionControl();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		AttackHashes = new int[3];
		AttackHashes[0] = Hash_Attack1;
		AttackHashes[1] = Hash_Attack2;
		AttackHashes[2] = Hash_Attack3;
		attackAreaPivot = transform.GetChild(0);
		attackArea = attackAreaPivot.GetChild(0).gameObject;
		attackCollider = GetComponentInChildren<New_AttackArea>();
		
		//AttackCollider���� ���� ���� ����Ʈ�� �߰�
		attackCollider.onCharacterEnter += (target) =>
		{
			targetChars.Add(target);
			Debug.Log("�����Ÿ� �ȿ� ����");
		};
		//attackCollider���� ���� ���� ����Ʈ���� ����

		attackCollider.onCharacterExit += (target) =>
		{
			Debug.Log("�����Ÿ� ���� ����");
			targetChars.Remove(target);
		};

		PlayerElementalStatusChange(ElementalType.None);
		pivotTransform = transform.GetChild(5);

		onDie += () => { anim.SetTrigger(Hash_Death); };
	}

	private void Start()
	{
		Cross = transform.up*2 + transform.right;
		LeftCross = transform.up * 2 + -(transform.right);
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		anim.SetFloat(Hash_AirSpeedY, fallSpeed);


		wallsensor = new WallSensor[2];
		wallsensor[0] = transform.GetChild(1).GetComponent<WallSensor>();
		wallsensor[1] = transform.GetChild(2).GetComponent<WallSensor>();
		wallsensor[0].OnWall += (OnOff) => SetTouchedWall_Right(OnOff);
		wallsensor[1].OnWall += (OnOff) => SetTouchedWall_Left(OnOff);

	}
	private void Update()
	{
		transform.Translate(Time.deltaTime * moveSpeed * dir);
	}

	private void OnClickMouse_Left(InputAction.CallbackContext obj)
	{
		Vector3 mousePosition = Input.mousePosition;

		MouseJustclick_Left?.Invoke();


		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		RaycastHit2D hit;


		if (hit = Physics2D.Raycast(ray.origin, ray.direction, 50.0f))
		{
			//Debug.Log($"{hit.transform.name}");
			if (hit.transform.TryGetComponent<IClickable>(out IClickable temp))
			{
				temp.OnClicking(temp);
			}
		}
	}

	private void OnMove(InputAction.CallbackContext context)
	{

		if (context.canceled)
		{
			anim.SetInteger(Hash_AnimState, 0);
			dir = Vector2.zero;
		}
		else
		{
			dir = context.ReadValue<Vector2>();
			if (dir.x != 0)
			{
				if (dir.x < -0.1f)
				{
					attackAreaPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
					spriteRenderer.flipX = true;
				}
				else if (dir.x > 0.1f)
				{
					attackAreaPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
					spriteRenderer.flipX = false;
				}
				anim.SetInteger(Hash_AnimState, 1);
			}
			dir.y = 0;
		}
	}


	private void OnDash(InputAction.CallbackContext obj)
	{
		if (IsGrounded)
		{
			if(PlayerTouchedWall == TouchedWall.None)
			{
				if (spriteRenderer.flipX == false)
				{
					rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
				}
				else
				{
					rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
				}
				anim.SetTrigger(Hash_Roll);
			}
			else if (PlayerTouchedWall == TouchedWall.LeftWall)
			{
				rb.AddForce(Cross * wallJumpForce, ForceMode2D.Impulse);
				//Debug.Log("���� ������ ����");
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				//Debug.Log("������ ������ ����");
				rb.AddForce(LeftCross * wallJumpForce, ForceMode2D.Impulse);
			}

		}
		else
		{
			if (PlayerTouchedWall == TouchedWall.LeftWall)
			{
				rb.AddForce(Cross * wallJumpForce, ForceMode2D.Impulse);
				//Debug.Log("���� ������ ����");
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				//Debug.Log("������ ������ ����");
				rb.AddForce(LeftCross * wallJumpForce, ForceMode2D.Impulse);
			}
		}
	}

	private void OnDown(InputAction.CallbackContext context)
	{

		if(context.canceled)
		{
			OnDownArrow = false;
		}
		else
		{
			if (isSpaceBarOn && !isTriggerSwitch)
			{
				StartCoroutine(TriggerOnOff());	
			}
			OnDownArrow = true;
		}
	}

	private void OnJump(InputAction.CallbackContext obj)
	{
		isSpaceBarOn = true;
		if (isGrounded)
		{

			if (!OnDownArrow && !isTriggerSwitch)
			{
				rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
				anim.SetTrigger(Hash_Jump);
			}
			else if (OnDownArrow && IsHalfPlatform)
			{
				if (!isTriggerSwitch)
				{
					StartCoroutine(TriggerOnOff());
				}
			}
		}
		
	}

	private void OffSpaceBar(InputAction.CallbackContext obj)
	{
		isSpaceBarOn = false;
	}

	IEnumerator TriggerOnOff()
	{
		isTriggerSwitch = true;
		//playerCollider.isTrigger = true;
		this.gameObject.layer = 8;
		yield return new WaitForSeconds(0.5f);
		//playerCollider.isTrigger = false;
		this.gameObject.layer = 9;
		StopAllCoroutines();
		isTriggerSwitch = false;
	}

	private void OnAttack(InputAction.CallbackContext context_)
	{
		//OffAttackARea();
		int randomAttackIndex;
		randomAttackIndex = (int)UnityEngine.Random.Range(0, 3);
		anim.SetTrigger(AttackHashes[randomAttackIndex]);
		ActiveElementalAttack?.Invoke();
		//foreach(var item in targetChars)
		//{
		//}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 7)
		{
			IsHalfPlatform = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 7)
		{
			IsHalfPlatform = false;
		}
	}

	/// <summary>
	/// character���� ����Ǵ� �� ���� ���
	/// </summary>
	/// <param name="target"></param>
	public override void Attack(Character target)
	{
		target.Defance(AttackState, elemantalStatus);
	}

	private void NoneAttack()
	{
		foreach(var target in targetChars)
		{
			Attack(target);
		}
	}

	private void WindAttack()
	{
		FarAttack(windBall);
	}

	private void WaterAttack()
	{
		FarAttack(waterBall);
	}

	private void ThunderAttack()
	{
		FarAttack(thunderBall);
	}

	private void FireAttack()
	{
		FarAttack(fireBall);
	}

	private void FarAttack(GameObject projectilePrefab)
	{

		if(spriteRenderer.flipX == false)
		{
			
			ProjectileBase projectile =
			Instantiate(projectilePrefab, attackArea.transform.position, Quaternion.identity).GetComponentInChildren<ProjectileBase>();
			projectile.OnHit += (target, elemental) => Attack(target);

		}
		else
		{
			
			ProjectileBase projectile =
			Instantiate(projectilePrefab, attackArea.transform.position, Quaternion.Euler(new Vector3(0, 0, 180))).GetComponentInChildren<ProjectileBase>();
			
			projectile.OnHit += (target, elemental) => Attack(target);

		}
	}

	private void SetTouchedWall_Right(bool IsOn)
	{
		if (IsOn)
		{
			PlayerTouchedWall = TouchedWall.RightWall;
		}
		else
		{
			PlayerTouchedWall = TouchedWall.None;
		}
	}
	private void SetTouchedWall_Left(bool IsOn)
	{
		if (IsOn)
		{
			PlayerTouchedWall = TouchedWall.LeftWall;
		}
		else
		{
			PlayerTouchedWall = TouchedWall.None;
		}
	}

	/// <summary>
	/// character ��ũ��Ʈ�� class�� change �Լ��� �����ϴ� �Լ���
	/// player���� �ִ� ������Ƽ�� �����ϱ� ���ؼ� �������.
	/// �� ������Ƽ������ �Լ��� ���� �˸°� �������ش�.
	/// </summary>
	public void PlayerElementalStatusChange(ElementalType elementalType)
	{
		elemantalStatus.ChangeType(elementalType);
		PlayerElementalStatus = elemantalStatus;
	}

	public override void Defance(float damage, ElemantalStatus elemantal = null)
	{
		anim.SetTrigger(Hash_Hurt);
		base.Defance(damage, elemantal);
	}
}
