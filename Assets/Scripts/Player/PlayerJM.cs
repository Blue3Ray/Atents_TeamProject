using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJM : MonoBehaviour
{
    private ActionControl inputActions;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isAttacking;
    private bool isGrounded; // 플레이어가 땅에 있는지를 추적하는 새로운 변수

    public float moveSpeed = 10f;
    public float jumpForce = 1000f;

    Vector2 dir;

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
        // 점프를 수행하기 전에 플레이어가 땅에 있는지 확인합니다.
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // 땅 체크를 수행합니다.
        isGrounded = IsGrounded();

        // 이동 처리를 수행합니다.
        transform.Translate(Time.deltaTime * moveSpeed * dir);

        if (isAttacking)
        {
            Debug.Log("공격 중...");
        }
    }

    private bool IsGrounded()
    {
        // 플레이어가 땅에 닿아 있는지 체크합니다.
        float extraHeight = 0.01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }

    private void Attack()
    {
        isAttacking = true;
    }
}
 