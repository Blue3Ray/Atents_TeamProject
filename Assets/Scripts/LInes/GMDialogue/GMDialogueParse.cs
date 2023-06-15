using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMDialogueParse : MonoBehaviour
{

    List<string> contextList;

    GMDialogue gmDialogue;


	public GMDialogue[] Parse (string CSV_File)
    {
        
        //�� �Լ��� ��ȯ�ϴ� �� GMDialogue�� �迭�̰�
        //���⼱ GMDialogue���� ����Ʈ �̴�!
        List<GMDialogue> dialoguesList = new List<GMDialogue>();
        
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

            //gmDialogue.name = row[1];


            //contextList.Add(row[2]);

			//Debug.Log($"{row[0]}");         //i��° ���� 1��° ��
			//Debug.Log($"{row[1]}");         //i��° ���� 1��° ��
			//Debug.Log($"{row[2]}");         //i��° ���� 1��° ��

			if (++i<stringData.Length)
            {
                ;
            }
        }

        return dialoguesList.ToArray();
    }

	private void Awake()
	{
        contextList = new List<string>();
        gmDialogue = new GMDialogue();


	}

	private void Start()
	{
        Parse("Lines");
	}
}
