using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJS : CharacterBase, IExperience
{
	/// <summary>
	/// 레이어를 옮겼다 가져오는 시간
	/// </summary>
	public float OnOffSecond = 0.1f;

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
	public bool isGrounded;

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
	/// None = 0
	/// Fire = 1
	/// Water = 2
	/// Wind = 3
	/// Thunder = 4
	/// </summary>
	[Header("<<none, fire, water, wind, thunder 순으로 쿨타임 적용>>")]
	public float[] coolTimes;

	[ColorUsageAttribute(true, true)]
	public Color[] SwordColors;

	float elapsedCoolTime = 0f;

	public float ElapsedCoolTime
	{
		get => elapsedCoolTime;
		set
		{
			elapsedCoolTime = value;
			if (elapsedCoolTime > coolTimes[(int)ElemantalStates.CurrentElemantal])
			{
				isOverCoolTime = true;
			}
			else
			{
				isOverCoolTime = false;
			}

		}
	}

	public bool isOverCoolTime = true;



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
				anim.SetBool(Hash_Grounded, isGrounded);
			}
		}
	}

	/// <summary>
	/// mp와 관련된 변수와 프로퍼티
	/// </summary>
	float mp;

	public float MP
	{
		get => mp;
		set
		{
			if (IsAlive)
			{
				mp = value;
				mp = Mathf.Clamp(mp, 0, maxMP);
				onMpChange?.Invoke(mp, maxMP);
			}
		}
	}

	float maxMP = 100;

	public float MaxMP
	{
		get => maxMP;
		private set
		{
			maxMP = value;
		}
	}

	/// <summary>
	/// 마나 바뀔 때 외쳐지는 델리게이트(현재 값, 최대 값)
	/// </summary>
	public Action<float, float> onMpChange;

	/// <summary>
	/// 플레이어의 레벨
	/// </summary>
	uint playerLevel;

	/// <summary>
	/// 플레이어 레벨의 프로퍼티
	/// </summary>
	public uint Level
	{
		get => playerLevel;
		set
		{
			if (IsAlive)
			{
				if (playerLevel != value  && value != 1)
				{
					playerLevel = value;
					onLevelUP?.Invoke(playerLevel);
					//Debug.Log($"레벨 : {playerLevel}");
				}
			}
		}
	}

	/// <summary>
	/// 플레이어의 경험치
	/// </summary>
	int playerEx = 0;

	/// <summary>
	/// 플레이어 경험치의 프로퍼티
	/// </summary>
	public int Experience
	{
		get => playerEx;
		set
		{
			if (IsAlive)
			{
				if (playerEx != value)
				{
					playerEx = value;
					//Debug.Log($"Exp : {playerEx}");
					if (playerEx > playerExMax)     // 현재 경험치가 맥스 경험치에 도달할 때(레벨업 할 때)
					{
						LevelUp();
						Debug.Log("levelup");
					}
					onChangeEx?.Invoke(playerLevel, playerEx, playerExMax);
				}
			}
		}
	}

	public void LevelUp()
	{
		Level++;
		playerEx = 0;
		playerExMax = playerExMax * 2;

		MaxHP = MaxHP * 1.1f;
		MaxMP = MaxMP * 1.1f;
		HP = MaxMP;
		MP = MaxMP;
	}

	/// <summary>
	/// 플레이어의 경험치 최대값
	/// </summary>
	int playerExMax = 30;

	/// <summary>
	/// 플레이어 경험치 최대값의 읽기 전용 프로퍼티
	/// </summary>
	public int ExperienceMax
	{
		get => playerExMax;
	}

	/// <summary>
	/// UI에 보낼 각종 변수 변화 프로퍼티
	/// 파라메터 (레벨, 경험치, 최대 경험치)
	/// 최대 경험치까지 같이 보내서 받은 곳에서 비율 계산한다.
	/// </summary>
	public Action<uint, int, int> onChangeEx { get; set; }

	/// <summary>
	/// 플레이어가 레벨업을 했을 때 보낼 프로퍼티
	/// </summary>
	public Action<uint> onLevelUP { get; set; }

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

	public Action onUsePerformed;


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

	/// <summary>
	/// 닿은 벽은 딱 하나만 되도록
	/// 이넘을 통해 관리하였습니다.
	/// </summary>
	public TouchedWall PlayerTouchedWall
	{
		get => playerTouchedWall;
		set
		{
			if (playerTouchedWall != value)
			{
				playerTouchedWall = value;
				//Debug.Log($"{playerTouchedWall}");
			}

		}
	}

	/// <summary>
	/// Move 액션맵에 바인딩 된 키들의 벡터값을 저장
	/// </summary>
	Vector2 dir;
	Vector2 Dir
	{
		get => dir;
		set
		{
			if (dir != value)
			{
				dir = value;

				if (dir != Vector2.zero)
				{
					knockBackDir = dir;
					if (dir.x > 0)
					{
						spriteRenderer.flipX = false;
					}
					else
					{
						spriteRenderer.flipX = true;
					}
				}


			}
		}
	}

	public Action[] activateAttack;

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
	Player_AttackArea attackCollider;

	/// <summary>
	/// 왼쪽과 오른쪽 벽센서 콜라이더를 받고 있다.
	/// [0] = 오른쪽
	/// [1] = 왼쪽
	/// </summary>
	WallSensor[] wallsensor;

	DefencSensor defencSensor;

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
	List<CharacterBase> targetChars = new();

	Material playerShader;

	protected override void OnEnable()
	{
		EnableInputAction();                                            //클릭을 제외한 다른 input System 연결
		inputActions.PlayerJM.Click.performed += OnClickMouse_Left;     //클릭은 따로 실행해줘서 ondie때 클릭은 먹히도록 설정
	}
	protected override void OnDisable()
	{
		inputActions.PlayerJM.Click.performed -= OnClickMouse_Left;
		DisableInputAction();
	}

	public void EnableInputAction()
	{
		inputActions.PlayerJM.Enable();
		inputActions.PlayerJM.Move.performed += OnMove;
		inputActions.PlayerJM.Move.canceled += OnMove;
		inputActions.PlayerJM.Jump.performed += OnJump;
		inputActions.PlayerJM.Jump.canceled += OffSpaceBar;
		inputActions.PlayerJM.Attack.performed += OnAttack;
		inputActions.PlayerJM.Down.performed += OnDown;
		inputActions.PlayerJM.Down.canceled += OnDown;
		inputActions.PlayerJM.Dash.performed += OnDash;
		inputActions.PlayerJM.Use.performed += OnUse;
	}

	public void DisableInputAction()
	{
		inputActions.PlayerJM.Use.performed -= OnUse;
		inputActions.PlayerJM.Dash.performed -= OnDash;
		inputActions.PlayerJM.Down.performed -= OnDown;
		inputActions.PlayerJM.Down.canceled -= OnDown;
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
		elemantalStatus= new ElemantalStates();
		activateAttack = new Action[5];
		activateAttack[0] += NoneAttack;
		activateAttack[1] += FireAttack;
		activateAttack[2] += WaterAttack;
		activateAttack[3] += WindAttack;
		activateAttack[4] += ThunderAttack;

		playerShader = transform.GetComponent<Renderer>().sharedMaterial;


		inputActions = new ActionControl();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		AttackHashes = new int[3];
		AttackHashes[0] = Hash_Attack1;
		AttackHashes[1] = Hash_Attack2;
		AttackHashes[2] = Hash_Attack3;
		attackAreaPivot = transform.GetChild(0);
		attackArea = attackAreaPivot.GetChild(0).gameObject;
		attackCollider = GetComponentInChildren<Player_AttackArea>();
		defencSensor = transform.GetChild(5).GetComponent<DefencSensor>();
		defencSensor.OnDefence += (demage) => Defence(demage, -Dir * 2.0f);

		//AttackCollider에서 들어온 것을 리스트에 추가
		attackCollider.onCharacterEnter += (target) =>
		{
			targetChars.Add(target);
		};

		//attackCollider에서 나간 것을 리스트에서 제거
		attackCollider.onCharacterExit += (target) =>
		{
			targetChars.Remove(target);
		};


		//죽었을 대 추가되는 람다함수입니다.
		onDie += () =>
		{
			anim.SetTrigger(Hash_Death);
			DisableInputAction();
			Dir = Vector2.zero;
		};
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		anim.SetFloat(Hash_AirSpeedY, fallSpeed);

		wallsensor = new WallSensor[2];
		wallsensor[0] = transform.GetChild(1).GetComponent<WallSensor>();
		wallsensor[1] = transform.GetChild(2).GetComponent<WallSensor>();
		wallsensor[0].OnWall += (OnOff) => SetTouchedWall_Right(OnOff);
		wallsensor[1].OnWall += (OnOff) => SetTouchedWall_Left(OnOff);

		GameManager.Ins.LoadPlayerData(this);
	}


	private void FixedUpdate()
	{
		ElapsedCoolTime += Time.deltaTime;
		transform.Translate(Time.fixedDeltaTime * moveSpeed * Dir);
	}

	private void OnClickMouse_Left(InputAction.CallbackContext obj)
	{
		Vector3 mousePosition = Input.mousePosition;

		MouseJustclick_Left?.Invoke();


		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		RaycastHit2D hit;

		if (hit = Physics2D.Raycast(ray.origin, ray.direction, 50.0f))
		{
			if (hit.transform.TryGetComponent<IClickable>(out IClickable temp))
			{
				temp.OnClicking(temp);
			}
		}
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		Vector2 result = Vector2.zero;

		if (context.canceled)
		{
			anim.SetInteger(Hash_AnimState, 0);
			//result = Vector2.zero;
		}
		else
		{

			result = context.ReadValue<Vector2>();
			if (result.x != 0)
			{
				if (result.x < -0.1f)
				{
					attackAreaPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
				}
				else if (result.x > 0.1f)
				{
					attackAreaPivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				}
				anim.SetInteger(Hash_AnimState, 1);
			}
			result.y = 0;
		}
		Dir = result;
	}


	/// <summary>
	/// 내려올 때 벽을 타고 내려온다.
	/// 벽을 타고 올라가게도 만들까?
	/// </summary>
	/// <param name="obj"></param>
	private void OnDash(InputAction.CallbackContext obj)
	{
		if (IsGrounded)
		{
			if (PlayerTouchedWall == TouchedWall.None)
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
			else
			{
				if (PlayerTouchedWall == TouchedWall.LeftWall)
				{
					rb.AddForce(new Vector3(2, 2, 0) * wallJumpForce, ForceMode2D.Impulse);
				}
				else if (PlayerTouchedWall == TouchedWall.RightWall)
				{
					rb.AddForce(new Vector3(-2, 2, 0) * wallJumpForce, ForceMode2D.Impulse);
				}
			}
		}
		else
		{
			if (PlayerTouchedWall == TouchedWall.LeftWall)
			{
				rb.AddForce(new Vector3(2, 2, 0) * wallJumpForce, ForceMode2D.Impulse);
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				rb.AddForce(new Vector3(-2, 2, 0) * wallJumpForce, ForceMode2D.Impulse);
			}
		}
	}

	private void OnDown(InputAction.CallbackContext context)
	{
		if (context.canceled)
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


	/// <summary>
	/// 반플랫폼 뚫고 내려가기를 위해 레이어를 옮겼다가 되돌려옵니다.
	/// </summary>
	/// <returns></returns>
	IEnumerator TriggerOnOff()
	{
		isTriggerSwitch = true;
		this.gameObject.layer = 8;
		yield return new WaitForSeconds(OnOffSecond);
		this.gameObject.layer = 9;
		StopAllCoroutines();
		isTriggerSwitch = false;
	}

	float attackCoolTime = 1.0f;
	public float attackCoolTimeMax = 1.0f;
	IEnumerator AttackCoolTime()
	{
		attackCoolTime = attackCoolTimeMax;
		while (attackCoolTime > 0)
		{
			attackCoolTime -= Time.deltaTime;
			yield return null;
		}
	}

	private void OnAttack(InputAction.CallbackContext context_)
	{

		if (IsAlive && isOverCoolTime)
		{
			ElapsedCoolTime = 0;
			int randomAttackIndex;
			randomAttackIndex = UnityEngine.Random.Range(0, 3);                 //0부터 2까지 난수 저장
			anim.SetTrigger(AttackHashes[randomAttackIndex]);                   //랜덤으로 정해진 번째의 공격 애니메이션 실행
			activateAttack[(int)elemantalStatus.CurrentElemantal]?.Invoke();                                    //실질적 공격 명령 함수 (연결되는 함수가 원소별로 여러가지이다.)
		}
	}

	public void ChangeActivateAttack(ElementalType elementalType)
	{
		elemantalStatus.ChangeType(elementalType);
		ElapsedCoolTime = 100;
		playerShader.SetColor("_EmissionColor", SwordColors[(int)elementalType]);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 7)
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
	public override void Attack(CharacterBase target)
	{
		target.Defence(AttackState, elemantalStatus);
	}

	public override void Attack(CharacterBase target, float knockBackPower)
	{
		base.Attack(target, knockBackPower);
	}




	private void NoneAttack()
	{
		foreach (var tmp in targetChars)
		{
			tmp.Defence(attackState, knockBackDir);
		}
	}
	private void FireAttack()
	{
		if (MP > 0)
		{
			FarAttack(PoolObjectType.Projectile_Fire);
			MP -= 5;
		}
	}
	private void WaterAttack()
	{
		if (MP > 0)
		{
			FarAttack(PoolObjectType.Projectile_Water);
			MP -= 5;
		}
	}

	private void ThunderAttack()
	{
		if (MP > 0)
		{
			FarAttack(PoolObjectType.Projectile_Thunder);
			MP -= 5;
		}
	}

	private void WindAttack()
	{
		if (MP > 0)
		{
			FarAttack(PoolObjectType.Projectile_Wind);
			MP -= 5;
		}
	}



	private void FarAttack(PoolObjectType type)
	{
		GameObject temp = Factory.Ins.GetObject(type, attackArea.transform.position, 0);
		ProjectileBase tempProjectile = temp.GetComponent<ProjectileBase>();
		tempProjectile.OnInitialize(knockBackDir);
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

	

	public override void Defence(float damage, ElemantalStates elemantal = null)
	{
		base.Defence(damage, elemantal);
		if (IsAlive) anim.SetTrigger(Hash_Hurt);
	}

	public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
	{
		base.Defence(damage, knockBackDir, elemantal);
		if (IsAlive) anim.SetTrigger(Hash_Hurt);
	}

	private void OnUse(InputAction.CallbackContext obj)
	{
		onUsePerformed?.Invoke();
	}

	// 씬 넘어갈때 게임매니져에서 받아올 스텟 부여 함수
	public void Loadtate(SaveData saveData)
	{
		if (saveData != null)
		{
			playerLevel = saveData.level;
			playerEx = saveData.exper;
			playerExMax = saveData.exper_max;
			hp = saveData.hp;
			maxHP = saveData.hp_max;
			mp = saveData.mp;
			maxMP = saveData.mp_max;

			for (int i = 0; i < elemantalStatus.elemantalevels.Length; i++)
			{
				elemantalStatus.elemantalevels[i] = saveData.elementLevel[i];
			}
		}
		else
		{
			playerLevel = 1;
			playerExMax = 30;
			playerEx = 0;
			maxHP = 100;
            HP = 100;
            maxMP = 100;
            MP = 100;
        }
	}
}
