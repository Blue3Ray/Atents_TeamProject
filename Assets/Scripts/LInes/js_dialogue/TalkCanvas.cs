using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkCanvas : MonoBehaviour
{
	/// <summary>
	/// 이벤트 인덱스에 따라 대화가 진행
	/// </summary>
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
	NPCbase npcBase;



	private void Awake()
	{
		GameObject gameObjecttmp = GameObject.Find("Rian");
		npcBase = gameObjecttmp.GetComponent<NPCbase>();
		actionControle = new ActionControl();
		Transform talker = transform.GetChild(0);
		Transform talking = transform.GetChild(1);
		character = talker.GetComponent<TextMeshProUGUI>();
		talkLine = talking.GetComponent<TextMeshProUGUI>();
		
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
