using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestP : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu 객체 생성
    GameObject elemanterMenu;

    private GameObject fireSelect;
    private GameObject waterSelect;
    private GameObject windSelect;
    private GameObject thunderSelect;

    

    private void Awake()
    {
        actionControl = new ActionControl();

        // UIPlayer 자식인 elemanterMenu 
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

    private void ElemanterSelect(InputAction.CallbackContext context)
    {
        Vector3 mousepostion = Mouse.current.position.ReadValue();
        
        


    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Vector3 mousepostion = Mouse.current.position.ReadValue();
    //    if (collision.gameObject.CompareTag("RingMenu1"))
    //    {

    //    }
    //    else if(collision.gameObject.CompareTag("RingMenu2"))
    //    {

    //    }
    //    else if(collision.gameObject.CompareTag("RingMenu3"))
    //    {

    //    }
    //    else if(collision.gameObject.CompareTag("RingMenu4"))
    //    {

    //    }
    //}

}
