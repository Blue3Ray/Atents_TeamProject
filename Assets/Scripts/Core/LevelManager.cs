using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    int level;
    public int Level
    {
        get => level;
        set
        { 
            level = value;
            if (level > 2)
            {
                LoadBossRoom();           
            }else
            {
                LoadLowLevel();
            }
        }
    }

    void LoadLowLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadBossRoom()
    {
        SceneManager.LoadScene(1);
    }
}
