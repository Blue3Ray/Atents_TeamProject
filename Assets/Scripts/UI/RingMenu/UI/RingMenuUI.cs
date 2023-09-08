using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RingMenuUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    ActionControl acionControl;

    RingMenuSlotUI[] slot;

    CanvasGroup canvasGroup;

    Transform test;

    private void Awake()
    {

        acionControl = new ActionControl();
        
        slot = new RingMenuSlotUI[5];
        test = transform.GetChild(0);
        slot[0] = test.GetChild(1).GetComponent<RingMenuSlotUI>();
        slot[1] = test.GetChild(2).GetComponent<RingMenuSlotUI>();
        slot[2] = test.GetChild(3).GetComponent<RingMenuSlotUI>();
        slot[3] = test.GetChild(4).GetComponent<RingMenuSlotUI>();

        slot[0].onEnter += Onclick;
        slot[1].onEnter += Onclick;
        slot[2].onEnter += Onclick;
        slot[3].onEnter += Onclick;
        //canvasGroup = GetComponent<CanvasGroup>();


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
        //canvasGroup.alpha = 0.0f;
        test.gameObject.SetActive(false);
    }


    /// <summary>
    ///  오른쪽 마우스로 RingMenu 호출
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext _)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();
        transform.position = mousepostion;
        test.gameObject.SetActive(true);
        //canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    ///  오른쪽 마우스 버튼을 놓을 시 Slot 선택
    /// </summary>
    /// <param name="context"></param>
    private void RingSlotSelect(InputAction.CallbackContext context)
    {

        test.gameObject.SetActive(false);
       // canvasGroup.alpha = 0.0f;
    }

    public ElementalType selectIndex = 0;

    public void Onclick(uint index)
    {
        selectIndex = (ElementalType)index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"업 현재 선택된 값 : {selectIndex}");
        GameManager.Ins.player.ElemantalSelect(selectIndex);
    }


}
