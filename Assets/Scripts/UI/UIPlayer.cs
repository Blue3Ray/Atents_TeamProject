using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
   public  GameObject elemanterSlot;

    //ElemanterMenu Ŭ���� ��������
    ElemanterMenu elemanter;

    private void Awake()
    {
        actionControl = new ActionControl();

        elemanterSlot = new GameObject();

        // UIPlayer �ڽ��� elemanterMenu 
        elemanterSlot = transform.GetChild(0).gameObject;
        
        // elemanterMenu ��ġ�� uiplayer ��ġ�� �޾ƿ���
         //elemanterSlot.transform.position = transform.position;

        elemanter = new ElemanterMenu();
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
        elemanterSlot.SetActive(true);
    }

    private void ElemanterSelect(InputAction.CallbackContext _)
    {
        elemanter.ElemanterSelct();
    }

}
