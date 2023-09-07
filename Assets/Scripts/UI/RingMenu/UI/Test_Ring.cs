using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test_Ring : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
    }

    private void Awake()
    {
        Debug.Log("awake");
    }
}
