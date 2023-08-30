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
    // �Ӽ� �޴�
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
    ///  ������ ���콺�� RingMenu ȣ��
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext context)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();

        elemanterMenu.transform.position = mousepostion;
        elemanterMenu.gameObject.SetActive(true);
    }

    /// <summary>
    ///  ������ ���콺 ��ư�� ���� �� Slot ����
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
          
                Debug.Log("��");
                break;
            case (uint)ElementalType.Wind:
                
                Debug.Log("�ٶ�");
                break;
            case (uint)ElementalType.Water:
              
                Debug.Log("��");
                break;
            case (uint)ElementalType.Thunder:
             
                Debug.Log("����");
                break;

        }
    }


}
