using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestP : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
    GameObject elemanterMenu;

    GameObject fireSelect;
    GameObject waterSelect;
    GameObject windSelect;
    GameObject thunderSelect;

    

    private void Awake()
    {
        actionControl = new ActionControl();

        // UIPlayer �ڽ��� elemanterMenu 
        elemanterMenu = transform.GetChild(0).gameObject;
        elemanterMenu.transform.position = transform.position;

        fireSelect = elemanterMenu.transform.GetChild(0).gameObject;
        waterSelect = elemanterMenu.transform.GetChild(1).gameObject;
        windSelect = elemanterMenu.transform.GetChild(2).gameObject;
        thunderSelect = elemanterMenu.transform.GetChild(3).gameObject;

        

    }

    private void OnEnable()
    {
        actionControl.MouseClickMenu.MouesEvent.Enable();
        actionControl.MouseClickMenu.MouesEvent.performed += Onclick;
        actionControl.MouseClickMenu.MouesEvent.canceled += ElemanterSelect;
    }



    private void OnDisable()
    {
        actionControl.MouseClickMenu.MouesEvent.canceled -= ElemanterSelect;
        actionControl.MouseClickMenu.MouesEvent.performed -= Onclick;
        actionControl.MouseClickMenu.MouesEvent.Disable();
    }

    private void Onclick(InputAction.CallbackContext _)
    {
        elemanterMenu.SetActive(true);
    }

    private void ElemanterSelect(InputAction.CallbackContext _)
    {
        if (fireSelect != null)
        {
            Debug.Log("�� ����");
        }
        else if (waterSelect != null)
        {
            Debug.Log("�� ����");
        }
        else if (windSelect != null)
        {
            Debug.Log("�ٶ� ����");
        }
        else if (thunderSelect != null)
        {
            Debug.Log("���� ����");
        }
    }

   
}
