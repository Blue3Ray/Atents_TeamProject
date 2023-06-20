using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickEvent : MonoBehaviour
{
    public GameObject mouseSelect; // 속성 선택 메뉴

    ActionControl actionControl;

    //Transform selectPostion; // 속성 선택 메뉴 위치

    private void Awake()
    {
       actionControl = new ActionControl();
    }

    private void OnEnable()
    {
        actionControl.MouseEvent.Enable();
        actionControl.MouseEvent.MouseClick.performed += OnMouseEventMenu;
        actionControl.MouseEvent.MouseClick.canceled += ESelect;

    }

    private void OnDisable()
    {
        actionControl.MouseEvent.MouseClick.canceled -= ESelect;
        actionControl.MouseEvent.MouseClick.performed -= OnMouseEventMenu;
        actionControl.MouseEvent.Disable();
    }

    // 마우스 오른쪽 버튼 선택 시 속성 선택 메뉴 발생
    private void OnMouseEventMenu(InputAction.CallbackContext _)
    {
        GameObject skillSelect = GameObject.Instantiate(mouseSelect);

        Transform child = transform.GetChild(0);

        skillSelect.transform.position = child.position;
        skillSelect.transform.rotation = child.rotation;
    }

    private void ESelect(InputAction.CallbackContext con_text)
    {
        
    }
}
