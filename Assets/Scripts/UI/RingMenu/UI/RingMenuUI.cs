using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Unity.VisualScripting;

public class RingMenuUI : MonoBehaviour
{
    ActionControl acionControl;

    // 속성 메뉴
    Transform elemanterMenu;

    RingMenuSlotUI menuSlotUI;



   

    private void Awake()
    {
        acionControl = new ActionControl();

       elemanterMenu = transform.GetChild(0);
       
        menuSlotUI = new RingMenuSlotUI();
        
        
       
    }
    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouseLeft.performed += ElemantalMove;
        acionControl.MouseClickMenu.MouseLeft.canceled += ElemantalMove;

        acionControl.MouseClickMenu.MouesRight.performed += OnRingMenu;
        acionControl.MouseClickMenu.MouesRight.canceled += RingSlotSelect;
    }


    private void OnDisable()
    {
        acionControl.MouseClickMenu.MouseLeft.canceled -= ElemantalMove;
        acionControl.MouseClickMenu.MouseLeft.performed -= ElemantalMove;
        acionControl.MouseClickMenu.Disable();
    }

    private void Start()
    {
        elemanterMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 속성을 RingMenu에 옮길때 사용
    /// </summary>
    /// <param name="context"></param>
    private void ElemantalMove(InputAction.CallbackContext context)
    {
        
    }

    /// <summary>
    ///  오른쪽 마우스로 RingMenu 호출
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext context)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();

        elemanterMenu.transform.position = mousepostion;
        elemanterMenu.gameObject.SetActive(true);
    }

    /// <summary>
    ///  오른쪽 마우스 버튼을 놓을 시 Slot 선택
    /// </summary>
    /// <param name="context"></param>
    private void RingSlotSelect(InputAction.CallbackContext context)
    {
        
        elemanterMenu.gameObject.SetActive(false);
    }

    private void OnSlotClick(uint index)
    {
       
    }

}
