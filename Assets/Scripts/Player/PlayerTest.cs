using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 스크립트 임시로 만듬, 기본적으로 점프, 이동만 구현되어있음.
public class PlayerTest : MonoBehaviour
{
    public Action<Vector3> ClickPosition;

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
        ac.ClickAction.Enable();
        ac.PlayerTest.Move.performed += OnMove;
        ac.PlayerTest.Move.canceled += OnMove;
        ac.PlayerTest.Jump.performed += OnJump;
        ac.ClickAction.Mouse_Left.performed += OnClickMouse_Left;
    }

	private void OnClickMouse_Left(UnityEngine.InputSystem.InputAction.CallbackContext _)
	{
        //Vector3 mousePosition = new(Mouse.current.position.x.value, Mouse.current.position.y.value, 0);
        Vector3 mousePosition = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		Debug.Log(ray.origin);

		RaycastHit2D hit;

		if (hit = Physics2D.Raycast(ray.origin, ray.direction, 50.0f))
        {
            Debug.Log($"{hit.transform.name} 부딪힘");
        }

		ClickPosition?.Invoke(mousePosition);

        //Ray ray = Camera.main.ScreenPointToRay(_);
	}

	private void OnDisable()
    {
        ac.PlayerTest.Jump.performed -= OnJump;
        ac.PlayerTest.Move.canceled -= OnMove;
        ac.PlayerTest.Move.performed -= OnMove;
        ac.ClickAction.Disable();
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
