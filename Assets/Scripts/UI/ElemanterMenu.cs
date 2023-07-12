using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElemanterMenu : MonoBehaviour
{
 


    public Button elemanterbutton1;
    public Button elemanterbutton2;
    public Button elemanterbutton3;
    public Button elemanterbutton4;


    Fire fire;
    Water water;
    Thunder thunder;
    Wind wind;

    private void Awake()
    {

        fire = GetComponentInChildren<Fire>();
        water = GetComponentInChildren<Water>();
        thunder = GetComponentInChildren<Thunder>();
        wind = GetComponentInChildren<Wind>();
    }


    public void ElemanterSelct() 
   {
     

   }

   
}
