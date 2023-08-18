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
    // ����ġ Ȯ�� �� ������Ƽ
    float Experience{get;set;}

    // �ִ� ����ġ Ȯ�ο� ������Ƽ
    float ExperienceMax {get;}

    int Level { get;set;}

    Action<float, float, int> onChangeEx { get; set; }

   

    // ���� ���� �˸��� ���� ��������Ʈ�� ������Ƽ
    Action onLevelUP { get; set; }  



}
