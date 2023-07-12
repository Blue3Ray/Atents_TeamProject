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

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    Fire fire;
    Water water;
    Thunder thunder;
    Wind wind;

    private void Awake()
    {
        button1 = GetComponent<Button>();
        button2 = GetComponent<Button>();
        button3 = GetComponent<Button>();
        button4 = GetComponent<Button>();

        fire = GetComponent<Fire>();
        water = GetComponent<Water>();
        thunder = GetComponent<Thunder>();
        wind = GetComponent<Wind>();
    }


    public void ElemanterSelct() 
   {
      if( fire != null )
        {
            fire.OnClick();
        }
      else if( water != null )
        { 
            water.OnClick();
        }
      else if ( wind != null )
        {
            wind.OnClick();
        }
      else if (thunder != null)
        {
            thunder.OnClick();
        }

   }

   
}
