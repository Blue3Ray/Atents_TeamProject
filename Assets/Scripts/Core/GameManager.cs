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
    PlayerJS player;
    public PlayerJS Player => player;
    
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
            player.inven = new Inventory(5,2);
        }
	}



    SaveData saveData;

    public void SavePlayerData(PlayerJS player)
    {
        // 저장해야 되는 데이터
        // 현재 체력, 최대체력, 현재 마나, 최대 마나, 경험치, 최대경험치, 원소 속성별 레벨, 인벤토리,
        saveData = new SaveData();
        saveData.level = player.Level;
        saveData.exper = player.Experience;
        saveData.exper_max = player.ExperienceMax;
        saveData.hp = player.HP;
        saveData.hp_max = player.MaxHP;
        saveData.mp = player.MP;
        saveData.mp_max = player.MaxMP;
        saveData.elementLevel = new int[player.ElemantalStates.elemantalevels.Length];
        
        for(int i =0; i < saveData.elementLevel.Length; i++)
        {
            saveData.elementLevel[i] = player.ElemantalStates.elemantalevels[i];
        }
    }

    public void LoadPlayerData(PlayerJS player)
    {
        player.Loadtate(saveData);
        saveData = null;
    }
}
public class SaveData
{
    public uint level;
    public int exper;
    public int exper_max;
    public float hp;
    public float hp_max;
    public float mp;
    public float mp_max;
    public int[] elementLevel;
}
