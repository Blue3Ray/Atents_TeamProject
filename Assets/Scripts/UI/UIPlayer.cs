using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu 객체 생성
    public GameObject elemanterMenu;

    //ElemanterMenu 클래스 가져오기
    ElemanterMenu elemanter;

    private void Awake()
    {
        actionControl = new ActionControl();


        // UIPlayer 자식인 elemanterMenu 
        //elemanterMenu = transform.GetChild(0).gameObject;
        
        // elemanterMenu 위치를 uiplayer 위치로 받아오기
       // elemanterMenu.transform.position = transform.position;

        elemanter = new ElemanterMenu();
    }

    private void OnEnable()
    {
        actionControl.MouseClickMenu.Enable();
        actionControl.MouseClickMenu.MouesEvent.performed += Onclick;
    }

    private void OnDisable()
    {
        actionControl.MouseClickMenu.MouesEvent.performed -= Onclick;
        actionControl.MouseClickMenu.Disable();
    }

    private void Onclick(InputAction.CallbackContext _)
    {
        elemanterMenu.SetActive(true);
        actionControl.MouseClickMenu.MouesEvent.Enable();
        actionControl.MouseClickMenu.MouesEvent.performed += Onclick;
        actionControl.MouseClickMenu.MouesEvent.canceled += ElemanterSelect;
    }

 

    

    private void ElemanterSelect(InputAction.CallbackContext _)
    {
        elemanter.ElemanterSelct();
    }

}
