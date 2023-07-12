using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : Singleton<DialogueParse>
{


	/// <summary>
	/// 두 줄짜리 대사를 집어넣을 리스트
	/// </summary>
    List<string> contextList;

	/// <summary>
	/// 이벤트, 이름, contextList가 들어가는 구조체의 리스트
	/// </summary>
    List<Dialogue> dialogues;

	/// <summary>
	/// 이벤트 하나에 알맞는 dialogues를 집어 넣을 리스트
	/// </summary>
    public List<OneDialogueEvent> finalDialogues;


	/// <summary>
	/// csv파일을 찾아 한 줄 씩 자르는 함수
	/// </summary>
	/// <param name="CSV_File"></param>
	/// <returns> Dialogue[] </returns>
	public Dialogue[] Parse (string CSV_File)
    {
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);            //파라메터로 받은 이름을 가진 csv 파일 불러 오기

		string[] stringData = csvData.text.Split(new char[] { '\n' });      //엔터 단위로 잘라서 문자 배열에 집어 넣기

		for (int i= 1; i < stringData.Length;)                              //한 줄을 ,로 쪼갠 다음 [이벤트, 이름, 연속 대사]끼리 같이 넣기
		{
			string[] row = stringData[i].Split(new char[] { ',' });         //한 줄 씩 , 단위로 자르기
			contextList.Clear();											//두 줄 씩 넣을 리스트 초기화
            Dialogue temp = new Dialogue();									//자른 걸 넣을 Dialogue 생성
            temp.Event = row[0];											//첫 번째 셀을 Event에 save
            temp.name = row[1];												//두 번째 셀을 이름에 save

			do
            {
				
                contextList.Add(row[2]);                                    //일단은 두 번째 셀을 연속대사에 save
				if (++i < stringData.Length)								//다음줄이 있으면 , 별로 쪼개기
				{
                    
                    row = stringData[i].Split(new char[] { ',' });			
				}
																			//다음줄이 없으면 break
                else
                {
                    break;
                }
			} while (row[0].ToString() == "" && row[1].ToString() == "");	//다음 줄이 이벤트와 이름이 공백이면 do 반복

            temp.contexts = contextList.ToArray();							//배열은 크기를 미리 설정해야 해서 list에서 배열로 전환
			dialogues.Add(temp);											//Dialogue의 리스트에 추가
		}

		return dialogues.ToArray();											//Dialogue 리스트를 배열로 바꿔서 리턴
        
    }


	/// <summary>
	/// Parse에서 생성된 Dialogue 배열을 이벤트별로 묶은 구조에 저장하는 함수
	/// </summary>
	private void eventParse()
	{
		OneDialogueEvent tmpOnDialogueEvent = new OneDialogueEvent();		//이벤트와 Dialogue 리스트를 가진 구조체 new

		for (int i = 0; i < dialogues.Count; i++)							//다이얼로그 리스트 개수 내에서만 반복되는 for문
		{
			if (!dialogues[i].Event.Equals(""))								//이벤트가 공백이 아니면
			{
				tmpOnDialogueEvent = new OneDialogueEvent();				//초기화 한 번 해주고
				tmpOnDialogueEvent.EventName = dialogues[i].Event;			//이벤트 이름 설정
			}
			tmpOnDialogueEvent.EventDialogues.Add(dialogues[i]);			//그 줄을 Dialogue의 리스트에 추가

			if(i+1 < dialogues.Count && !dialogues[i + 1].Event.Equals(""))	//다음줄이 존재하면서 공백도 아니면
			{
				finalDialogues.Add(tmpOnDialogueEvent);						//이벤트의 마지막 대사이므로 리스트 추가 후 반복
			}
			
		}
		finalDialogues.Add(tmpOnDialogueEvent);								//다음 줄이 존재하지 않을 때 if에 들어가지 않으므로 add
	}


	private void Awake()
	{
		dialogues = new List<Dialogue>();
		contextList = new List<string>();
        finalDialogues = new List<OneDialogueEvent>();
        Parse("Lines0705");
		eventParse();

	}

	
	//	//***아래는 finalDialogues의 모든 요소를 디버그해주는 삼중중첩 반복문입니다.
	//	//Test가 아닐 때는 꼭 가려주세요
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
