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
	/// ���̾ �Ű�� �������� �ð�
	/// </summary>
	public float OnOffSecond = 0.1f;

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
	public bool isGrounded;

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
	/// None = 0
	/// Fire = 1
	/// Water = 2
	/// Wind = 3
	/// Thunder = 4
	/// </summary>
	[Header("<<none, fire, water, wind, thunder ������ ��Ÿ�� ����>>")]
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
				anim.SetBool(Hash_Grounded, isGrounded);
			}
		}
	}

	/// <summary>
	/// mp�� ���õ� ������ ������Ƽ
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
	/// ���� �ٲ� �� �������� ��������Ʈ(���� ��, �ִ� ��)
	/// </summary>
	public Action<float, float> onMpChange;

	/// <summary>
	/// �÷��̾��� ����
	/// </summary>
	uint playerLevel;

	/// <summary>
	/// �÷��̾� ������ ������Ƽ
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
					//Debug.Log($"���� : {playerLevel}");
				}
			}
		}
	}

	/// <summary>
	/// �÷��̾��� ����ġ
	/// </summary>
	int playerEx = 0;

	/// <summary>
	/// �÷��̾� ����ġ�� ������Ƽ
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
					if (playerEx > playerExMax)     // ���� ����ġ�� �ƽ� ����ġ�� ������ ��(������ �� ��)
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
	/// �÷��̾��� ����ġ �ִ밪
	/// </summary>
	int playerExMax = 30;

	/// <summary>
	/// �÷��̾� ����ġ �ִ밪�� �б� ���� ������Ƽ
	/// </summary>
	public int ExperienceMax
	{
		get => playerExMax;
	}

	/// <summary>
	/// UI�� ���� ���� ���� ��ȭ ������Ƽ
	/// �Ķ���� (����, ����ġ, �ִ� ����ġ)
	/// �ִ� ����ġ���� ���� ������ ���� ������ ���� ����Ѵ�.
	/// </summary>
	public Action<uint, int, int> onChangeEx { get; set; }

	/// <summary>
	/// �÷��̾ �������� ���� �� ���� ������Ƽ
	/// </summary>
	public Action<uint> onLevelUP { get; set; }

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

	public Action onUsePerformed;


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

	/// <summary>
	/// ���� ���� �� �ϳ��� �ǵ���
	/// �̳��� ���� �����Ͽ����ϴ�.
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
	/// Move �׼Ǹʿ� ���ε� �� Ű���� ���Ͱ��� ����
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
	Player_AttackArea attackCollider;

	/// <summary>
	/// ���ʰ� ������ ������ �ݶ��̴��� �ް� �ִ�.
	/// [0] = ������
	/// [1] = ����
	/// </summary>
	WallSensor[] wallsensor;

	DefencSensor defencSensor;

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
	List<CharacterBase> targetChars = new();

	Material playerShader;

	protected override void OnEnable()
	{
		EnableInputAction();                                            //Ŭ���� ������ �ٸ� input System ����
		inputActions.PlayerJM.Click.performed += OnClickMouse_Left;     //Ŭ���� ���� �������༭ ondie�� Ŭ���� �������� ����
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

		//AttackCollider���� ���� ���� ����Ʈ�� �߰�
		attackCollider.onCharacterEnter += (target) =>
		{
			targetChars.Add(target);
		};

		//attackCollider���� ���� ���� ����Ʈ���� ����
		attackCollider.onCharacterExit += (target) =>
		{
			targetChars.Remove(target);
		};


		//�׾��� �� �߰��Ǵ� �����Լ��Դϴ�.
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
	/// ������ �� ���� Ÿ�� �����´�.
	/// ���� Ÿ�� �ö󰡰Ե� �����?
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
	/// ���÷��� �հ� �������⸦ ���� ���̾ �Ű�ٰ� �ǵ����ɴϴ�.
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
			randomAttackIndex = UnityEngine.Random.Range(0, 3);                 //0���� 2���� ���� ����
			anim.SetTrigger(AttackHashes[randomAttackIndex]);                   //�������� ������ ��°�� ���� �ִϸ��̼� ����
			activateAttack[(int)elemantalStatus.CurrentElemantal]?.Invoke();                                    //������ ���� ��� �Լ� (����Ǵ� �Լ��� ���Һ��� ���������̴�.)
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
	/// character���� ����Ǵ� �� ���� ���
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

	// �� �Ѿ�� ���ӸŴ������� �޾ƿ� ���� �ο� �Լ�
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
