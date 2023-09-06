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

    RingMenuSlotUI[] slot;

    CanvasGroup canvasGroup;

    private void Awake()
    {

        acionControl = new ActionControl();

        elemanterMenu = transform.GetChild(0);
        
        slot = new RingMenuSlotUI[5];
        
        slot[0] = elemanterMenu.GetChild(1).GetComponent<RingMenuSlotUI>();
        slot[1] = elemanterMenu.GetChild(2).GetComponent<RingMenuSlotUI>();
        slot[2] = elemanterMenu.GetChild(3).GetComponent<RingMenuSlotUI>();
        slot[3] = elemanterMenu.GetChild(4).GetComponent<RingMenuSlotUI>();

        slot[0].onUP += SlotSelect;
        slot[1].onUP += SlotSelect;
        slot[2].onUP += SlotSelect;
        slot[3].onUP += SlotSelect;

        canvasGroup = GetComponent<CanvasGroup>();
  
       
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
        canvasGroup.alpha = 0.0f;

    }


    /// <summary>
    ///  오른쪽 마우스로 RingMenu 호출
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext _)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();

        elemanterMenu.transform.position = mousepostion;
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    ///  오른쪽 마우스 버튼을 놓을 시 Slot 선택
    /// </summary>
    /// <param name="context"></param>
    private void RingSlotSelect(InputAction.CallbackContext context)
    {
        canvasGroup.alpha = 0.0f;
    }

    public void SlotSelect(ElementalType type)
    {
        Debug.Log(type);
        
        //switch (type)
        //{
        //    case ElementalType.Fire:
                
        //        Debug.Log("불");
        //        break;
        //    case ElementalType.Wind:
                
        //        Debug.Log("바람");
        //        break;
        //    case ElementalType.Water:
              
        //        Debug.Log("물");
        //        break;
        //    case ElementalType.Thunder:
             
        //        Debug.Log("번개");
        //        break;
        //}
        
    }


}
