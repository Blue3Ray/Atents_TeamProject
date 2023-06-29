using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �׽�Ʈ ������ �������� �ӽ� Player Ŭ���� ���߿� ��ξ��� Ŭ���� ����� ���ٰ�
/// </summary>
public class Player : MonoBehaviour
{
    int hp = 100;
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
