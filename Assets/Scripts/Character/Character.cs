using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
}