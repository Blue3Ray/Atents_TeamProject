using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkCanvas : MonoBehaviour
{
	Action EndTalk;
	ActionControl actionControle;
	TextMeshProUGUI character;
	TextMeshProUGUI talkLine;
	int talkIndex = 0;
	int eventIndex = 0;
	int contextIndex = 0;


	private void Awake()
	{
		actionControle = new ActionControl();
		Transform tmp = transform.GetChild(0);
		Transform tmpLine = tmp.GetChild(1);
		Transform tmpCharactor = tmp.GetChild(0);
		character = tmpCharactor.GetComponent<TextMeshProUGUI>();
		talkLine = tmpLine.GetComponent<TextMeshProUGUI>();
			
	}

	private void OnEnable()
	{

		actionControle.ClickAction.Enable();
		actionControle.ClickAction.Mouse_Left.performed += OnClick;

		OnTalking();
	}

	private void OnDisable()
	{

		actionControle.ClickAction.Mouse_Left.performed -= OnClick;
		actionControle.ClickAction.Disable();
	}



	private void OnClick(InputAction.CallbackContext _)
	{
		Debug.Log("마우스 눌림");
		talkIndex++;
		OnTalking();
	}

	private void OnTalking()
	{

		if (DialogueParse.Ins.finalDialogues[eventIndex].EventDialogues.Count > talkIndex)
		{
			character.text = DialogueParse.Ins.finalDialogues[eventIndex].EventDialogues[talkIndex].name;
			talkLine.text = DialogueParse.Ins.finalDialogues[eventIndex].EventDialogues[talkIndex].contexts[0];
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}
}
