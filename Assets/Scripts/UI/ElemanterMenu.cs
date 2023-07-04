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

    public Button elemanterbutton1;
    public Button elemanterbutton2;
    public Button elemanterbutton3;
    public Button elemanterbutton4;

    Elemanter1 elemanter1;
    Elemanter2 elemanter2;
    Elemanter3 elemanter3;
    Elemanter4 elemanter4;


    private void Awake()
    {
        elemanterbutton1 = gameObject.AddComponent<Button>();
        elemanterbutton2 = gameObject.AddComponent<Button>();
        elemanterbutton3 = gameObject.AddComponent<Button>();
        elemanterbutton4 = gameObject.AddComponent<Button>();

        elemanter1 = new Elemanter1();
        elemanter2 = new Elemanter2();
        elemanter3 = new Elemanter3();
        elemanter4 = new Elemanter4();
    }

    public void ElemanterSelct() 
   {
        GameObject result;

       switch(eTpye)
        {
            case ElemanterType.button1:
                elemanterbutton1.Select();
                break;
            case ElemanterType.button2:
                elemanterbutton2.Select();
                break;
            case ElemanterType.button3:
                elemanterbutton3.Select();
                 break;
            case ElemanterType.button4:
                 elemanterbutton4.Select();
                 break;
            default:
                break;
        }
   }

}
