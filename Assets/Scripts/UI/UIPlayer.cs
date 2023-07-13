using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
   public  GameObject elemanterSlot;

    ElemanterMenu elemanterMenu;


    private void Awake()
    {
        actionControl = new ActionControl();

        elemanterMenu = new ElemanterMenu();    

        // UIPlayer �ڽ��� elemanterMenu 
        
        
        // elemanterMenu ��ġ�� uiplayer ��ġ�� �޾ƿ���
         //elemanterSlot.transform.position = transform.position;

        
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

    private void ElemanterSelect(InputAction.CallbackContext context)
    {
        elemanterMenu.ElemanterSelct();
       
    }

}
