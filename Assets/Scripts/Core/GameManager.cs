using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element
{ 
    None,
    Fire,
    WInd,
    Water,
    Thunder
}



public class Player : MonoBehaviour
{
    int hp = 100;
    Element element  = Element.None;
    public int HP
    {
        get { return hp; } set {  hp = value; }
    }
}


public class GameManager : Singleton<GameManager>
{
    PlayerTest playerTest;

    private void Awake()
    {
        //playerTest = new PlayerTest();
    }
}
