using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RingMenuUI : MonoBehaviour
{
    ActionControl acionControl;

    RingMenuSlotUI[] slot;

    CanvasGroup canvasGroup;
  

    private void Awake()
    {

        acionControl = new ActionControl();
        
        slot = new RingMenuSlotUI[5];
        
        slot[0] = transform.GetChild(1).GetComponent<RingMenuSlotUI>();
        slot[1] = transform.GetChild(2).GetComponent<RingMenuSlotUI>();
        slot[2] = transform.GetChild(3).GetComponent<RingMenuSlotUI>();
        slot[3] = transform.GetChild(4).GetComponent<RingMenuSlotUI>();

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
    ///  ������ ���콺�� RingMenu ȣ��
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext _)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();

        transform.position = mousepostion;
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    ///  ������ ���콺 ��ư�� ���� �� Slot ����
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
                
        //        Debug.Log("��");
        //        break;
        //    case ElementalType.Wind:
                
        //        Debug.Log("�ٶ�");
        //        break;
        //    case ElementalType.Water:
              
        //        Debug.Log("��");
        //        break;
        //    case ElementalType.Thunder:
             
        //        Debug.Log("����");
        //        break;
        //}
        
    }

}
