using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJM : MonoBehaviour
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
    /// 공격중인지 확인할 변수
    /// </summary>
    private bool isAttacking;

    /// <summary>
    /// 땅에 닿았는지 확인하는 변수
    /// </summary>
    private bool isGrounded;

    bool IsGrounded
    {
        get => isGrounded;
        set
        {
            if(isGrounded != value)
            {
                isGrounded = value;
                anim.SetBool(Hash_Grounded, isGrounded);
                Debug.Log($"애니메이터의 Grounded가 {isGrounded}로 설정되었습니다");
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

    /// <summary>
    /// hp가 바뀔 때마다 hp를 invoke
    /// </summary>
    public Action<float> onHpChange;

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

    int[] AttackHashes;

    private void OnEnable()
    {
        //굳이 inputAction을 활성화시키지 않아도 될 것 같아서 주석 처리했습니다!
        //inputActions.Enable();
        inputActions.PlayerJM.Enable();
        inputActions.PlayerJM.Move.performed += OnMove;
        inputActions.PlayerJM.Move.canceled += OnMove;
        inputActions.PlayerJM.Jump.performed += OnJump;
        
        //참조도 없고 정의도 여기가 끝인데 ctx가 뭔지 여쭤보기
        inputActions.PlayerJM.Attack.performed += Attack;
    }

    private void OnDisable()
    {
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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim.SetFloat(Hash_AirSpeedY, fallSpeed);
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);

        if (isAttacking)
        {
            AttackAction();
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            anim.SetInteger(Hash_AnimState, 0);
        }
        else
        {
            anim.SetInteger(Hash_AnimState, 1);
        }
        dir = context.ReadValue<Vector2>();
        if(dir.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if(dir.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        


    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger(Hash_Jump);
        }
    }



	////1. transform position이 아니라 bounds의 center를 써야하는 건가요?
	////2. extraHeight의 역할은?
	////3. 콜라이더의 중심으로부터 콜라이더의 세로길이의 반에 해당하는 레이에 0.01을 더한 후 GroundLayer안에 있는지
	////확인하는 걸로 이해를 했는데
	////콜라이더를 사용하는 게 좋을 것 같다고 생각합니다!
	////4. 무엇보다 이 함수에서 체크는 했는데 막상 isGround는 바꿔주지 않고 있는 것 같습니다.
	//private bool IsGrounded()
 //   {
 //       float extraHeight = 0.01f;
 //       RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, 
 //           Vector2.down, playerCollider.bounds.extents.y + extraHeight, LayerMask.GetMask("Ground"));
 //       return raycastHit.collider != null;
 //   }

    private void Attack(InputAction.CallbackContext _)
    {
        isAttacking = true;
    }

    private void AttackAction()
    {
        int randomAttackIndex;
        randomAttackIndex = (int)UnityEngine.Random.Range(0, 3);
        Debug.Log($"{randomAttackIndex}");
		anim.SetTrigger(AttackHashes[randomAttackIndex]);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("공격 중: " + collider.gameObject.name);
        }


        isAttacking = false;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
	}

	private void OnCollisionExit2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Ground"))
		{
            IsGrounded = false;
		}
	}
}
