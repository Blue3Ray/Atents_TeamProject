using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 오브젝트 풀에 들어갈 오브젝트들이 상속받을 클래스
/// </summary>
public class PooledObject : MonoBehaviour
{
    /// <summary>
    /// 이 게임 오브젝트가 비활성화 될 때 실행되는 델리게이트
    /// </summary>
    public System.Action onDisable;

    protected virtual void OnEnable()
    {
        transform.localPosition = Vector3.zero;                 // 위치와 회전 초기화
        transform.localRotation = Quaternion.identity;
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();
    }

    /// <summary>
    /// 일정시간 후 오브젝트 비활성화
    /// </summary>
    /// <param name="delay">비활성화 지연시킬 시간</param>
    /// <returns></returns>
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
