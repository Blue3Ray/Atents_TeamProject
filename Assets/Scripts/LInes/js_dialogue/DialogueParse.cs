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

        //������ �� �� �� �ɰ�����
        //���⼭�� �� �࿡�� �޸����� �ɰ���.
        for(int i= 1; i < stringData.Length;)
        {
            
            string[] row = stringData[i].Split(new char[] { ',' });
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

        dialogueCount = dialogues.Count;
		return dialogues.ToArray();
        
    }



    private void eventParse()
    {
        for (int i = 0; i < dialogues.Count;)
        {
            OneDialogueEvent tmpOnDialogueEvent = new OneDialogueEvent();

            if (dialogues[i].Event.ToString() != "")
            {
                Debug.Log(i);
                tmpOnDialogueEvent.EventName = dialogues[i].Event;
                Debug.Log($"{tmpOnDialogueEvent.EventName}");

                do
                {
                    //count �������� dialogue�� ���ϱ� null exeption�� �B��
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

        //for (int j = 0; j < dialogues.Count; j++)
        //{
        //    Debug.Log($"{dialogues[j].name}");
        //}
		
	}
}
