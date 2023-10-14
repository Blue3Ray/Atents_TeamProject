using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElemantalStausWindow : MonoBehaviour
{
    ActionControl actionControl;

    /// <summary>
    /// �Ӽ� �������ͽ� â
    /// </summary>
    Transform statusWindow;

    /// <summary>
    /// �Ӽ� ����
    /// </summary>
    StatusSlot[] statusSlot;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    Transform stetZone;

    /// <summary>
    /// ���� ���� �ؽ�Ʈ ����
    /// </summary>
    TextMeshProUGUI stetValueText;

    ElemantalStates elemantalStates;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        actionControl = new ActionControl();

        statusWindow = transform.GetChild(0);

        statusSlot = new StatusSlot[4];
        statusSlot[0] = statusWindow.GetChild(1).GetComponent<StatusSlot>();
        statusSlot[1] = statusWindow.GetChild(2).GetComponent<StatusSlot>();
        statusSlot[2] = statusWindow.GetChild(3).GetComponent<StatusSlot>();
        statusSlot[3] = statusWindow.GetChild(4).GetComponent<StatusSlot>();

        stetZone = statusWindow.GetChild(5);
        stetValueText = stetZone.GetChild(1).GetComponent<TextMeshProUGUI>();

        canvasGroup = transform.GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;

    }

    private void OnEnable()
    {
        actionControl.ElemantalStausWindow.Enable();
        actionControl.ElemantalStausWindow.StatusWindow.performed += OnOffStatusWindow;
    }


    private void OnDisable()
    {
        actionControl.ElemantalStausWindow.StatusWindow.performed -= OnOffStatusWindow;
        actionControl.ElemantalStausWindow.Disable();
    }

    private void Start()
    {
        PlayerJS player = GameManager.Ins.Player;
        RefreshData(player.SkillStet);
        player.onChangeSkillStet += RefreshData;
    }

    public void StetCheck()
    {
        PlayerJS player = GameManager.Ins.Player;
        player.SkillStet--;
    }

    public void RefreshData(int stetvalue)
    {
        stetValueText.text = $"{stetvalue}";
    }

    private void OnOffStatusWindow(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (canvasGroup.interactable)
        {

            CloseStatusWindow();
        }
        else
        {
            OpenStatusWindow();
        }

    }

    /// <summary>
    /// �Ӽ� ���� â Open
    /// </summary>
    private void OpenStatusWindow()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        GameManager.Ins.Player.DisableInputAction();
    }
    /// <summary>
    /// �Ӽ� ���� â Close
    /// </summary>
    public void CloseStatusWindow()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        GameManager.Ins.Player.EnableInputAction();
    }

}
