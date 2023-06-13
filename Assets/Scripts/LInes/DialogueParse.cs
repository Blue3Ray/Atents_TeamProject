using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{
	protected List<Dictionary<string, object>> dataDialog;


	protected virtual void Awake()
	{
		dataDialog = CSVReader.Read("Lines");
		

	}
}
