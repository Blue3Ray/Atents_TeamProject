using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  

   public void ElemanterSelct() 
   {
        switch(eTpye)
        {
            case ElemanterType.button1:
                Debug.Log("button1 ����");
                break;
            case ElemanterType.button2:
                Debug.Log("button2 ����");
                break;
            case ElemanterType.button3:
                Debug.Log("button3 ����");
                break;
            case ElemanterType.button4:
                Debug.Log("button4 ����");
                break;
        }
   }

}
