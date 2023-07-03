using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ElemanterType
{
    button1,
    button2,
    button3,
    button4
}
public class ElemanterMenu : MonoBehaviour
{
    public ElemanterType eTpye;

    Button button1;
    Button button2;
    Button button3;
    Button button4;

    private void Awake()
    {
        
    }

    public void ElemanterSelct() 
    {
        switch(eTpye)
        {
            case ElemanterType.button1:
                Debug.Log("button1 선택");
                break;
            case ElemanterType.button2:
                Debug.Log("button2 선택");
                break;
            case ElemanterType.button3:
                Debug.Log("button3 선택");
                break;
            case ElemanterType.button4:
                Debug.Log("button4 선택");
                break;
        }
    }

}
