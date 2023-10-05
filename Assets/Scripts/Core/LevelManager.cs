using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// ���� ���� ����
    /// </summary>
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
            }
            else
            {
                LoadLowLevel();
            }
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public int bossLevel;


    /// <summary>
    /// ��� ���� �� �ҷ�����
    /// </summary>
    void LoadLowLevel()
    {
        GameManager.Ins.SavePlayerData(GameManager.Ins.Player);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// ������ �� �ҷ�����
    /// </summary>
    void LoadBossRoom()
    {
        SceneManager.LoadScene(1);
    }
}
