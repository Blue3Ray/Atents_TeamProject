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

        return dialogues.ToArray();
        
    }

    private void eventParse()
    {

    }


	private void Awake()
	{
		dialogues = new List<Dialogue>();
		contextList = new List<string>();


	}

	private void Start()
	{
        Parse("Lines");
        eventParse();

        //      foreach (Dialogue tdialogue in dialogues)
        //      {
        //          tmpcount = tdialogue.count;
        //	Debug.Log($"{tmpcount}");
        //          Debug.Log($"{tdialogue.name}");

        //}
        for (int j = 0; j < dialogues.Count; j++)
        {
            Debug.Log($"{dialogues[j].Event}");
        }
		//Debug.Log($"{dialoguesList[2].name}");
	}
}
