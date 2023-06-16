using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickEvent : MonoBehaviour
{
    public GameObject mouseSelect; // �Ӽ� ���� �޴�

    ActionControl actionControl;

    //Transform selectPostion; // �Ӽ� ���� �޴� ��ġ

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

    // ���콺 ������ ��ư ���� �� �Ӽ� ���� �޴� �߻�
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
