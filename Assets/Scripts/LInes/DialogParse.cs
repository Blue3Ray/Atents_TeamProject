using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogParse : Singleton<DialogParse>
{
	//이 클래스의 함수 OnDialog는
	//열의 제목과 행 번호 -1 을 넘겨주면
	//엑셀에서 거기에 맞는 대사(NowDialog)를 반환해줍니다.


	public string NowDialog
	{
		get => nowDialog;
	}



	protected List<Dictionary<string, object>> dataDialog;
	string nowDialog;

	protected virtual void Awake()
	{
		dataDialog = CSVReader.Read("Lines");
		

	}

	public string OnDialog(string Header, int row)
	{

		nowDialog = dataDialog[row][Header].ToString();
		return nowDialog;
	}
}
