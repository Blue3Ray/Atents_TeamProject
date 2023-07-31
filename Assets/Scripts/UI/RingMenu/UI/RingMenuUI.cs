using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class RingMenuUI : MonoBehaviour
{
    TextMeshProUGUI equippedText;


    ActionControl acionControl;

    // 속성 메뉴
    Transform elemanterMenu;

    // 속성 메뉴 안에 있는 슬롯
    Transform[] elemanterSlot;

    RingMenuSlot ringslot;

    // 슬롯 확인용 프로퍼티
    public RingMenuSlot RingSlot => ringslot;

    public uint Index => ringslot.Index;

    private void Awake()
    {
        acionControl = new ActionControl();

        // 링메뉴 자식인 속성 메뉴 호출
        elemanterMenu = transform.GetChild(0);

        // 초기값으로 속성 메뉴가 보이지 않는 상태
        elemanterMenu.gameObject.SetActive(false);
    }

    
   
    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesEvent.performed += OnElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.canceled += OffElemanterMenu;
    }



    private void OnDisable()
    {
        acionControl.MouseClickMenu.MouesEvent.canceled -= OffElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.performed -= OnElemanterMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    // 마우스 오른쪽 버튼 누를 시 마우스 포인터 위치에 속성 메뉴 활성화
    private void OnElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        elemanterMenu.position = mousePos;
        elemanterMenu.gameObject.SetActive(true);

    }

    // 속성 메뉴 활성화 된 상태에서 속성메뉴 위치에서 마우스 오른쪽 버튼 해제 시 속성 선택
    private void OffElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 target = context.ReadValue<Vector2>();
        target = mousePos;

    }

   
}
