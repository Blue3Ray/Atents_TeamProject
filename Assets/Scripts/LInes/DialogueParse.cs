using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{
    
	private void Awake()
	{
		List<Dictionary<string, object>> dataDialog = CSVReader.Read("Lines");
		
		for(int i = 0; i < dataDialog.Count; i++)
		{
		
			print(dataDialog[i]["Event Name"].ToString());

		}
		


	}
}
