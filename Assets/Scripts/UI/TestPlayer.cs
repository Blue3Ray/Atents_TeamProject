using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
    
    public Element element = Element.None;

 
    
   
}
