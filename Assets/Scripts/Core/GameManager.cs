using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 테스트 용으로 만들어놓은 임시 Player 클래스 나중에 재민씨가 클래스 만들면 없앨것
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
    Player player;

    private void Awake()
    {
        player = new Player();
    }
}
