using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GMDialogue 
{
    [Tooltip("��� ġ�� ĳ���� �̸�")]
    public string name;
    
    [Tooltip("��� ����")]
    public string[] contexts;

}

[System.Serializable]
public class GMDialogueEvent
{
    public string name;

    /// <summary>
    /// �� �� ���� �� �ٱ��� ���������� ������ ����
    /// </summary>
    public Vector2 line;

    /// <summary>
    /// ��� �����͸� �迭�� �����ϴ� ��(��ȭ��)
    /// </summary>
    public GMDialogue[] gmDialogues;
}
