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
	public SpriteRenderer spriteRenderer;

	/// <summary>
	/// 땅에 닿았는지 확인하는 변수
	/// </summary>
	private bool isGrounded;

	/// <summary>
	/// 리지드바디를 껐다가 키는 이넘이 진행중인지
	/// </summary>
	bool isTriggerSwitch = false;

	/// <summary>
	/// 아래키를 누르고 점프를 하든, 점프를 누르고 아래키를 누르든
	/// 둘 다 되어야 하니까 스페이스가 눌렸는지도 확인해야 한다.
	/// </summary>
	bool isSpaceBarOn = false;

	/// <summary>
	/// Ground에 닿았는지 유무에 따라
	/// 그에 걸맞는 애니메이션을 설정하는 프로퍼티
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
	/// w키가 눌려있는지를 확인한다.
	/// </summary>
	bool OnDownArrow = false;

	/// <summary>
	/// 왼쪽 시프트키를 눌렀을 때 어떤 크기로
	/// addforce할지를 인스펙터에서 받는 변수
	/// </summary>
	public float dashForce = 2.0f;

	/// <summary>
	/// 왼쪽이 클릭되었는지를 받는다.
	/// 현재 캐릭터 간의 이야기는 클릭을 입력 받아야 하는데
	/// 그때 player의 인풋액션을 확인하고 있다.
	/// </summary>
	public Action MouseJustclick_Left;

	/// <summary>
	/// 반플랫폼에 닿았는지 안닿았는지
	/// </summary>
	bool IsHalfPlatform = false;

	/// <summary>
	/// 이 델리게이트에 Elemental의 속성에 맞는 공격함수가 연결된다.
	/// player고유한 기능만 넣을 거고
	/// 나머지는 character의 attack을 사용한다.
	/// </summary>
	public Action ActiveElementalAttack;


	/// <summary>
	/// 캐릭터 스크립트에 있는 elemantalStatus에 접근할 수 있는
	/// public 프로퍼티로서
	/// elemantalStatus를 set할 때마다 원소 타입을 챙겨서 연결되는 함수를 바꾼다.
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
	/// 프로퍼티가 public일 때 변수는 private이어도 되지만
	/// 타입은 public이어야 하기 때문에 public으로 함.
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
	/// Move 액션맵에 바인딩 된 키들의 벡터값을 저장
	/// </summary>
	Vector2 dir;

	/// <summary>
	/// 애니메이터 컴포넌트를 받아올 변수
	/// </summary>
	Animator anim;

	/// <summary>
	/// attack의 콜라이더의 부모 transform
	/// </summary>
	Transform attackAreaPivot;

	/// <summary>
	/// attackarea 콜라이더
	/// </summary>
	GameObject attackArea;

	/// <summary>
	/// 기존 방식에서의 오류를 잡기위해 만든
	/// 새로운 attackCollider
	/// 추후 위에 attakArea를 삭제할 예정
	/// </summary>
	New_AttackArea attackCollider;

	/// <summary>
	/// 왼쪽과 오른쪽 벽센서 콜라이더를 받고 있다.
	/// [0] = 오른쪽
	/// [1] = 왼쪽
	/// </summary>
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
	readonly int Hash_Roll = Animator.StringToHash("Roll");
	readonly int Hash_Hurt = Animator.StringToHash("Hurt");
	readonly int Hash_Death = Animator.StringToHash("Death");

	/// <summary>
	/// 액션 애니메이션 세 개 중 하나를 랜덤으로 setTrigger하기 위해
	/// 해쉬 세 개를 배열에 저장
	/// </summary>
	int[] AttackHashes;

	public float wallJumpForce = 10.0f;

	/// <summary>
	/// attack area 안에 들어온 character 리스트
	/// </summary>
	List<Character> targetChars = new();

	/// <summary>
	/// 오른쪽 대각선 방향
	/// </summary>
	Vector3 Cross;

	/// <summary>
	/// 
	/// </summary>
	Vector3 LeftCross;

	private void OnEnable()
	{
		//굳이 inputAction을 활성화시키지 않아도 될 것 같아서 주석 처리했습니다!
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
		
		//AttackCollider에서 들어온 것을 리스트에 추가
		attackCollider.onCharacterEnter += (target) =>
		{
			targetChars.Add(target);
			Debug.Log("사정거리 안에 들어옴");
		};
		//attackCollider에서 나간 것을 리스트에서 제거

		attackCollider.onCharacterExit += (target) =>
		{
			Debug.Log("사정거리 에서 나감");
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
				//Debug.Log("왼쪽 벽에서 점프");
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				//Debug.Log("오른쪽 벽에서 점프");
				rb.AddForce(LeftCross * wallJumpForce, ForceMode2D.Impulse);
			}

		}
		else
		{
			if (PlayerTouchedWall == TouchedWall.LeftWall)
			{
				rb.AddForce(Cross * wallJumpForce, ForceMode2D.Impulse);
				//Debug.Log("왼쪽 벽에서 점프");
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				//Debug.Log("오른쪽 벽에서 점프");
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
	/// character에서 실행되는 찐 공격 기능
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
	/// character 스크립트의 class의 change 함수를 실행하는 함수로
	/// player에만 있는 프로퍼티에 접근하기 위해서 만들었다.
	/// 그 프로퍼티에서는 함수를 각각 알맞게 연결해준다.
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
