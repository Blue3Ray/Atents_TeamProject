using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

public class RingMenu : MonoBehaviour
{
    ActionControl acionControl;

    Transform elemanterMenu;

    Transform[] elemanterSlot;

    Fire fire;

    private void Awake()
    {
        acionControl = new ActionControl();

        elemanterMenu = transform.GetChild(0);

        elemanterSlot = new Transform[4];
        elemanterSlot[0] = elemanterMenu.GetChild(0).transform;
        elemanterSlot[1] = elemanterMenu.GetChild(1).transform;
        elemanterSlot[2] = elemanterMenu.GetChild(2).transform;
        elemanterSlot[3] = elemanterMenu.GetChild(3).transform;
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

    private void Start()
    {
      
    }

    private void Update()
    {
        
    }

    private void OnElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        
        elemanterMenu.position = mousePos;
        elemanterMenu.gameObject.SetActive(true);

    }

    private void OffElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 target = context.ReadValue<Vector2>();
       

        elemanterMenu.gameObject.SetActive(false);
    }

}
