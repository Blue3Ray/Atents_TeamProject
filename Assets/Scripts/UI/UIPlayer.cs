using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIPlayer : MonoBehaviour
{

    public GameObject elemanterSlot;

    ActionControl acionControl;

    private void Awake()
    {
       acionControl = new ActionControl();
    }

    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesEvent.performed += OnElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.canceled += OffElemanterMenu;
    }

    private void OnDisable()
    {
        acionControl.MouseClickMenu.MouesEvent.canceled -= OffElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.performed -= OnElemanterMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Update()
    {

    }

    private void OnElemanterMenu(InputAction.CallbackContext context)
    {
        elemanterSlot.SetActive(true);
    }

    private void OffElemanterMenu(InputAction.CallbackContext context)
    {
        elemanterSlot.SetActive(false);
    }


}
