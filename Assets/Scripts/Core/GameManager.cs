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
    public PlayerJS player;
    
    ItemDataManager itemDataManager;
    
    public ItemDataManager ItemData => itemDataManager;

    private void Awake()
    {
        itemDataManager = GetComponent<ItemDataManager>();
        
    }

	protected override void OnInitalize()
	{
		base.OnInitalize();
        player = FindObjectOfType<PlayerJS>();
       
        if(player != null)
        {
            player.inven = new Inventory(7);
        }

	}


}
