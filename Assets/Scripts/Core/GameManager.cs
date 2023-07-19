using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element : byte
{ 
    Fire = 1,
    WInd = 2,
    Water = 4,
    Thunder = 8
}


public class GameManager : Singleton<GameManager>
{
    PlayerTest playerTest;

    private void Awake()
    {
        //playerTest = new PlayerTest();
    }
}
