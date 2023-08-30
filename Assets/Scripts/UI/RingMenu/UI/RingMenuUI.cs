using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Unity.VisualScripting;

public class RingMenuUI : RingMenuSlotUI
{
    ActionControl acionControl;
    // 속성 메뉴
    Transform elemanterMenu;

    RingMenuSlotUI[] slot;

    protected override void Awake()
    {
        base.Awake();

        acionControl = new ActionControl();

        elemanterMenu = transform.GetChild(0);
        slot = new RingMenuSlotUI[5];
        slot[0] = elemanterMenu.GetChild(0).GetComponent<RingMenuSlotUI>();
        slot[1] = elemanterMenu.GetChild(1).GetComponent<RingMenuSlotUI>();
        slot[2] = elemanterMenu.GetChild(2).GetComponent<RingMenuSlotUI>();
        slot[3] = elemanterMenu.GetChild(3).GetComponent<RingMenuSlotUI>();
       
        slot[0].onClick += SlotSelect;
        slot[1].onClick += SlotSelect;
        slot[2].onClick += SlotSelect;
        slot[3].onClick += SlotSelect;
        
       
    }

    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesRight.performed += OnRingMenu;
        acionControl.MouseClickMenu.MouesRight.canceled += RingSlotSelect;
    }


    private void OnDisable()
    {
        acionControl.MouseClickMenu.MouesRight.canceled -= RingSlotSelect;
        acionControl.MouseClickMenu.MouesRight.performed -= OnRingMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Start()
    {
        elemanterMenu.gameObject.SetActive(false);
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

    public void SlotClick()
    {
        
    }
   

    public void SlotSelect(uint index)
    {
        switch (index)
        {
            case (uint)ElementalType.Fire:
          
                Debug.Log("불");
                break;
            case (uint)ElementalType.Wind:
                
                Debug.Log("바람");
                break;
            case (uint)ElementalType.Water:
              
                Debug.Log("물");
                break;
            case (uint)ElementalType.Thunder:
             
                Debug.Log("번개");
                break;

        }
    }


}
