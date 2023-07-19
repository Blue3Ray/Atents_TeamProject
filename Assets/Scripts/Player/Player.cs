using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Element element = Element.None;

    int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
}
