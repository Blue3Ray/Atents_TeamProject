using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class RingMenuUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    ActionControl acionControl;

    RingMenuSlotUI[] slot;

    CanvasGroup canvasGroup;

    Transform frame;

    public GameObject ringeffect;

    PlayerJS player;

    public System.Action<uint> onUP;

    private void Awake()
    {

        acionControl = new ActionControl();
        
        slot = new RingMenuSlotUI[5];
        frame = transform.GetChild(0);
        slot[0] = frame.GetChild(1).GetComponent<RingMenuSlotUI>();
        slot[1] = frame.GetChild(2).GetComponent<RingMenuSlotUI>();
        slot[2] = frame.GetChild(3).GetComponent<RingMenuSlotUI>();
        slot[3] = frame.GetChild(4).GetComponent<RingMenuSlotUI>();
        slot[4] = frame.GetChild(0).GetComponent<RingMenuSlotUI>();

        slot[0].onEnter += Onclick;
        slot[1].onEnter += Onclick;
        slot[2].onEnter += Onclick;
        slot[3].onEnter += Onclick;
        slot[4].onEnter += Onclick;
    }

    void Start()
    {
        player = GameManager.Ins.Player;
        frame.gameObject.SetActive(false);
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


    /// <summary>
    ///  오른쪽 마우스로 RingMenu 호출
    /// </summary>
    /// <param name="context"></param>
    private void OnRingMenu(InputAction.CallbackContext _)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();
        transform.position = mousepostion;
        frame.gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            canvasGroup = slot[i].GetComponentInChildren<CanvasGroup>();
            canvasGroup.alpha = 0.0f;
        }

    }

    /// <summary>
    ///  오른쪽 마우스 버튼을 놓을 시 Slot 선택
    /// </summary>
    /// <param name="context"></param>
    private void RingSlotSelect(InputAction.CallbackContext context)
    {

        frame.gameObject.SetActive(false);

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
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"업 현재 선택된 값 : {selectIndex}");
            player.ChangeActivateAttack(selectIndex);
        }
      
    }


}
