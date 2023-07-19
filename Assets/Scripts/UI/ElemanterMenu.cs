using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElemanterMenu : MonoBehaviour
{
    ActionControl acionControl;

    Transform elemanterMenu;

    Element element = Element.None;

    private void Awake()
    {
        acionControl = new ActionControl();

        elemanterMenu = transform.GetChild(0);

    }
    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesEvent.performed += OnElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.canceled += OffElemanterMenu;

        acionControl.MouseClickMenu.MousePosition.performed += MousePosition;
    }



    private void OnDisable()
    {
        acionControl.MouseClickMenu.MousePosition.performed -= MousePosition;

        acionControl.MouseClickMenu.MouesEvent.canceled -= OffElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.performed -= OnElemanterMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        
    }

    private void OnElemanterMenu(InputAction.CallbackContext _)
    {
        elemanterMenu.gameObject.SetActive(true);
    }

    private void OffElemanterMenu(InputAction.CallbackContext contect)
    {
        elemanterMenu.gameObject.SetActive(false);
    }

    // ¸¶¿ì½º ÁÂÇ¥
    private void MousePosition(InputAction.CallbackContext contect)
    {
        Vector3 mousePos = contect.ReadValue<Vector2>();

        ///Vector3 target = Camera.main.ScreenToWorldPoint(worldPos);
        //worldPoint.z = 0;
        elemanterMenu.transform.position = mousePos;

        
    }




}
