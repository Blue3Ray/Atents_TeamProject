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

    // 속성 메뉴
    Transform elemanterMenu;

   RingSlotUI[] slotUI;

    RingMenuSlot ringslot;

    // 슬롯 확인용 프로퍼티
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
