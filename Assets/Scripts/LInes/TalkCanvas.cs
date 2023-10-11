using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TalkCanvas : MonoBehaviour,IPointerClickHandler
{
	/// <summary>
	/// �̺�Ʈ �ε����� ���� �̺�Ʈ �� ��ȭ ����ü�� �����´�.
	/// </summary>
	public int eventIndex = 0;

	/// <summary>
	/// �� �̺�Ʈ���� ���ϴ� ����� ��ü�� ������ �߰��� ����
	/// </summary>
	int talkIndex = 0;

	/// <summary>
	/// �� ���� ���� ������ ���� �� �� ������ ǥ��
	/// </summary>
	int contextIndex = 0;

	string tmpText;
	string tmpTextAnim;
	public float talkAnimSpeed = 1.0f;
	bool IsTalking = false;
	bool IsTalkStart = false;
	public Action EndTalk;

	private List<OneDialogueEvent> finalDialogues;
	TextMeshProUGUI character;
	TextMeshProUGUI talkLine;
	CanvasGroup canvasGroup;

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
		//�� ĳ���Ͱ� �� ���� ������
		if (finalDialogues[eventIndex].EventDialogues[talkIndex].contexts.Length > contextIndex)
		{
			contextIndex++;

		}
		//�� ĳ���Ͱ� �� ���� �������� ���� ĳ���Ͱ� ���ϱ�
		else
		{
			talkIndex++;
			contextIndex = 0;
		}

		if (talkIndex < finalDialogues[eventIndex].EventDialogues.Count)
		{
			if (finalDialogues[eventIndex].EventDialogues[talkIndex].contexts.Length <= contextIndex)
			{
				SetIndex();
				return;
			}
		}
		else
		{
			CanvasLineOff();
		}
		OnTalking();
	}

	private void OnTalking()
	{
		//�̺�Ʈ�� ��ȭ�� ������ �ʾ��� ��
		if (finalDialogues[eventIndex].EventDialogues.Count > talkIndex)
		{
			character.text = finalDialogues[eventIndex].EventDialogues[talkIndex].name;
			tmpText = finalDialogues[eventIndex].EventDialogues[talkIndex].contexts[contextIndex];
			StartCoroutine(talkAnimation(tmpText));
		}
		//�̺�Ʈ�� ��ȭ�� ������ ��
		else
		{
			CanvasLineOff();
			GameManager.Ins.Player.EnableInputAction();
		}
		Debug.Log($"contextIndex: {contextIndex},talkIndex: {talkIndex}");
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

