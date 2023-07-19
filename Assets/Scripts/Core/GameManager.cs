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


public class GameManager : Singleton<GameManager>
{
    PlayerTest playerTest;

    private void Awake()
    {
        //playerTest = new PlayerTest();
    }
}
