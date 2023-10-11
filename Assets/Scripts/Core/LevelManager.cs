using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// 현재 던전 레벨
    /// </summary>
    int level;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            RoomGenerator.Ins.ClearData();
            if (level % 3 == 0)
            {
                LoadBossRoom();
            }
            else
            {
                LoadLowLevel();
            }
        }
    }

    /// <summary>
    /// 보스방 레벨
    /// </summary>
    public int bossLevel;


    

    /// <summary>
    /// 평상 던전 씬 불러오기
    /// </summary>
    void LoadLowLevel()
    {
        
        GameManager.Ins.SavePlayerData(GameManager.Ins.Player);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 보스방 씬 불러오기
    /// </summary>
    void LoadBossRoom()
    {
        SceneManager.LoadScene(1);
    }
}
