using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
	/// mp�� ���õ� ������ ������Ƽ
	/// </summary>
	float mp;

	int maxMP = 100;

	public float MP
	{
		get => mp;
		set
		{
			if (IsAlive)
			{
				mp = value;
				mp = Mathf.Clamp(MP, 0, maxMP);
				onMpchange?.Invoke(MP / maxMP);
				//Debug.Log($"���� : {MP}");
			}
		}
	}

	/// <summary>
	/// ���� �ٲ� �� �������� ��������Ʈ
	/// </summary>
	public Action<float> onMpchange;

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
				if (playerLevel != value)
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
	int playerEx = 0 ;
	
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
					if (playerEx > playerExMax)
					{
						Level++;
						playerEx = 0;
						
					}
				}
			}
		} 
	}

	/// <summary>
	/// �÷��̾��� ����ġ �ִ밪
	/// </summary>
	int playerExMax = 100;

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
    public Action<uint, int, int> onChangeEx {get; set; }
    
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

	
	/// <summary>
	/// ĳ���� ��ũ��Ʈ�� �ִ� elemantalStatus�� ������ �� �ִ� ������Ƽ�ν�
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

	/// <summary>
	/// ���� ���� �� �ϳ��� �ǵ���
	/// �̳��� ���� �����Ͽ����ϴ�.
	/// </summary>
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

	/// <summary>
	/// 
	/// </summary>
	Vector3 LeftCross;

	protected override void OnEnable()
	{
		EnableInputAction();											//Ŭ���� ������ �ٸ� input System ����
		inputActions.PlayerJM.Click.performed += OnClickMouse_Left;		//Ŭ���� ���� �������༭ ondie�� Ŭ���� �������� ����
	}
    protected override void OnDisable()
	{
		inputActions.PlayerJM.Click.performed -= OnClickMouse_Left;
		DisableInputAction();
	}

	void EnableInputAction()
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
	}

	void DisableInputAction()
	{
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
		MP = maxMP;
		HP = maxHP;
		Level = 1;
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
		attackCollider = GetComponentInChildren<Player_AttackArea>();
		defencSensor = transform.GetChild(5).GetComponent<DefencSensor>();
		defencSensor.OnDefence += (demage) => Defence(demage);
		
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

		PlayerElementalStatusChange(ElementalType.None);

		//�׾��� �� �߰��Ǵ� �����Լ��Դϴ�.
		onDie += () => 
		{ 
			anim.SetTrigger(Hash_Death);
			DisableInputAction();
			dir = Vector2.zero;
		};
	}

	private void Start()
	{
		
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
	private void FixedUpdate()
	{
		transform.Translate(Time.fixedDeltaTime * moveSpeed * dir);
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
			
			

		}
		else
		{
			if (PlayerTouchedWall == TouchedWall.LeftWall)
			{
				rb.AddForce(transform.right * wallJumpForce, ForceMode2D.Impulse);
			}
			else if (PlayerTouchedWall == TouchedWall.RightWall)
			{
				rb.AddForce(-transform.right * wallJumpForce, ForceMode2D.Impulse);
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

	private void OnAttack(InputAction.CallbackContext context_)
	{
		if(MP> 0 && IsAlive)
		{
			int randomAttackIndex;
			randomAttackIndex = (int)UnityEngine.Random.Range(0, 3);			//0���� 2���� ���� ����
			anim.SetTrigger(AttackHashes[randomAttackIndex]);					//�������� ������ ��°�� ���� �ִϸ��̼� ����
			ActiveElementalAttack?.Invoke();                                    //������ ���� ��� �Լ� (����Ǵ� �Լ��� ���Һ��� ���������̴�.)
			MP -= 5;
		}
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
		foreach(var tmp in targetChars)
		{
			tmp.Defence(attackState);
		}
	}
	private void FireAttack()
	{
		FarAttack(PoolObjectType.Projectile_Fire);
	}
	private void WaterAttack()
	{

		FarAttack(PoolObjectType.Projectile_Water);
	}

	private void ThunderAttack()
	{
		FarAttack(PoolObjectType.Projectile_Thunder);
	}

	private void WindAttack()
	{
		FarAttack(PoolObjectType.Projectile_Wind);
	}



	private void FarAttack(PoolObjectType type)
	{
		if (spriteRenderer.flipX == false)
		{
			Factory.Ins.GetObject(type, attackArea.transform.position, 0);
		}
		else
		{
			Factory.Ins.GetObject(type, attackArea.transform.position, 180);
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
	/// character ��ũ��Ʈ�� ���� ememetalstatus class�� change �Լ��� �����ϴ� �Լ���.
	/// player�� ���� ����Ÿ�� ������ �� Ŭ������ change�� �����Ű�� �۾��� �Ѵ�.
	/// </summary>
	public void PlayerElementalStatusChange(ElementalType elementalType)
	{
		elemantalStatus.ChangeType(elementalType);
		PlayerElementalStatus = elemantalStatus;
	}

	public override void Defence(float damage, ElemantalStatus elemantal = null)
	{
		anim.SetTrigger(Hash_Hurt);
		base.Defence(damage, elemantal);
	}
}
