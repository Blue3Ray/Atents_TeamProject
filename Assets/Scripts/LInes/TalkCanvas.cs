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
	TextMeshProUGUI character;
	TextMeshProUGUI talkLine;
	int talkIndex = 0;
	int contextIndex = 0;
	string tmpText;
	string tmpTextAnim;
	public float talkAnimSpeed = 1.0f;
	bool IsTalking = false;
	private List<OneDialogueEvent> finalDialogues;
	bool IsTalkStart = false;

	public Action EndTalk;

	private void Awake()
	{
		Transform talker = transform.GetChild(0);
		Transform talking = transform.GetChild(1);
		character = talker.GetComponent<TextMeshProUGUI>();
		talkLine = talking.GetComponent<TextMeshProUGUI>();
		PlayerTest.Ins.MouseJustclick_Left += OnClick;
		finalDialogues = DialogueParse.Ins.finalDialogues;
	}



	private void OnEnable()
	{
		IsTalkStart = true;
		OnTalking();
	}

	private void OnDisable()
	{
		IsTalkStart = false;
	}



	private void OnClick()
	{
		if (IsTalkStart)
		{
			
			if (!IsTalking)
			{
				SetIndex();
			}

		}
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
			EndTalk?.Invoke();
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
