using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IExperience
{ 
    /// <summary>
    /// ����
    /// </summary>
    uint Level { get;set;}

    /// <summary>
    /// ����ġ Ȯ�� �� ������Ƽ
    /// </summary>
    int Experience{get;set;}

    /// <summary>
    /// �ִ� ����ġ Ȯ�ο� ������Ƽ
    /// </summary>
    int ExperienceMax {get;}

    /// <summary>
    /// ����ġ�� �ٲ�� ����� �ڸ�����Ʈ
    /// </summary>
    Action<uint, int, int> onChangeEx { get; set; }

   
    /// <summary>
    /// ���� ���� �˸��� ���� ��������Ʈ�� ������Ƽ
    /// </summary>
    Action onLevelUP { get; set; }  



}
