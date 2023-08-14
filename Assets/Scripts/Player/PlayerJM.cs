
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
    private bool isGrounded;
    private bool ignorePlatformCollision;

    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float attackRange = 1f;

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
        if (!ignorePlatformCollision && isGrounded)
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
        isGrounded = IsGrounded();

        transform.Translate(Time.deltaTime * moveSpeed * dir);

        if (isAttacking)
        {
            AttackAction();
        }

        ignorePlatformCollision = Keyboard.current.sKey.isPressed && Keyboard.current.altKey.isPressed;
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
            Debug.Log("АјАн Сп: " + collider.gameObject.name);
        }
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignorePlatformCollision && other.CompareTag("PlatformHalf"))
        {
            Physics2D.IgnoreCollision(playerCollider, other, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!ignorePlatformCollision && other.CompareTag("PlatformHalf"))
        {
            Physics2D.IgnoreCollision(playerCollider, other, false);
        }
    }
}
