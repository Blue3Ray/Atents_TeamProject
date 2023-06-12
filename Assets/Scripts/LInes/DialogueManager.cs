using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

	private string[] DialogueLines;
	TextAsset dialogueText;

	private void Awake()
	{
		dialogueText = Resources.Load<TextAsset>("Dialogue");
		DialogueLines = dialogueText.text.Split('\n');
		
	}
}
