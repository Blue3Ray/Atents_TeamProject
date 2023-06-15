using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GMDialogue 
{
    [Tooltip("대사 치는 캐릭터 이름")]
    public string name;
    
    [Tooltip("대사 내용")]
    public string[] contexts;

}

[System.Serializable]
public class GMDialogueEvent
{
    public string name;

    /// <summary>
    /// 몇 줄 부터 몇 줄까지 가져올지를 설정할 변수
    /// </summary>
    public Vector2 line;

    /// <summary>
    /// 대사 데이터를 배열로 저장하는 것(대화별)
    /// </summary>
    public GMDialogue[] gmDialogues;
}
