using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJM_test : MonoBehaviour
{
    private ActionControl inputActions;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isAttacking;
    private bool isGrounded;

    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float attackRange = 1f;

    Vector2 dir;

    //지선 - inventory를 플레이어가 가질 수 있도록 추가
	public Inventory inven;

	private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
		inputActions = new ActionControl();
        inputActions.PlayerJM.Enable();
        inputActions.PlayerJM.Move.performed += OnMove;
        inputActions.PlayerJM.Move.canceled += OnMove;
        inputActions.PlayerJM.Jump.performed += OnJump;
        inputActions.PlayerJM.Attack.performed += ctx => Attack();

    }
    

    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {

        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Start()
    {
		inven = new Inventory(7);
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {

        isGrounded = IsGrounded();


        transform.Translate(Time.deltaTime * moveSpeed * dir);

        if (isAttacking)
        {
            AttackAction();
        }
    }

    private bool IsGrounded()
    {

        float extraHeight = 0.01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }

    private void Attack()
    {
        isAttacking = true;
    }

    private void AttackAction()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in colliders)
        {

            Debug.Log("공격 중: " + collider.gameObject.name);
        }


        isAttacking = false;
    }
}
