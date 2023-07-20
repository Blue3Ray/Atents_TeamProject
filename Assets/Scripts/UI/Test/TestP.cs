using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestP : MonoBehaviour
{
    ActionControl acionControl;

    // elematerMenu 객체 생성
    GameObject elemanterMenu;


    TestPlayer testPlayer;


    private void Awake()
    {
        acionControl = new ActionControl();

        // UIPlayer 자식인 elemanterMenu 
        elemanterMenu = transform.GetChild(0).gameObject;

        testPlayer = FindObjectOfType<TestPlayer>();


    }
    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesEvent.performed += OnElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.canceled += OffElemanterMenu;

        acionControl.MouseClickMenu.MousePosition.performed += MousePosition;
    }

    

    private void OnDisable()
    {
        acionControl.MouseClickMenu.MousePosition.performed -= MousePosition;

        acionControl.MouseClickMenu.MouesEvent.canceled -= OffElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.performed -= OnElemanterMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Update()
    {
        
    }

    private void OnElemanterMenu(InputAction.CallbackContext _)
    {
        elemanterMenu.SetActive(true);
       
    }

    private void OffElemanterMenu(InputAction.CallbackContext _)
    {
        elemanterMenu.SetActive(false);
    }

    // 마우스 좌표
    private void MousePosition(InputAction.CallbackContext contect)
    {
        Vector3 mousePos = contect.ReadValue<Vector2>();

        

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        //Vector3 target = Camera.main.WorldToViewportPoint(worldPoint);
        worldPoint.z= 0;
        elemanterMenu.transform.position = worldPoint;

    }



}
