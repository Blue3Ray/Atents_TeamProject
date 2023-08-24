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
	public PlayerJM playerTest;
    public PlayerJS playerTest1;
    public Player_JS playerTest2;
    
    ItemDataManager itemDataManager;
    
    public ItemDataManager ItemData => itemDataManager;

    public bool IsRight => !playerTest1.spriteRenderer.flipX;

    private void Awake()
    {
        itemDataManager = GetComponent<ItemDataManager>();
        
    }

	protected override void OnInitalize()
	{
		base.OnInitalize();
        playerTest = FindObjectOfType<PlayerJM>();
        if(playerTest == null)
        {
            playerTest1 = FindObjectOfType<PlayerJS>();
            if(playerTest1 == null)
            {
                playerTest2 = FindAnyObjectByType<Player_JS>();
            }
        }
        if(playerTest != null)
        {
            playerTest.inven = new Inventory(7);
        }

	}


}
