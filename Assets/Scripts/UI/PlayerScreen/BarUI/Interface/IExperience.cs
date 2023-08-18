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
    // 경험치 확인 용 프로퍼티
    float Experience{get;set;}

    // 최대 경험치 확인용 프로퍼티
    float ExperienceMax {get;}

    int Level { get;set;}

    Action<float, float, int> onChangeEx { get; set; }

   

    // 레벨 업을 알리기 우한 데리게이트용 프로퍼티
    Action onLevelUP { get; set; }  



}
