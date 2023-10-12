using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElemantalStausWindow : MonoBehaviour
{
    ActionControl actionControl;

    /// <summary>
    /// 속성 스테이터스 창
    /// </summary>
    Transform statusWindow;

    /// <summary>
    /// 속성 슬롯
    /// </summary>
    StatusSlot[] statusSlot;

    /// <summary>
    /// 보유 스텟 구역
    /// </summary>
    Transform stetZone;

    /// <summary>
    /// 보유 스탯 텍스트 구역
    /// </summary>
    TextMeshProUGUI stetValue;

    ElemantalStates elemantalStates;

    private void Awake()
    {
        actionControl = new ActionControl();

        statusWindow = transform.GetChild(0);

        statusSlot = new StatusSlot[4];
        statusSlot[0] = statusWindow.GetChild(1).GetComponent<StatusSlot>();
        statusSlot[1] = statusWindow.GetChild(2).GetComponent<StatusSlot>();
        statusSlot[2] = statusWindow.GetChild(3).GetComponent<StatusSlot>();
        statusSlot[3] = statusWindow.GetChild(4).GetComponent<StatusSlot>();

        stetZone = statusWindow.GetChild(5);
        stetValue = stetZone.GetChild(1).GetComponent<TextMeshProUGUI>();
        
    }

    private void OnEnable()
    {
        actionControl.ElemantalStausWindow.Enable();
        actionControl.ElemantalStausWindow.StatusWindow.performed += OpenStatusWindow;
        actionControl.ElemantalStausWindow.StatusWindow.canceled += OpenStatusWindow;
    }


    private void OnDisable()
    {
        actionControl.ElemantalStausWindow.StatusWindow.canceled -= OpenStatusWindow;
        actionControl.ElemantalStausWindow.StatusWindow.performed -= OpenStatusWindow;
        actionControl.ElemantalStausWindow.Disable();
    }

    private void Start()
    {
        statusWindow.gameObject.SetActive(false);
    }

    private void OpenStatusWindow(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        

        statusWindow.gameObject.SetActive(true);
    }

}
