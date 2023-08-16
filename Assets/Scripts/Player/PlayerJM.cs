
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJM : MonoBehaviour
{
    public Inventory inven;
    private ActionControl inputActions;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isAttacking;
    private bool isGrounded;

    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float attackRange = 1f;
    public float maxJumpSpeed = 10f; 

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
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        isGrounded = IsGrounded();

        if (isAttacking)
        {
            AttackAction();
        }

        if (!isGrounded && rb.velocity.y > maxJumpSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
        }

        transform.Translate(Time.deltaTime * moveSpeed * dir);
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
       
    }
}
