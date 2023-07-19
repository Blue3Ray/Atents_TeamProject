using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
}
