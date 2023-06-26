using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkCanvas : MonoBehaviour
{
	//인스펙터 창에서 이 인덱스만 고쳐주면 알아서 대화가 실행됩니다!
	public int eventIndex = 0;
	ActionControl actionControle;
	TextMeshProUGUI character;
	TextMeshProUGUI talkLine;
	int talkIndex = 0;
	int contextIndex = 0;
	string tmpText;
	string tmpTextAnim;
	public float talkAnimSpeed = 1.0f;
	bool IsTalking = false;
	private List<OneDialogueEvent> finalDialogues;



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
		
		finalDialogues = DialogueParse.Ins.finalDialogues;
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
		if(!IsTalking)
		SetIndex();
	}

	private void SetIndex()
	{
		contextIndex++;
		if (
			finalDialogues[eventIndex].EventDialogues[talkIndex].contexts.Length > contextIndex)
		{
			
		}
		else
		{
			talkIndex++;
			contextIndex = 0;
		}

		OnTalking();
	}

	private void OnTalking()
	{

		if (finalDialogues[eventIndex].EventDialogues.Count > talkIndex)
		{
			character.text = finalDialogues[eventIndex].EventDialogues[talkIndex].name;
			tmpText= finalDialogues[eventIndex].EventDialogues[talkIndex].
			contexts[contextIndex];
			StartCoroutine(talkAnimation(tmpText));
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}

	IEnumerator talkAnimation(string context)
	{
		tmpTextAnim = "";
		IsTalking = true;
		for (int i = 0; i < context.Length; i++)
		{
			tmpTextAnim += context[i];
			talkLine.text = tmpTextAnim;
			yield return new WaitForSeconds(talkAnimSpeed);
		}
		IsTalking = false;
	}
}
