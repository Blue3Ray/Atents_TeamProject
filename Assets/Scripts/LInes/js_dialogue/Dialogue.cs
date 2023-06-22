using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue 
{
    [Tooltip("��� ġ�� ĳ���� �̸�")]
    public string name;
    
    [Tooltip("��� ����")]
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



