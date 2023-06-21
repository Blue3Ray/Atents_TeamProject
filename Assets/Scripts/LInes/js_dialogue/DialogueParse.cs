using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{

    List<string> contextList;

    List<Dialogue> dialogues;

    List<OneDialogueEvent> finalDialogues;

    int saveIndex;

    int dialogueCount;


	//List<Dialogue> dialoguesList;
	//int tmpcount = 0;

	public Dialogue[] Parse (string CSV_File)
    {
        
        //�� �Լ��� ��ȯ�ϴ� �� GMDialogue�� �迭�̰�
        //���⼱ GMDialogue���� ����Ʈ �̴�!
        //List<Dialogue> dialoguesList = new List<Dialogue>();
        
        //TextAsset Ÿ������ �Է¹��� �̸��� csv ������ ã�Ƽ�
        //csvData��� ������ ����־���.
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);

        //�Ʊ� ������ ������ {}�ȿ� �� �� �������� ������
        //���ο� char�迭 �� csv���� �� �پ� �ϴ� ���´�.
        string[] stringData = csvData.text.Split(new char[] { '\n' });

		//dialogues = new List<Dialogue>();

        //������ �� �� �� �ɰ�����
        //���⼭�� �� �࿡�� �޸����� �ɰ���.
        for(int i= 1; i < stringData.Length;)
        {
            
            string[] row = stringData[i].Split(new char[] { ',' });
            contextList.Clear();
            Dialogue temp = new Dialogue();
            temp.Event = row[0];
            temp.name = row[1];

			do
            {
                contextList.Add(row[2]);
				//Debug.Log($"�ο���: {row[2]}");
				if (++i < stringData.Length)
				{
                    
                    row = stringData[i].Split(new char[] { ',' });
				}
                else
                {
                    break;
                }
			} while (row[0].ToString() == "" && row[1].ToString() == "");

            temp.contexts = contextList.ToArray();
			dialogues.Add(temp);
		}

       // dialogueCount = dialogues.Count;
		return dialogues.ToArray();
        
    }



	private void eventParse()
	{
		for (int i = 0; i < dialogues.Count; i++)
		{
			OneDialogueEvent tmpOnDialogueEvent = new OneDialogueEvent();

			if (dialogues[i].Event.ToString() != "")
			{
				tmpOnDialogueEvent.EventName = dialogues[i].Event;
				tmpOnDialogueEvent.EventDialogues = new List<Dialogue>();

				do
				{
					tmpOnDialogueEvent.EventDialogues.Add(dialogues[i]);
					i++;
					if (i >= dialogues.Count)
					{
						break;
					}

				} while (dialogues[i].Event.ToString() == "");

			}

			finalDialogues.Add(tmpOnDialogueEvent);

		}
	}

	private void Awake()
	{
		dialogues = new List<Dialogue>();
		contextList = new List<string>();
        finalDialogues = new List<OneDialogueEvent>();

	}

	private void Start()
	{
        Parse("NewDialogue");
		eventParse();
		//for(int i = 0; i < finalDialogues.Count; i++)
		//{
		//	Debug.Log($"{finalDialogues[i].EventName}");
		//	Debug.Log($"{finalDialogues[i].EventDialogues[0].contexts[0]}");

		//}


		for (int j = 0; j < dialogues.Count; j++)
		{
			Debug.Log($"{j}");

			Debug.Log($"{dialogues[j].Event}");
			Debug.Log($"{dialogues[j].name}");
			for (int i = 0; i < dialogues[j].contexts.Length; i++)
			{
				Debug.Log($"{dialogues[j].contexts[i]}");
			}
		}

	}
}
