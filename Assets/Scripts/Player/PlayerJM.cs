using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJM : MonoBehaviour
{
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
    /// ���������� Ȯ���� ����
    /// </summary>
    private bool isAttacking;

    /// <summary>
    /// ���� ��Ҵ��� Ȯ���ϴ� ����
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
                Debug.Log($"�ִϸ������� Grounded�� {isGrounded}�� �����Ǿ����ϴ�");
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
    /// Move �׼Ǹʿ� ���ε� �� Ű���� ���Ͱ��� ����
    /// </summary>
    Vector2 dir;

    /// <summary>
    /// �ִϸ����� ������Ʈ�� �޾ƿ� ����
    /// </summary>
    Animator anim;

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

    private void OnEnable()
    {
        //���� inputAction�� Ȱ��ȭ��Ű�� �ʾƵ� �� �� ���Ƽ� �ּ� ó���߽��ϴ�!
        //inputActions.Enable();
        inputActions.PlayerJM.Enable();
        inputActions.PlayerJM.Move.performed += OnMove;
        inputActions.PlayerJM.Move.canceled += OnMove;
        inputActions.PlayerJM.Jump.performed += OnJump;
        
        //������ ���� ���ǵ� ���Ⱑ ���ε� ctx�� ���� ���庸��
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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim.SetFloat(Hash_AirSpeedY, fallSpeed);
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

        //���߿� �ִϸ��̼� �̷����� �� �� �ʿ��մϴ�.
        //�ٸ� ���� �ƹ�Ÿ ������ �Ǿ� �־ �̷����� �ȵǹǷ�
        //�޸ӳ��̵� �ƹ�Ÿ ������ ���� �غ����� �� �����ϴ�!
        anim.SetBool(Hash_IsLeft, dir.x < 0);
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger(Hash_Jump);
        }
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);

        if (isAttacking)
        {
            AttackAction();
        }
    }



	////1. transform position�� �ƴ϶� bounds�� center�� ����ϴ� �ǰ���?
	////2. extraHeight�� ������?
	////3. �ݶ��̴��� �߽����κ��� �ݶ��̴��� ���α����� �ݿ� �ش��ϴ� ���̿� 0.01�� ���� �� GroundLayer�ȿ� �ִ���
	////Ȯ���ϴ� �ɷ� ���ظ� �ߴµ�
	////�ݶ��̴��� ����ϴ� �� ���� �� ���ٰ� �����մϴ�!
	////4. �������� �� �Լ����� üũ�� �ߴµ� ���� isGround�� �ٲ����� �ʰ� �ִ� �� �����ϴ�.
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
        anim.SetTrigger(Hash_Attack1);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("���� ��: " + collider.gameObject.name);
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
