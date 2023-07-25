using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element : byte
{ 
    None = 0,
    Fire = 1,
    Wind = 2,
    Water = 4,
    Thunder = 8
}


public class GameManager : Singleton<GameManager>
{
    PlayerTest playerTest;
    ItemDataManager itemDataManager;

    public ItemDataManager ItemData => itemDataManager;

    private void Awake()
    {
        itemDataManager = GetComponent<ItemDataManager>();
        
        playerTest = new PlayerTest();
    }
}
