using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestP : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu 按眉 积己
    GameObject elemanterMenu;

    GameObject fireSelect;
    GameObject waterSelect;
    GameObject windSelect;
    GameObject thunderSelect;

    

    private void Awake()
    {
        actionControl = new ActionControl();

        // UIPlayer 磊侥牢 elemanterMenu 
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
            Debug.Log("阂 积己");
        }
        else if (waterSelect != null)
        {
            Debug.Log("拱 积己");
        }
        else if (windSelect != null)
        {
            Debug.Log("官恩 积己");
        }
        else if (thunderSelect != null)
        {
            Debug.Log("锅俺 积己");
        }
    }

   
}
