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
    /// 레벨
    /// </summary>
    uint Level { get;set;}

    /// <summary>
    /// 경험치 확인 용 프로퍼티
    /// </summary>
    int Experience{get;set;}

    /// <summary>
    /// 최대 경험치 확인용 프로퍼티
    /// </summary>
    int ExperienceMax {get;}

    /// <summary>
    /// 경험치가 바뀔시 사용할 텔리게이트
    /// </summary>
    Action<uint, int, int> onChangeEx { get; set; }

   
    /// <summary>
    /// 레벨 업을 알리기 위한 데리게이트용 프로퍼티
    /// </summary>
    Action onLevelUP { get; set; }  



}
