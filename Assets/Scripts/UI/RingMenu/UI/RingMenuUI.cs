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
    ActionControl acionControl;

    // �Ӽ� �޴�
    Transform elemanterMenu;

   RingSlotUI[] slotUI;

    RingMenuSlot ringslot;

    // ���� Ȯ�ο� ������Ƽ
    public RingMenuSlot RingSlot => ringslot;

    public uint Index => ringslot.Index;

    private void Awake()
    {
        acionControl = new ActionControl();

       elemanterMenu = transform.GetChild(0);
        slotUI = elemanterMenu.GetComponentsInChildren<RingSlotUI>();

       
    }

    private void Start()
    {
        elemanterMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void OnSlotClick(uint index)
    {
       
    }

}
