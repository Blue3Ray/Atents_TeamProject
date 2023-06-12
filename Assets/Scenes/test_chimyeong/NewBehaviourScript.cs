using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
       
  
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = new Vector2(-1,rigid.velocity.y); // 단순 왼쪽방향 이동
    }
}
