using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue 
{
    [Tooltip("대사 치는 캐릭터 이름")]
    public string name;
    
    [Tooltip("대사 내용")]
    public string[] contexts;

    public string Event;

}

[System.Serializable]
public class OneDialogueEvent
{
    public OneDialogueEvent()
    {
        EventDialogues = new List<Dialogue>();
    }
        
    public string EventName;

    public List<Dialogue> EventDialogues;
}



