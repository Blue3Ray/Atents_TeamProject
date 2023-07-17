using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    ActionControl actionControl;

    private void Awake()
    {
        actionControl = new ActionControl();
    }


    //private void OnEnable()
    //{
    //    actionControl.MouseClickMenu.Enable();
    //    actionControl.MouseClickMenu.MouesEvent.canceled += ElementerSelect;
    //}



    //private void OnDisable()
    //{
    //    actionControl.MouseClickMenu.MouesEvent.canceled -= ElementerSelect;
    //    actionControl.MouseClickMenu.Disable();
    //}


    public void OnClickBuuton()
    {
        //Input.GetMouseButton(1);
        Debug.Log("번개 생성");


     }

        //private void ElementerSelect(UnityEngine.InputSystem.InputAction.CallbackContext context)
        //{
        //    OnClickBuuton();
        //}

    }
