using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu 객체 생성
   public  GameObject elemanterSlot;

  

    private void Awake()
    {
        actionControl = new ActionControl();

        

        // UIPlayer 자식인 elemanterMenu 
        
        
        // elemanterMenu 위치를 uiplayer 위치로 받아오기
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
        
       Vector2 mousepostion = Mouse.current.position.ReadValue();
        
        Debug.Log($"{mousepostion}");
    }

}
