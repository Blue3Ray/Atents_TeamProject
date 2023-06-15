using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMDialogueParse : MonoBehaviour
{

    List<string> contextList;

    GMDialogue gmDialogue;


	public GMDialogue[] Parse (string CSV_File)
    {
        
        //이 함수가 반환하는 건 GMDialogue의 배열이고
        //여기선 GMDialogue들의 리스트 이다!
        List<GMDialogue> dialoguesList = new List<GMDialogue>();
        
        //TextAsset 타입으로 입력받은 이름의 csv 파일을 찾아서
        //csvData라는 변수에 집어넣었다.
        TextAsset csvData = Resources.Load<TextAsset>(CSV_File);

        //아까 가져온 파일을 {}안에 들어간 걸 기준으로 나눠서
        //새로운 char배열 즉 csv에서 한 줄씩 일단 빼온다.
        string[] stringData = csvData.text.Split(new char[] { '\n' });

        //위에서 한 행 씩 쪼갰으니
        //여기서는 그 행에서 콤마별로 쪼갠다.
        for(int i= 1; i < stringData.Length;)
        {
            
            string[] row = stringData[i].Split(new char[] { ',' });

            //gmDialogue.name = row[1];


            //contextList.Add(row[2]);

			//Debug.Log($"{row[0]}");         //i번째 행의 1번째 열
			//Debug.Log($"{row[1]}");         //i번째 행의 1번째 열
			//Debug.Log($"{row[2]}");         //i번째 행의 1번째 열

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
