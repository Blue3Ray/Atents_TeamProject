using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스크립트 임시로 만듬, 기본적으로 점프, 이동만 구현되어있음.
public class PlayerTest : MonoBehaviour
{
    public float moveSpeed = 10;

    float currentSpeed;

    Vector2 dir;

    ActionControl ac;

    Rigidbody2D rb;

    private void Awake()
    {
        ac = new();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        dir.y = 0;
        transform.Translate(currentSpeed * Time.deltaTime * dir);
    }

    private void OnEnable()
    {
        ac.PlayerTest.Enable();
        ac.PlayerTest.Move.performed += OnMove;
        ac.PlayerTest.Move.canceled += OnMove;
        ac.PlayerTest.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        ac.PlayerTest.Jump.performed -= OnJump;
        ac.PlayerTest.Move.canceled -= OnMove;
        ac.PlayerTest.Move.performed -= OnMove;
        ac.PlayerTest.Disable();
    }
    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        rb.AddForce(transform.up * 10f, ForceMode2D.Impulse);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }


}
