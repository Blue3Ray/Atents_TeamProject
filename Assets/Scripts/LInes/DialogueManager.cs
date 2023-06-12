using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

	private string[] dialogueLines;
	TextAsset dialogueText;

	private string[] DialogueLines
	{
		get => dialogueLines;
	}

	private void Awake()
	{
		if (dialogueLines == null)
		{

			dialogueText = Resources.Load<TextAsset>("Dialogue");
			dialogueLines = dialogueText.text.Split('\n');
		}
		else
		{
			Debug.Log("�̹� dialogueLines�� �����Ǿ����ϴ�.");
		}
	}
}

