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
        Vector3 mousepostion = Mouse.current.position.ReadValue();

        if(mousepostion == elemanterbutton1.transform.position)
        {
            Debug.Log("1");
        }
        else if(mousepostion == elemanterbutton2.transform.position)
        {
            Debug.Log("2");
        }
        else if(mousepostion == elemanterbutton3.transform.position)
        {
            Debug.Log("3");
        }
        else if(mousepostion == elemanterbutton4.transform.position)
        {
            Debug.Log("4");
        }
       

    }

   
}
