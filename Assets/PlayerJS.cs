using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJS : MonoBehaviour
{
	/// <summary>
	/// inventory를 플레이어가 가질 수 있도록 추가
	/// </summary>
	public Inventory inven;

	/// <summary>
	/// Input System 받아올 변수
	/// </summary>
	private ActionControl inputActions;

	/// <summary>
	/// 리지드 바디는 rb에 저장
	/// </summary>
	private Rigidbody2D rb;

	/// <summary>
	/// 플레이어의 캡슐 콜라이더
	/// </summary>
	private Collider2D playerCollider;

	/// <summary>
	/// 좌우 반전을 위해 스프라이트 렌더러를 캐싱할 변수
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// 땅에 닿았는지 확인하는 변수
	/// </summary>
	private bool isGrounded;

	bool isTriggerSwitch = false;

	bool isSpaceBarOn = false;

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
	/// 달리는 스피드
	/// </summary>
	public float moveSpeed = 10f;

	/// <summary>
	/// 점프 높이에 관여된 힘
	/// </summary>
	public float jumpForce = 10f;

	/// <summary>
	/// 공격범위
	/// </summary>
	public float attackRange = 1f;

	/// <summary>
	/// anim의 airSpeed set을 음수로 해놓지 않으면 점프 애니메이션에서
	/// 트랜지션이 idle까지 transition이 되지를 않아서
	/// 미리 음수로 지정
	/// </summary>
	public float fallSpeed = -1.0f;

	/// <summary>
	/// player의 hp
	/// </summary>
	float hp = 0.0f;
	public float maxHp = 100;

	bool OnDownArrow = false;

	/// <summary>
	/// hp가 바뀔 때마다 hp를 invoke
	/// </summary>
	public Action<float> onHpChange;

	public Action OnBlockCommand;

	public Action MouseJustclick_Left;

	bool IsHalfPlatform = false;

	/// <summary>
	/// hp가 양수인지 확인해서 bool로 get할 수 있는 프로퍼티
	/// </summary>
	public bool IsAlive => hp > 0;

	public float HP
	{
		get => hp;
		set
		{
			if (hp != value && IsAlive)
			{
				hp = Mathf.Max(0, value);
				onHpChange?.Invoke(hp);
			}
		}
	}


	/// <summary>
	/// Move 액션맵에 바인딩 된 키들의 벡터값을 저장
	/// </summary>
	Vector2 dir;

	/// <summary>
	/// 애니메이터 컴포넌트를 받아올 변수
	/// </summary>
	Animator anim;

	Transform attackAreaPivot;
	GameObject attackArea;
	New_AttackArea attackCollider;
	WallSensor[] wallsensor;

	/// <summary>
	/// 애니메이터 파라메터 해쉬 모음집
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

	/// <summary>
	/// 액션 애니메이션 세 개 중 하나를 랜덤으로 setTrigger하기 위해
	/// 해쉬 세 개를 배열에 저장
	/// </summary>
	int[] AttackHashes;

	private void OnEnable()
	{
		//굳이 inputAction을 활성화시키지 않아도 될 것 같아서 주석 처리했습니다!
		//inputActions.Enable();
		inputActions.PlayerJM.Enable();
		inputActions.PlayerJM.Move.performed += OnMove;
		inputActions.PlayerJM.Move.canceled += OnMove;
		inputActions.PlayerJM.Jump.performed += OnJump;
		inputActions.PlayerJM.Jump.canceled += OffSpaceBar;
		inputActions.PlayerJM.Attack.performed += Attack;
		inputActions.PlayerJM.Click.performed += OnClickMouse_Left;
		inputActions.PlayerJM.Down.performed += OnDown;
		inputActions.PlayerJM.Down.canceled += OnDown;
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

	private void OnDisable()
	{
		inputActions.PlayerJM.Jump.canceled -= OffSpaceBar;
		inputActions.PlayerJM.Down.performed -= OnDown;
		inputActions.PlayerJM.Down.canceled -= OnDown;
		inputActions.PlayerJM.Click.performed -= OnClickMouse_Left;
		inputActions.PlayerJM.Jump.performed -= OnJump;
		inputActions.PlayerJM.Move.performed -= OnMove;
		inputActions.PlayerJM.Move.canceled -= OnMove;
		inputActions.PlayerJM.Enable();
		//inputActions.Disable();
	}

	private void Awake()
	{
		inputActions = new ActionControl();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		AttackHashes = new int[3];
		AttackHashes[0] = Hash_Attack1;
		AttackHashes[1] = Hash_Attack2;
		AttackHashes[2] = Hash_Attack3;
		HP = maxHp;
		attackAreaPivot = transform.GetChild(0);
		attackArea = attackAreaPivot.GetChild(0).gameObject;
		attackCollider = GetComponentInChildren<New_AttackArea>();
		wallsensor = GetComponentsInChildren<WallSensor>();
		//Debug.Log($"{wallsensor.Length}");

		attackCollider.onCharacterEnter += (target) => {
			targetChars.Add(target);
			Debug.Log("사정거리 안에 들어옴");
		};

		attackCollider.onCharacterExit += (target) => {
			Debug.Log("사정거리 에서 나감");
			targetChars.Remove(target);
		};

	}

	List<Character> targetChars = new();

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		anim.SetFloat(Hash_AirSpeedY, fallSpeed);
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

	private void OnJump(InputAction.CallbackContext obj)
	{
		isSpaceBarOn = true;
		if (IsGrounded && !OnDownArrow &&!isTriggerSwitch)
		{
			rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			anim.SetTrigger(Hash_Jump);
		}
		else if(OnDownArrow && IsHalfPlatform)
		{
			if (!isTriggerSwitch)
			{
				StartCoroutine(TriggerOnOff());
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

	private void Attack(InputAction.CallbackContext context_)
	{
		//OffAttackARea();
		int randomAttackIndex;
		randomAttackIndex = (int)UnityEngine.Random.Range(0, 3);
		anim.SetTrigger(AttackHashes[randomAttackIndex]);
		OnAttackARea();
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

	public void OffAttackARea()
	{

		//attackCollider.enabled = false;
	}
	public void OnAttackARea()
	{
		foreach(var item in targetChars)
		{
			Debug.Log("공격함");
		}
		//attackCollider.enabled = true;
	}
}
