using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp_Bar : MonoBehaviour
{
    public Transform Enemy;
    public Slider hpbar;
    public float maxHp;
    public float currenthp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Enemy.position);
        hpbar.value = currenthp / maxHp;

    }
}
