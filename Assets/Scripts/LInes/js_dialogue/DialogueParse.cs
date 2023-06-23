using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{

    List<string> contextList;

    List<Dialogue> dialogues;

    List<OneDialogueEvent> finalDialogues;


	//List<Dialogue> dialoguesList;
	//int tmpcount = 0;

	public Dialogue[] Parse (string CSV_File)
    {
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);

        string[] stringData = csvData.text.Split(new char[] { '\n' });

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

		return dialogues.ToArray();
        
    }



	private void eventParse()
	{
		OneDialogueEvent tmpOnDialogueEvent = new OneDialogueEvent();

		for (int i = 0; i < dialogues.Count; i++)
		{
			if (!dialogues[i].Event.Equals("")) // 만약 내용에 이벤트가 있으면
			{
				// 이벤트 새로 선언
				tmpOnDialogueEvent = new OneDialogueEvent();
				tmpOnDialogueEvent.EventName = dialogues[i].Event;
			}
			tmpOnDialogueEvent.EventDialogues.Add(dialogues[i]);

			if(i+1 < dialogues.Count && !dialogues[i + 1].Event.Equals(""))
			{
				finalDialogues.Add(tmpOnDialogueEvent);
			}
			
		}
		finalDialogues.Add(tmpOnDialogueEvent);
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
		
		// *** 아래는 finalDialogues의 모든 요소를 디버그해주는 삼중중첩 반복문입니다.
		//Test가 아닐 때는 꼭 가려주세요
		//for (int i = 0; i < finalDialogues.Count; i++)
		//{
		//	Debug.Log($"{finalDialogues[i].EventName}");

		//	for(int j = 0; j < finalDialogues[i].EventDialogues.Count; j++)
		//	{

		//		for(int z = 0; z < finalDialogues[i].EventDialogues[j].contexts.Length; z++)
		//		{
		//			Debug.Log($"{finalDialogues[i].EventDialogues[j].contexts[z]}");
		//		}
		//	}

		//}

		//*** 아래는 dialogues의 요소를 하나씩 디버그 해주는 코드입니다
		//for (int j = 0; j < dialogues.Count; j++)
		//{
		//	Debug.Log($"{j}");

		//	Debug.Log($"{dialogues[j].Event}");
		//	Debug.Log($"{dialogues[j].name}");
		//	for (int i = 0; i < dialogues[j].contexts.Length; i++)
		//	{
		//		Debug.Log($"{dialogues[j].contexts[i]}");
		//	}
		//}

	}
}
