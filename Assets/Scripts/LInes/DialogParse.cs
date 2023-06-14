using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogParse : Singleton<DialogParse>
{
	//�� Ŭ������ �Լ� OnDialog��
	//���� ����� �� ��ȣ -1 �� �Ѱ��ָ�
	//�������� �ű⿡ �´� ���(NowDialog)�� ��ȯ���ݴϴ�.


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
