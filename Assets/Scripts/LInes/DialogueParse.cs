using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : Singleton<DialogueParse>
{


	/// <summary>
	/// �� ��¥�� ��縦 ������� ����Ʈ
	/// </summary>
    List<string> contextList;

	/// <summary>
	/// �̺�Ʈ, �̸�, contextList�� ���� ����ü�� ����Ʈ
	/// </summary>
    List<Dialogue> dialogues;

	/// <summary>
	/// �̺�Ʈ �ϳ��� �˸´� dialogues�� ���� ���� ����Ʈ
	/// </summary>
    public List<OneDialogueEvent> finalDialogues;


	/// <summary>
	/// csv������ ã�� �� �� �� �ڸ��� �Լ�
	/// </summary>
	/// <param name="CSV_File"></param>
	/// <returns> Dialogue[] </returns>
	public Dialogue[] Parse (string CSV_File)
    {
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);            //�Ķ���ͷ� ���� �̸��� ���� csv ���� �ҷ� ����

		string[] stringData = csvData.text.Split(new char[] { '\n' });      //���� ������ �߶� ���� �迭�� ���� �ֱ�

		for (int i= 1; i < stringData.Length;)                              //�� ���� ,�� �ɰ� ���� [�̺�Ʈ, �̸�, ���� ���]���� ���� �ֱ�
		{
			string[] row = stringData[i].Split(new char[] { ',' });         //�� �� �� , ������ �ڸ���
			contextList.Clear();											//�� �� �� ���� ����Ʈ �ʱ�ȭ
            Dialogue temp = new Dialogue();									//�ڸ� �� ���� Dialogue ����
            temp.Event = row[0];											//ù ��° ���� Event�� save
            temp.name = row[1];												//�� ��° ���� �̸��� save

			do
            {
				
                contextList.Add(row[2]);                                    //�ϴ��� �� ��° ���� ���Ӵ�翡 save
				if (++i < stringData.Length)								//�������� ������ , ���� �ɰ���
				{
                    
                    row = stringData[i].Split(new char[] { ',' });			
				}
																			//�������� ������ break
                else
                {
                    break;
                }
			} while (row[0].ToString() == "" && row[1].ToString() == "");	//���� ���� �̺�Ʈ�� �̸��� �����̸� do �ݺ�

            temp.contexts = contextList.ToArray();							//�迭�� ũ�⸦ �̸� �����ؾ� �ؼ� list���� �迭�� ��ȯ
			dialogues.Add(temp);											//Dialogue�� ����Ʈ�� �߰�
		}

		return dialogues.ToArray();											//Dialogue ����Ʈ�� �迭�� �ٲ㼭 ����
        
    }


	/// <summary>
	/// Parse���� ������ Dialogue �迭�� �̺�Ʈ���� ���� ������ �����ϴ� �Լ�
	/// </summary>
	private void eventParse()
	{
		OneDialogueEvent tmpOnDialogueEvent = new OneDialogueEvent();		//�̺�Ʈ�� Dialogue ����Ʈ�� ���� ����ü new

		for (int i = 0; i < dialogues.Count; i++)							//���̾�α� ����Ʈ ���� �������� �ݺ��Ǵ� for��
		{
			if (!dialogues[i].Event.Equals(""))								//�̺�Ʈ�� ������ �ƴϸ�
			{
				tmpOnDialogueEvent = new OneDialogueEvent();				//�ʱ�ȭ �� �� ���ְ�
				tmpOnDialogueEvent.EventName = dialogues[i].Event;			//�̺�Ʈ �̸� ����
			}
			tmpOnDialogueEvent.EventDialogues.Add(dialogues[i]);			//�� ���� Dialogue�� ����Ʈ�� �߰�

			if(i+1 < dialogues.Count && !dialogues[i + 1].Event.Equals(""))	//�������� �����ϸ鼭 ���鵵 �ƴϸ�
			{
				finalDialogues.Add(tmpOnDialogueEvent);						//�̺�Ʈ�� ������ ����̹Ƿ� ����Ʈ �߰� �� �ݺ�
			}
			
		}
		finalDialogues.Add(tmpOnDialogueEvent);								//���� ���� �������� ���� �� if�� ���� �����Ƿ� add
	}


	private void Awake()
	{
		dialogues = new List<Dialogue>();
		contextList = new List<string>();
        finalDialogues = new List<OneDialogueEvent>();
        Parse("Lines0705");
		eventParse();

	}

	
	//	//***�Ʒ��� finalDialogues�� ��� ��Ҹ� ��������ִ� ������ø �ݺ����Դϴ�.
	//	//Test�� �ƴ� ���� �� �����ּ���
	//	//for (int i = 0; i < finalDialogues.Count; i++)
	//	//{
	//	//	Debug.Log($"{finalDialogues[i].EventName}");

	//	//	for (int j = 0; j < finalDialogues[i].EventDialogues.Count; j++)
	//	//	{

	//	//		for (int z = 0; z < finalDialogues[i].EventDialogues[j].contexts.Length; z++)
	//	//		{
	//	//			Debug.Log($"{finalDialogues[i].EventDialogues[j].contexts[z]}");
	//	//		}
	//	//	}

	//	//}

}
