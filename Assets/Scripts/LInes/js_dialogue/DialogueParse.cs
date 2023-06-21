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
        
        //이 함수가 반환하는 건 GMDialogue의 배열이고
        //여기선 GMDialogue들의 리스트 이다!
        //List<Dialogue> dialoguesList = new List<Dialogue>();
        
        //TextAsset 타입으로 입력받은 이름의 csv 파일을 찾아서
        //csvData라는 변수에 집어넣었다.
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);

        //아까 가져온 파일을 {}안에 들어간 걸 기준으로 나눠서
        //새로운 char배열 즉 csv에서 한 줄씩 일단 빼온다.
        string[] stringData = csvData.text.Split(new char[] { '\n' });

		//dialogues = new List<Dialogue>();

        //위에서 한 행 씩 쪼갰으니
        //여기서는 그 행에서 콤마별로 쪼갠다.
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
				//Debug.Log($"두와일: {row[2]}");
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
