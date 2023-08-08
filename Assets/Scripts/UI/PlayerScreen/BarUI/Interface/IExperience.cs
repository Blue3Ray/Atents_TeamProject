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
    float Experience{get;set;}
    float ExperienceMax {get;set;}

    int LevelingIndex { get; }

}
