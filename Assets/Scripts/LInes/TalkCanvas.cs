using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TalkCanvas : MonoBehaviour, IPointerClickHandler
{
	/// <summary>
	/// �̺�Ʈ �ε����� ���� ��ȭ�� ����
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
	CanvasGroup canvasGroup;
	public Action EndTalk;

	private void Awake()
	{
		canvasGroup = transform.GetComponent<CanvasGroup>();
		Transform talker = transform.GetChild(0);
		Transform talking = transform.GetChild(1);
		character = talker.GetComponent<TextMeshProUGUI>();
		talkLine = talking.GetComponent<TextMeshProUGUI>();
		CanvasLineOff();
	}
	private void Start()
	{
		finalDialogues = DialogueParse.Ins.finalDialogues;
		
	}

	public void CanvasLineOn()
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
		//SetIndex();
		OnTalking();
		IsTalkStart = true;
	}
	
	public void CanvasLineOff()
	{
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
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
		//������ ���̺��� context Index�� ���� ��
		if (finalDialogues[eventIndex].EventDialogues[talkIndex].contexts.Length > contextIndex)
		{
			
		}
		//���� ������ ��
		else
		{
			talkIndex++;
			contextIndex = 0;
		}

		OnTalking();
	}

	private void OnTalking()
	{
		//�̺�Ʈ�� ��ȭ ���ں��� talkIndex�� ���ڰ� ���� ��
		if (finalDialogues[eventIndex].EventDialogues.Count > talkIndex)
		{
			character.text = finalDialogues[eventIndex].EventDialogues[talkIndex].name;
			tmpText= finalDialogues[eventIndex].EventDialogues[talkIndex].
			contexts[contextIndex];
			StartCoroutine(talkAnimation(tmpText));
		}
		else
		{
			CanvasLineOff();
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

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick();
	}
}
