using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class TestPlayer : MonoBehaviour
{
    int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    Fire fire;
    Wind wind;
    Water water;
    Thunder thunder;

    public Element element = Element.None;
    
    
    }

 
    
   

