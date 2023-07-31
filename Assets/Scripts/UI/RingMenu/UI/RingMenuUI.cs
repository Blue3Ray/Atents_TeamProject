using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingMenuUI : MonoBehaviour
{
    ActionControl acionControl;

    // 속성 메뉴
    Transform elemanterMenu;

    // 속성 메뉴 안에 있는 슬롯
    Transform[] elemanterSlot;

    //속성 클래스
    Fire fire;
    Water water;
    Wind wind;
    Thunder thunder;

    TestPlayer testPlayer;

    private void Awake()
    {
        acionControl = new ActionControl();

        // 링메뉴 자식인 속성 메뉴 호출
        elemanterMenu = transform.GetChild(0);

        // 속성 메뉴 자식인 각 속성 슬롯 호출 
        elemanterSlot = new Transform[4];
        elemanterSlot[0] = elemanterMenu.GetChild(0).transform;
        elemanterSlot[1] = elemanterMenu.GetChild(1).transform;
        elemanterSlot[2] = elemanterMenu.GetChild(2).transform;
        elemanterSlot[3] = elemanterMenu.GetChild(3).transform;

        // 초기값으로 속성 메뉴가 보이지 않는 상태
        elemanterMenu.gameObject.SetActive(false);

        // 각 속성 클래스 호출
        fire = new Fire();
        water = new Water();
        wind = new Wind();
        thunder = new Thunder();

        testPlayer = FindObjectOfType<TestPlayer>();
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

    // 마우스 오른쪽 버튼 누를 시 마우스 포인터 위치에 속성 메뉴 활성화
    private void OnElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        elemanterMenu.position = mousePos;
        elemanterMenu.gameObject.SetActive(true);

    }

    // 속성 메뉴 활성화 된 상태에서 속성메뉴 위치에서 마우스 오른쪽 버튼 해제 시 속성 선택
    private void OffElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 target = context.ReadValue<Vector2>();
        target = mousePos;

        if (target.y > elemanterMenu.position.y)
        {
            fire.OnClickBuuton();
            testPlayer.element = Element.Fire;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.x > elemanterMenu.position.x)
        {
            water.OnClickBuuton();
            testPlayer.element = Element.Water;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.y < elemanterMenu.position.y)
        {
            wind.OnClickBuuton();
            testPlayer.element = Element.Wind;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.x < elemanterMenu.position.x)
        {
            thunder.OnClickBuuton();
            testPlayer.element = Element.Thunder;
            elemanterMenu.gameObject.SetActive(false);
        }
        else
        {
            testPlayer.element = Element.None;
            elemanterMenu.gameObject.SetActive(false);
        }


    }
}
