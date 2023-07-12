using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어로부터 클릭에 반응해야 하는 인터페이스
/// </summary>
public interface IClickable
{
    public void OnClicking(IClickable tmp);
}
